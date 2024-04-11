using DataExporter.Dtos;
using DataExporter.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataExporter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoliciesController : ControllerBase
    {
        private PolicyService _policyService;

        public PoliciesController(PolicyService policyService) 
        { 
            _policyService = policyService;
        }

        [HttpPost]
        public async Task<IActionResult> PostPolicies([FromBody]CreatePolicyDto createPolicyDto)
        {         
            var action = await _policyService.CreatePolicyAsync(createPolicyDto);
            return new OkObjectResult(action);
        }


        [HttpGet]
        public async Task<IActionResult> GetPolicies()
        {
            var policies = await _policyService.ReadPoliciesAsync();
            return new OkObjectResult(policies);
        }

        [HttpGet("{policyId}")]
        public async Task<IActionResult> GetPolicy(int id)
        {
            return Ok(_policyService.ReadPolicyAsync(id));
        }


        [HttpPost("export")]
        public async Task<IActionResult> ExportData([FromQuery]DateTime startDate, [FromQuery] DateTime endDate)
        {
            var export = await _policyService.Export(startDate, endDate);
            return new OkObjectResult(export);
        }
    }
}
