namespace ediMvcApi.Models
{
    public class User
    {
        public int id { get; set; }
        public int? biodata_id { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public int? akses_id { get; set; }
        public DateTime create_on { get; set; }
        public int create_by { get; set; }
        public DateTime? update_on { get; set;}
        public int? update_by { get; set; }
        public DateTime? delete_on { get; set;}
        public int? delete_by { get; set; }
        public bool is_delete { get; set; }
    }
}
