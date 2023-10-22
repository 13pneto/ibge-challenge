﻿using challenge.ibge.infra.data.Dtos;
using Microsoft.AspNetCore.Http;

namespace challenge.ibge.infra.data.Services.Interfaces;

public interface ILocalityService
{
    Task<LocalityDto> CreateAsync(LocalityDto localityDto);
    Task<LocalityDto> UpdateAsync(int id, LocalityDto localityDto);
    Task DeleteAsync(int id);
    Task<LocalityDto?> GetByIdAsync(int id);
    Task<List<LocalityDto>> GetAllAsync(string? filter);
    Task<LocalityImportResult> ImportAsync(IFormFile file);
}