using ediMvcApi.Models;
using ediMvcApi.Repository.BiodataRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ediMvcApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiodataController : ControllerBase
    {
        private readonly IBiodataRepository _biodataRepository;
        private Response res = new Response();

        public BiodataController(IBiodataRepository biodataRepository)
        {
            _biodataRepository = biodataRepository;
        }

        [HttpPost("[action]")]
        public async Task<Response> CreateBiodata(Biodata biodata)
        {
            res = await _biodataRepository.CreateBiodata(biodata);
            return res;
        }

        [HttpGet("[action]")]
        public async Task<Response> GetBiodataById(int bioId)
        {
            res = await _biodataRepository.GetBiodataById(bioId);
            return res;
        }

        [HttpGet("[action]")]
        public async Task<Response> GetAllBiodata() { 
            res = await _biodataRepository.GetBiodataByString("");
            return res;
        }

        [HttpGet("[action]")]
        public async Task<Response> GetBiodataByString(string filter) => await _biodataRepository.GetBiodataByString(filter);

        [HttpDelete("[action]")]
        public async Task<Response> DeleteBiodata(int bioId)
        {
            res = await _biodataRepository.DeleteBiodata(bioId);
            return res;
        }

        [HttpPut("[action]")]
        public async Task<Response> UpdateBiodata(Biodata biodata)
        {
            res = await _biodataRepository.UpdateBiodata(biodata);
            return res;
        }
    }
}
