using AutoMapper;
using DataExporter.Dtos;
using DataExporter.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DataExporter.Services
{
    public class PolicyService : IPolicyService
    {
        private ExporterDbContext _dbContext;
        private IMapper _mapper;

        public PolicyService(ExporterDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
            _mapper = mapper;
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
                var policy = _mapper.Map<Policy>(createPolicyDto);

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
                var readPoliciesResult = _mapper.Map<List<ReadPolicyDto>>(policies.ToList());
                return await Task.FromResult(readPoliciesResult);
            }

            return await Task.FromResult(new List<ReadPolicyDto>());
        }

        /// <summary>
        /// Retrieves a policy by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a ReadPolicyDto.</returns>
        public async Task<ReadPolicyDto?> ReadPolicyAsync(string policyNumber)
        {
            var policy = await _dbContext.Policies.Include(x => x.Notes).SingleAsync(x => x.PolicyNumber == policyNumber);
            if (policy == null)
            {
                return null;
            }

            var policyDto = _mapper.Map<ReadPolicyDto>(policy);

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
            if (startDate > endDate)
            {
                throw new Exception("start date must not be bigger than end date");
            }

            var exportPolicies = _dbContext.Policies.Include(x => x.Notes).Where(x => x.StartDate >= startDate && x.StartDate <= endDate);

            if (exportPolicies.Any()) 
            {
                var exportDtos = _mapper.Map<List<ExportDto>>(exportPolicies.ToList());
                return exportDtos;
            }

            return await Task.FromResult(new List<ExportDto>());
        }

        /// <summary>
        /// Adds a new note for an existing policy
        /// </summary>
        /// <param name="noteDto"></param>
        public async Task AddNote(NoteDto noteDto)
        {
            var policy = await _dbContext.Policies.SingleAsync(x => x.Id == noteDto.PolicyId);

            if (policy != null)
            {
                var note = _mapper.Map<Note>(noteDto);
                _dbContext.Notes.Add(note);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
