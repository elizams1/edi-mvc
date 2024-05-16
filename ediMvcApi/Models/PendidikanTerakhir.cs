namespace ediMvcApi.Models
{
    public class PendidikanTerakhir
    {
        public int id { get; set; }
        public string? biodata_id { get; set; }
        public string? jenjang_pendidikan { get; set; }
        public string? nama_institusi {  get; set; }
        public string? jurusan { get; set; }
        public string? tahun_lulus { get; set; }
        public decimal? ipk { get; set; }
        public DateTime create_on { get; set; }
        public int create_by { get; set; }
        public DateTime? update_on { get; set; }
        public int? update_by { get; set; }
        public DateTime? delete_on { get; set; }
        public int? delete_by { get; set; }
        public bool is_delete { get; set; }
    }
}
