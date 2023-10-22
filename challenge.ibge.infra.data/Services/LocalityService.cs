using System.Diagnostics;
using challenge.ibge.infra.data.Converters;
using challenge.ibge.infra.data.Dtos;
using challenge.ibge.infra.data.Entities;
using challenge.ibge.infra.data.Services.Interfaces;
using challenge.ibge.infra.data.UnitOfWork.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace challenge.ibge.infra.data.Services;

public class LocalityService : ILocalityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LocalityService> _logger;
    private readonly ILocalityValidationService _localityValidationService;

    public LocalityService(IUnitOfWork unitOfWork, ILogger<LocalityService> logger,
        ILocalityValidationService localityValidationService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _localityValidationService = localityValidationService;
    }

    public async Task<LocalityDto> CreateAsync(LocalityDto localityDto)
    {
        var locality = new Locality(localityDto);

        _unitOfWork.LocalityRepository.Add(locality);
        await _unitOfWork.SaveChangesAsync();

        return locality.ToDto();
    }

    public async Task<LocalityDto> UpdateAsync(int id, LocalityDto localityDto)
    {
        var dbLocality = await _unitOfWork.LocalityRepository.FindByIdAsync(id);
        
        dbLocality.Update(localityDto);
        _unitOfWork.LocalityRepository.Update(dbLocality);
        await _unitOfWork.SaveChangesAsync();

        return dbLocality.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var dbIbge = await _unitOfWork.LocalityRepository.FindByIdAsync(id);

        _unitOfWork.LocalityRepository.Delete(dbIbge);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<LocalityDto?> GetByIdAsync(int id)
    {
        var dbIbge = await _unitOfWork.LocalityRepository.GetByIdAsync(id);
        return dbIbge?.ToDto();
    }

    public async Task<List<LocalityDto>> GetAllAsync(string? filter)
    {
        var dbLocalities = await _unitOfWork.LocalityRepository
            .GetAllAsync(locality =>
                filter == null ||
                (locality.IbgeCode.ToString().Contains(filter) ||
                 locality.City.Contains(filter) ||
                 locality.UF.Contains(filter)));

        return dbLocalities
            .Select(dbLocality => dbLocality.ToDto())
            .ToList();
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

        foreach (IRow row in sheet)
        {
            var cols = row.Cells;

            if (row.RowNum is 0) //First line (headers)        
            {
                continue;
            }

            if (cols.Count < 4) //Must have 4 columns to be valid locality
            {
                localityFailedToImportDtos.Add(new LocalityValidationResultDto()
                {
                    IsValid = false,
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
                IbgeCode = int.Parse(ibgeCode),
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

        var dbIbges = await _unitOfWork.LocalityRepository.GetAllAsync();
        _logger.LogInformation("Current total IBGE's in database: {CurrentTotalIbgesInDb}", dbIbges.Count);

        var dbIbgeCodes = dbIbges.Select(x => x.IbgeCode);
        var dtoIbgeCodes = localityDtos.Select(x => x.IbgeCode);
        var ibgeCodesToRemove = dbIbgeCodes.Intersect(dtoIbgeCodes).ToList();
        var ignoredCount = ibgeCodesToRemove.Count();

        localityDtos.RemoveAll(ibgeDto =>
            ibgeCodesToRemove.Any(code => ibgeDto.IbgeCode == code)); // Remove existents ibges in database
        _logger.LogInformation(
            "Total IBGE's ignored because already exists in database: {TotalIgnoredIbgesThatAlreadyExistsInDb}",
            ignoredCount);

        var ibges = localityDtos.Select(ibgeDto => new Locality(ibgeDto)).ToList();
        _logger.LogInformation("Total IBGE's will be created: {TotalIbgesThatWillBeCreated}", ibges.Count);

        var sw = new Stopwatch();
        sw.Start();
        await _unitOfWork.LocalityRepository.BulkInsertAsync(ibges);
        sw.Stop();

        _logger.LogInformation("Total localities failed to import: {TotalLocalityFailCount}",
            localityFailedToImportDtos.Count);
        _logger.LogInformation("Elapsed time to import {ImportedCount}: {ImportElapsedTime}", localityDtos.Count,
            sw.Elapsed);

        return new LocalityImportResult
        {
            Elapsed = sw.Elapsed,
            CreatedCount = ibges.Count,
            IgnoredCount = ignoredCount < 0 ? 0 : ignoredCount,
            FailedCount = localityFailedToImportDtos.Count,
            FailedLocalities = localityFailedToImportDtos
        };
    }
}