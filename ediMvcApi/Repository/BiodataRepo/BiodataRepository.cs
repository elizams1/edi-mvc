using ediMvcApi.Models;
using System.Data.SqlClient;
using System.Net;
using System;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace ediMvcApi.Repository.BiodataRepo
{
    public class BiodataRepository : IBiodataRepository
    {
        private readonly IConfiguration _config;
        private readonly string connect;
        private Response res = new Response();

        public BiodataRepository(IConfiguration config)
        {
            _config = config;
            connect = _config.GetConnectionString("DefaultConnection");
        }

        public async Task<Response> CreateBiodata(Biodata biodata)
        {
            using var connection = new SqlConnection(connect);
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    var create = "insert into t_biodata (posisi, nama, no_ktp, tempat_lahir, tanggal_lahir, jenis_kelamin, agama, gol_darah, status, alamat_ktp, alamat_tinggal, email_bio, no_telp, no_telp_terdekat, skill, penempatan, eks_gaji, create_on, create_by) values (@Posisi, @Nama, @No_ktp, @Tempat_lahir, @Tanggal_lahir, @Jenis_kelamin, @Agama, @Gol_darah, @Status, @Alamat_ktp, @Alamat_tinggal, @Email_bio, @No_telp, @No_telp_terdekat, @Skill, @Penempatan, @Eks_gaji, getdate(), 1 ) ; SELECT CAST(SCOPE_IDENTITY() as int)";
                    int biosId = connection.ExecuteScalar<int>(create,
                        new
                        {
                            Posisi = biodata.posisi,
                            Nama = biodata.nama,
                            No_ktp = biodata.no_ktp,
                            Tempat_lahir = biodata.tempat_lahir,
                            Tanggal_lahir = biodata.tanggal_lahir,
                            Jenis_kelamin = biodata.jenis_kelamin,
                            Agama = biodata.agama,
                            Gol_darah = biodata.gol_darah,
                            Status = biodata.status,
                            Alamat_ktp = biodata.alamat_ktp,
                            Alamat_tinggal = biodata.alamat_tinggal,
                            Email_bio = biodata.email_bio,
                            No_telp = biodata.no_telp,
                            No_telp_terdekat = biodata.no_telp_terdekat,
                            Skill = biodata.skill,
                            Penempatan = biodata.penempatan,
                            Eks_gaji=biodata.eks_gaji
                        }, tran);

                    if(biodata.PendidikanTerakhirs != null && biodata.PendidikanTerakhirs.Count() > 0)
                    {
                        var createPendidikan = "insert into t_pendidikan_terakhir (biodata_id, jenjang_pendidikan, nama_institusi, jurusan, tahun_lulus, ipk, create_on, create_by) values (@Biodata_id, @Jenjang_pendidikan, @Nama_institusi, @Jurusan, @Tahun_lulus, @Ipk, getdate(), 1)";

                        foreach(PendidikanTerakhir item in biodata.PendidikanTerakhirs)
                        {
                            var createPendidikanAdd = await connection.ExecuteAsync(createPendidikan,
                                new
                                {
                                    Biodata_id = biosId,
                                    Jenjang_pendidikan = item.jenjang_pendidikan,
                                    Nama_institusi = item.nama_institusi,
                                    Jurusan = item.jurusan,
                                    Tahun_lulus = item.tahun_lulus,
                                    Ipk = item.ipk
                                },tran);
                        }
                    }

                    if(biodata.RiwayatPekerjaans !=null && biodata.RiwayatPekerjaans.Count() > 0)
                    {
                        var createRiwayatPekerjaan = "insert into t_riwayat_pekerjaan (biodata_id, nama_perusahaan, posisi, gaji, tahun_pekerjaan, create_on, create_by) values (@Biodata_id, @Nama_perusahaan, @Posisi, @Gaji, @Tahun_pekerjaan, getdate(), 1";

                        foreach(RiwayatPekerjaan item in biodata.RiwayatPekerjaans)
                        {
                            var createRiwayatPekerjaanAdd = await connection.ExecuteAsync(createRiwayatPekerjaan,
                                new
                                {
                                    Biodata_id = biosId,
                                    Nama_perusahaan = item.nama_perusahaan,
                                    Posisi = item.posisi,
                                    Gaji = item.gaji,
                                    Tahun_pekerjaan = item.tahun_pekerjaan
                                },tran);
                        }
                    }

                    if(biodata.RiwayatPelatihans!=null && biodata.RiwayatPelatihans.Count() > 0)
                    {
                        var createRiwayatPelatihan = "insert into t_riwayat_pelatihan (biodata_id, nama_kursus, sertifikat, tahun_kursus, create_on, create_by) values (@Biodata_id, @Nama_kursus, @Sertifikat, @Tahun_kursus, getdate(), 1";

                        foreach (RiwayatPelatihan item in biodata.RiwayatPelatihans)
                        {
                            var createRiwayatPelatihanAdd = await connection.ExecuteAsync(createRiwayatPelatihan,
                                new
                                {
                                    Biodata_id = biosId,
                                    Nama_kursus = item.nama_kursus,
                                    Sertifikat = item.sertifikat,
                                    Tahun_kursus = item.tahun_kursus
                                }, tran);
                        }
                    }

                    tran.Commit();

                    res.data = biodata;
                    res.message = "Biodata berhasil ditambahkan";
                    res.statusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    res.message = ex.Message;
                }
            }
            return res;
        }

        public async Task<Response> DeleteBiodata(int bioId)
        {
            using var connection = new SqlConnection(connect);
            connection.Open();

            using(var tran = connection.BeginTransaction())
            {
                try
                {
                    var existBio = GetBiodataById(bioId).Result.data;
                    if (existBio != null)
                    {
                        var delete = "update t_biodata set is_delete = 1, delete_on=getdate(), delete_by=1 where id = @Id";
                        var bios = await connection.ExecuteAsync(delete, new { Id = bioId }, tran);
                        tran.Commit();

                        res.data = existBio;
                        res.message = "Biodata berhasil dihapus";
                        res.statusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.message = "Biodata tidak berhasil dihapus";
                        res.statusCode = HttpStatusCode.Ambiguous;
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    res.message = ex.Message;
                }
            }
            return res;
        }

        public async Task<Response> GetBiodataByString(string? filter)
        {
            try
            {
                using var connection = new SqlConnection(connect);
                string sql;
                string fil = '%' + filter + '%';
                if (filter == "")
                {
                    sql = "select * from T_Biodata ";
                }
                else
                {
                    sql = "select * from T_Biodata where nama like @Filter or posisi like @Filter";
                }

                var biodata = await connection.QueryAsync<Biodata>(sql, new {Filter =fil});

                if (biodata.ToList().Count > 0)
                {
                    res.data = biodata;
                    res.message = "Biodata berhasil diakses";
                    res.statusCode = HttpStatusCode.OK;
                }
                else
                {
                    res.message = "Biodata tidak berhasil diakses";
                    res.statusCode = HttpStatusCode.NoContent;
                }
                biodata = null;
            }
            catch (Exception ex)
            {
                res.message = ex.Message;
            }
            return res;
        }

        public async Task<Response> GetBiodataById(int bioId)
        {
            try
            {
                if (bioId > 0)
                {
                    using var connection = new SqlConnection(connect);
                    //var bios = await connection.QueryFirstAsync<Biodata>("select * from t_biodata where id=@Id and is_delete=0", new { Id = bioId });

                    string sql = @"
                        select
                            tb.*,
                            tpt.id as TptId,
                            tpt.*,
                            trp.id as TrpId,
                            trp.*,
                            trl.id as TrlId,
                            trl.*
                        from t_biodata tb
                        left join t_pendidikan_terakhir tpt on tb.id = tpt.biodata_id
                        left join t_riwayat_pekerjaan trp on tb.id = trp.biodata_id
                        left join t_riwayat_pelatihan trl on tb.id = trl.biodata_id
                        where tb.id = @BioId and tb.is_delete=0;
                    ";

                    var biosDictionary = new Dictionary<int, Biodata>();

                    var thbios = await connection.QueryAsync<Biodata, PendidikanTerakhir, RiwayatPekerjaan,RiwayatPelatihan, Biodata>(
                       sql,
                       (biodata, pendidikanTerakhir, riwayatPekerjaan, riwayatPelatihan) =>
                    {
                        Console.WriteLine(pendidikanTerakhir);
                        if (!biosDictionary.TryGetValue(biodata.id, out Biodata existingBio))
                        {
                             existingBio = biodata;
                            existingBio.PendidikanTerakhirs = new List<PendidikanTerakhir>();
                            existingBio.RiwayatPekerjaans = new List<RiwayatPekerjaan>();
                            existingBio.RiwayatPelatihans = new List<RiwayatPelatihan>();
                            biosDictionary.Add(existingBio.id, existingBio);
                        }

                        if(pendidikanTerakhir != null)
                        {
                            existingBio.PendidikanTerakhirs.Add(pendidikanTerakhir);
                        }
                        if(riwayatPekerjaan != null)
                        {
                            existingBio.RiwayatPekerjaans.Add(riwayatPekerjaan);
                        }
                        if (riwayatPelatihan != null)
                        {
                            existingBio.RiwayatPelatihans.Add(riwayatPelatihan);
                        }
                        

                        return existingBio;
                    }, new { BioId = bioId}, splitOn: "TptId, TrpId, TrlId");


                    if (biosDictionary.Count != 0)
                    {
                        res.data = biosDictionary.Values;
                        res.message = $"User dengan id = {bioId} berhasil diakses";
                        res.statusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.message = $"User dengan id = {bioId} tidak ditemuka";
                        res.statusCode = HttpStatusCode.NoContent;
                    }
                    thbios = null;
                }
                else
                {
                    res.statusCode = HttpStatusCode.BadRequest;
                    res.message = $"User dengan id = {bioId} tidak diketahui";
                }
            }
            catch (Exception ex)
            {
                res.message = ex.Message;
            }
            return res;
        }

        public async Task<Response> UpdateBiodata(Biodata biodata)
        {
            using var connection = new SqlConnection(connect);
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    var existBio = GetBiodataById(biodata.id).Result.data;
                    if(existBio != null)
                    {
                        var update = @"update t_biodata 
                        set posisi = @Posisi,nama = @Nama, no_ktp = @No_ktp, 
                            tempat_lahir = @Tempat_lahir, tanggal_lahir = @Tanggal_lahir,
                            jenis_kelamin =@Jenis_kelamin, agama = @Agama, gol_darah = @Gol_darah,
                            status = @Status, alamat_ktp = @Alamat_ktp, alamat_tinggal = @Alamat_tinggal, email_bio = @Email_bio, no_telp=@No.telp, 
                            no_telp_terdekat=@No_telp_terdekat, skill=@Skill, penempatan=@Penempatan, eks_gaji=@Eks_gaji, update_on=getdate(), update_by=1 where id=@Id";
                        var biosId = await connection.ExecuteAsync(update,
                            new
                            {
                                Id = biodata.id,
                                Posisi = biodata.posisi,
                                Nama = biodata.nama,
                                No_ktp = biodata.no_ktp,
                                Tempat_lahir = biodata.tempat_lahir,
                                Tanggal_lahir = biodata.tanggal_lahir,
                                Jenis_kelamin = biodata.jenis_kelamin,
                                Agama = biodata.agama,
                                Gol_darah = biodata.gol_darah,
                                Status = biodata.status,
                                Alamat_ktp = biodata.alamat_ktp,
                                Alamat_tinggal = biodata.alamat_tinggal,
                                Email_bio = biodata.email_bio,
                                No_telp = biodata.no_telp,
                                No_telp_terdekat = biodata.no_telp_terdekat,
                                Skill = biodata.skill,
                                Penempatan = biodata.penempatan,
                                Eks_gaji = biodata.eks_gaji
                            }, tran);

                        if (biodata.PendidikanTerakhirs != null && biodata.PendidikanTerakhirs.Count() > 0)
                        {
                            var updatePendidikan = @"
                            update t_pendidikan_terakhir set biodata_id=@Biodata_id, jenjang_pendidikan=@Jenjang_pendidikan, nama_institusi=@Nama_institusi, jurusan=@Jurusan, tahun_lulus=@Tahun_lulus, ipk=@Ipk, update_on=getdate(), update_by=1)";

                            foreach (PendidikanTerakhir item in biodata.PendidikanTerakhirs)
                            {
                                var updatePendidikanAdd = await connection.ExecuteAsync(updatePendidikan,
                                    new
                                    {
                                        Biodata_id = biodata.id,
                                        Jenjang_pendidikan = item.jenjang_pendidikan,
                                        Nama_institusi = item.nama_institusi,
                                        Jurusan = item.jurusan,
                                        Tahun_lulus = item.tahun_lulus,
                                        Ipk = item.ipk
                                    }, tran);
                            }
                        }

                        if (biodata.RiwayatPekerjaans != null && biodata.RiwayatPekerjaans.Count() > 0)
                        {
                            var updateRiwayatPekerjaan = "update t_riwayat_pekerjaan (biodata_id, nama_perusahaan, posisi, gaji, tahun_pekerjaan, update_on, update_by) values (@Biodata_id, @Nama_perusahaan, @Posisi, @Gaji, @Tahun_pekerjaan, getdate(), 1";

                            foreach (RiwayatPekerjaan item in biodata.RiwayatPekerjaans)
                            {
                                var updateRiwayatPekerjaanAdd = await connection.ExecuteAsync(updateRiwayatPekerjaan,
                                    new
                                    {
                                        Biodata_id = biodata.id,
                                        Nama_perusahaan = item.nama_perusahaan,
                                        Posisi = item.posisi,
                                        Gaji = item.gaji,
                                        Tahun_pekerjaan = item.tahun_pekerjaan
                                    }, tran);
                            }
                        }

                        if (biodata.RiwayatPelatihans != null && biodata.RiwayatPelatihans.Count() > 0)
                        {
                            var updateRiwayatPelatihan = "update t_riwayat_pelatihan (biodata_id, nama_kursus, sertifikat, tahun_kursus, update_on, update_by) values (@Biodata_id, @Nama_kursus, @Sertifikat, @Tahun_kursus, getdate(), 1";

                            foreach (RiwayatPelatihan item in biodata.RiwayatPelatihans)
                            {
                                var createRiwayatPelatihanAdd = await connection.ExecuteAsync(updateRiwayatPelatihan,
                                    new
                                    {
                                        Biodata_id = biodata.id,
                                        Nama_kursus = item.nama_kursus,
                                        Sertifikat = item.sertifikat,
                                        Tahun_kursus = item.tahun_kursus
                                    }, tran);
                            }
                        }

                        tran.Commit();

                        res.data = biodata;
                        res.message = "Biodata berhasil ditambahkan";
                        res.statusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.message = "Biodata tidak berhasil diperbaharui";
                        res.statusCode = HttpStatusCode.Ambiguous;
                    }
                    
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    res.message = ex.Message;
                }
            }
            return res;
        }
    }
}
