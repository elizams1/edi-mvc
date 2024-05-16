namespace ediMvcApi.Models
{
    public class Biodata
    {
        public int id { get; set; }
        public string? posisi { get; set; }
        public string? nama { get; set; }
        public string? no_ktp { get; set; }
        public string? tempat_lahir { get; set; }
        public DateTime? tanggal_lahir { get; set; }
        public string? jenis_kelamin { get; set; }
        public string? agama { get; set; }
        public string? gol_darah { get; set; }
        public string? status { get; set; }
        public string? alamat_ktp { get; set; }
        public string? alamat_tinggal { get; set; }
        public string? email_bio { get; set; }
        public string? no_telp { get; set; }
        public string? no_telp_terdekat { get; set; }
        public string? skill { get; set; }
        public string? penempatan { get; set; }
        public string? eks_gaji { get; set; }
        public DateTime create_on { get; set; }
        public int create_by { get; set; }
        public DateTime? update_on { get; set; }
        public int? update_by { get; set; }
        public DateTime? delete_on { get; set; }
        public int? delete_by { get; set; }
        public bool is_delete { get; set; }


        public List<PendidikanTerakhir> PendidikanTerakhirs { get; set; }
        public List<RiwayatPekerjaan> RiwayatPekerjaans { get; set; }
        public List<RiwayatPelatihan> RiwayatPelatihans { get; set; }
    }
}
