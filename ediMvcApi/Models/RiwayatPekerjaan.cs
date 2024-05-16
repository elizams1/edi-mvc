namespace ediMvcApi.Models
{
    public class RiwayatPekerjaan
    {
        public int id { get; set; }
        public string? biodata_id { get; set; }
        public string? nama_perusahaan {  get; set; }
        public string? posisi {  get; set; }
        public string? gaji {  get; set; }
        public string? tahun_pekerjaan {  get; set; }
        public DateTime create_on { get; set; }
        public int create_by { get; set; }
        public DateTime? update_on { get; set; }
        public int? update_by { get; set; }
        public DateTime? delete_on { get; set; }
        public int? delete_by { get; set; }
        public bool is_delete { get; set; }
    }
}
