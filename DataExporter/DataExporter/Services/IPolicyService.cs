using DataExporter.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DataExporter.Services
{
    public interface IPolicyService
    {
        Task<ReadPolicyDto?> CreatePolicyAsync(CreatePolicyDto createPolicyDto);
        Task<IList<ReadPolicyDto>> ReadPoliciesAsync();
        Task<ReadPolicyDto?> ReadPolicyAsync(string policyNumber);
        Task<IList<ExportDto>> Export(DateTime startDate, DateTime endDate);
        Task AddNote([FromBody] NoteDto noteDto);
    }
}
