using System.Diagnostics;
using challenge.ibge.infra.data.Converters;
using challenge.ibge.infra.data.Dtos;
using challenge.ibge.infra.data.Entities;
using challenge.ibge.infra.data.Services.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace challenge.ibge.infra.data.Services;

public class LocalityService : ILocalityService
{
    private readonly MySqlDbContext _mySqlDbContext;
    private readonly DbSet<Locality> _dbSet;
    private readonly ILogger<LocalityService> _logger;
    private readonly ILocalityValidationService _localityValidationService;

    public LocalityService(MySqlDbContext mySqlDbContext, ILogger<LocalityService> logger, ILocalityValidationService localityValidationService)
    {
        _mySqlDbContext = mySqlDbContext;
        _logger = logger;
        _localityValidationService = localityValidationService;
        _dbSet = mySqlDbContext.Ibges;
    }

    public async Task<LocalityDto> CreateAsync(LocalityDto localityDto)
    {
        var ibge = new Locality(localityDto);

        await InitDbOperation(() =>
        {
            _dbSet.AddAsync(ibge);
        });

        return ibge.ToDto();
    }

    public async Task<LocalityDto> UpdateAsync(int id, LocalityDto localityDto)
    {
        var dbIbge = _dbSet.Single(ibge => ibge.Id == id);
        
        await InitDbOperation(() =>
        {
            dbIbge.Update(localityDto);
            _dbSet.Update(dbIbge);
        });

        return dbIbge.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var dbIbge = _mySqlDbContext.Ibges.Single(ibge => ibge.Id == id);

        await InitDbOperation(() => { _dbSet.Remove(dbIbge); });
    }

    public async Task<LocalityDto?> GetByIdAsync(int id)
    {
        var dbIbge = await _dbSet.FirstOrDefaultAsync(ibge => ibge.Id == id);
        return dbIbge?.ToDto();
    }

    public async Task<List<LocalityDto>> GetAllAsync(string? filter)
    {
        var dbIbges = await _dbSet
            .Where(x =>
                filter == null ||
                (x.IbgeCode.ToString().Contains(filter) ||
                 x.City.Contains(filter) ||
                 x.UF.Contains(filter)))
            .ToListAsync();

        return dbIbges.Select(dbIbge => dbIbge.ToDto()).ToList();
    }

    public async Task<LocalityImportResult> ImportAsync(IFormFile file)
    {
        var fileExtension = Path.GetExtension(file.FileName);

        var validExtensions = new[] { ".xls", ".xlxs" };

        if (validExtensions.Contains(fileExtension) == false)
        {
            throw new Exception("The extension of file to import must be: .xls or .xlxs");
        }

        var localityDtos = new List<LocalityDto>();
        var localityFailedToImportDtos = new List<LocalityValidationResultDto>();

        const int ufColumnPosition = 2;
        const int ibgeCodeColumnPosition = 3;
        const int cityNameColumnPosition = 4;

        await using var stream = file.OpenReadStream();
        var workbook = new HSSFWorkbook(stream);
        var sheet = workbook.GetSheetAt(0);

        int x = 0;
        foreach (IRow row in sheet)
        {
            x += 1;
            var cols = row.Cells;
            
            if (row.RowNum is 0) //First line (headers)        
            {
                continue;
            }

            if (cols.Count < 4) //Must have 4 columns to be valid locality
            {
                localityFailedToImportDtos.Add(new LocalityValidationResultDto()
                {
                    Row = row.RowNum
                });
                
                continue;
            }


            //0       1           2                 3                          4
            //UF	Nome_UF		         Código Município Completo	     Nome_Município
            //11	Rondônia	RO	             1100015	          Alta Floresta D''Oeste

            var (uf, ibgeCode, cityName) = (
                cols[ufColumnPosition].ToString(),
                cols[ibgeCodeColumnPosition].ToString(),
                cols[cityNameColumnPosition].ToString());

            var localityDto = new LocalityDto()
            {
                Code = int.Parse(ibgeCode),
                City = cityName,
                UF = uf
            };

            var localityValidationResultDto = await _localityValidationService.ValidateCanImport(localityDto);
            
            if (localityValidationResultDto.IsValid)
            {
                localityDtos.Add(localityValidationResultDto.Locality);
            }
            else
            {
                localityValidationResultDto.Row = row.RowNum;
                localityFailedToImportDtos.Add(localityValidationResultDto);
            }
        }

        var dbIbges = await _dbSet.ToListAsync();
        _logger.LogInformation("Current total IBGE's in database: {CurrentTotalIbgesInDb}", dbIbges.Count);

        var dbIbgeCodes = dbIbges.Select(x => x.IbgeCode);
        var dtoIbgeCodes = localityDtos.Select(x => x.Code);
        var ibgeCodesToRemove = dbIbgeCodes.Intersect(dtoIbgeCodes).ToList();
        var ignoredCount = ibgeCodesToRemove.Count(); 
        
        localityDtos.RemoveAll(ibgeDto => ibgeCodesToRemove.Any(code => ibgeDto.Code == code)); // Remove existents ibges in database
        _logger.LogInformation("Total IBGE's ignored because already exists in database: {TotalIgnoredIbgesThatAlreadyExistsInDb}", ignoredCount);
            
        var ibges = localityDtos.Select(ibgeDto => new Locality(ibgeDto)).ToList();
        _logger.LogInformation("Total IBGE's will be created: {TotalIbgesThatWillBeCreated}", ibges.Count);
        
        var sw = new Stopwatch();
        sw.Start();
        await _mySqlDbContext.BulkInsertAsync(ibges);
        sw.Stop();
            
        _logger.LogInformation("Total localities failed to import: {TotalLocalityFailCount}", localityFailedToImportDtos.Count);
        _logger.LogInformation("Elapsed time to import {ImportedCount}: {ImportElapsedTime}",localityDtos.Count ,sw.Elapsed);
            
        return new LocalityImportResult
        {
            Elapsed = sw.Elapsed,
            CreatedCount = ibges.Count,
            IgnoredCount = ignoredCount < 0 ? 0 : ignoredCount,
            FailedCount = localityFailedToImportDtos.Count,
            FailedLocalities = localityFailedToImportDtos 
        };
    }


    private async Task InitDbOperation(Action act)
    {
        act.Invoke();
        await _mySqlDbContext.SaveChangesAsync();
    }
}