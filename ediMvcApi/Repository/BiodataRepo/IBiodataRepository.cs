using ediMvcApi.Models;

namespace ediMvcApi.Repository.BiodataRepo
{
    public interface IBiodataRepository
    {
        //Task<Response> GetAllBiodata();
        Task<Response> GetBiodataById(int bioId);
        Task<Response> GetBiodataByString(string? filter);
        Task<Response> CreateBiodata(Biodata biodata);
        Task<Response> UpdateBiodata(Biodata biodata);
        Task<Response> DeleteBiodata(int bioId);
    }
}
