using DataExporter.Dtos;
using DataExporter.Model;
using Microsoft.EntityFrameworkCore;


namespace DataExporter.Services
{
    public class PolicyService
    {
        private ExporterDbContext _dbContext;

        public PolicyService(ExporterDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
        }

        /// <summary>
        /// Creates a new policy from the DTO.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>Returns a ReadPolicyDto representing the new policy, if succeded. Returns null, otherwise.</returns>
        public async Task<ReadPolicyDto?> CreatePolicyAsync(CreatePolicyDto createPolicyDto)
        {
            if (createPolicyDto != null)
            {
                var policy = new Policy
                {
                    PolicyNumber = createPolicyDto.PolicyNumber,
                    Premium = createPolicyDto.Premium,
                    StartDate = createPolicyDto.StartDate
                };

                _dbContext.Policies.Add(policy);
                await _dbContext.SaveChangesAsync();
            }

            return await Task.FromResult(new ReadPolicyDto());
        }

        /// <summary>
        /// Retrives all policies.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a list of ReadPoliciesDto.</returns>
        public async Task<IList<ReadPolicyDto>> ReadPoliciesAsync()
        {
            var policies = _dbContext.Policies;

            if (policies.Any())
            {
                var readPoliciesResult = new List<ReadPolicyDto>();
                readPoliciesResult.AddRange(policies.Select(x => new ReadPolicyDto 
                { 
                    Id = x.Id, 
                    PolicyNumber = x.PolicyNumber, 
                    Premium = x.Premium, 
                    StartDate = x.StartDate 
                }));

                return await Task.FromResult(readPoliciesResult);
            }
            return await Task.FromResult(new List<ReadPolicyDto>());
        }

        /// <summary>
        /// Retrieves a policy by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a ReadPolicyDto.</returns>
        public async Task<ReadPolicyDto?> ReadPolicyAsync(int id)
        {
            var policy = await _dbContext.Policies.SingleAsync(x => x.Id == id);
            if (policy == null)
            {
                return null;
            }

            var policyDto = new ReadPolicyDto()
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Premium = policy.Premium,
                StartDate = policy.StartDate
            };

            return policyDto;
        }

        /// <summary>
        /// Exports all policies with StartDate between params StartDate and EndDate
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Returns a list of ExportDtos</returns>
        public async Task<IList<ExportDto>> Export(DateTime startDate, DateTime endDate)
        {
            var exportPolicies = _dbContext.Policies.Where(x => x.StartDate >= startDate && x.StartDate <= endDate);

            if (exportPolicies.Any()) 
            {
                var exportDtos = new List<ExportDto>();
                exportDtos.AddRange(exportPolicies.Select(x => new ExportDto() 
                { 
                    PolicyNumber = x.PolicyNumber, 
                    Premium = x.Premium, 
                    StartDate = x.StartDate,
                    Notes = (IList<string>)x.Notes
                }));

                return await Task.FromResult(exportDtos);
            }

            return await Task.FromResult(new List<ExportDto>());
        }
    }
}
