using Dapper;
using Dapper.Transaction;
using ediMvcApi.Models;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Transactions;

namespace ediMvcApi.Repository.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        private readonly string connect;
        private Response res = new Response();

        public UserRepository(IConfiguration config)
        {
            _config = config;
            connect = _config.GetConnectionString("DefaultConnection");
        }

        public async Task<Response> CreateUser(User user)
        {
            using (var connection = new SqlConnection(connect))
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        var email = GetUserByEmail(user.email).Result.data;
                        if (email == null)
                        {
                            var create = "insert into t_user (email, password, akses_id, create_on, create_by) values (@email, @password, @akses_id, getdate(), 1 )";
                            var users = connection.Execute(create, new {email=user.email, password=user.password, akses_id = user.akses_id},  tran);
                            tran.Commit();

                            res.data = GetAllUsers().Result.data;
                            res.message = "User berhasil ditambahkan";
                            res.statusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.message = "Email sudah terdaftar sebelumnya";
                            res.data = email;
                            res.statusCode = HttpStatusCode.Ambiguous;
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        res.message = ex.Message;
                    }
                }

            }
            return res;
        }

        public async Task<Response> DeleteUser(int userId)
        {
            using var connection = new SqlConnection(connect);
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    var existUser = GetUserById(userId).Result.data;
                    if (existUser != null)
                    {
                        var delete = "update t_user set is_delete = 1, delete_on=getdate(), delete_by=1 where id = @Id";
                        var users = await connection.ExecuteAsync(delete, new {Id=userId}, tran);
                        tran.Commit();

                        res.data = GetAllUsers().Result.data;
                        res.message = "User berhasil dihapus";
                        res.statusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.message = "User tidak berhasil dihapus";
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

        public async Task<Response> GetAllUsers()
        {
            try
            {
                using var connection = new SqlConnection(connect);
                var users = await connection.QueryAsync<User>("select * from t_user where is_delete = 0");
                
                res.data = users;
                res.message = (users.ToList().Count > 0) ? $"{users.ToList().Count} User data berhasil diakses" : "User tidak memiliki data!";
                res.statusCode = (users.ToList().Count > 0) ? HttpStatusCode.OK : HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                res.message = ex.Message;
            }
            
            return res;
        }

        public async Task<Response> GetUserByEmail(string email)
        {
            try
            {
                using var connection = new SqlConnection(connect);
                var users = await connection.QueryFirstOrDefaultAsync<User>("select * from t_user where email=@Email and is_delete=0", new { Email = email });

                res.data = users;
                res.message = "User berhasil ditemukan!";
                res.statusCode = HttpStatusCode.OK;

                users = null;
            }
            catch (Exception ex)
            {
                res.message = ex.Message;
            }
            
            return res;
        }

        public async Task<Response> GetUserById(int userId)
        {
            try
            {
                if(userId > 0)
                {
                    using var connection = new SqlConnection(connect);
                    var users = await connection.QueryFirstAsync<User>("select * from t_user where id=@Id and is_delete=0", new { Id = userId });

                    if (users != null)
                    {
                        res.data = users;
                        res.message = $"User dengan id = {userId} berhasil diakses";
                        res.statusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.message = $"User dengan id = {userId} tidak ditemuka";
                        res.statusCode = HttpStatusCode.NoContent;
                    }
                    users = null;
                }
                else
                {
                    res.statusCode = HttpStatusCode.BadRequest;
                    res.message = $"User dengan id = {userId} tidak diketahui";
                }
            }
            catch (Exception ex) 
            {
                res.message = ex.Message;
            }
            return res;
        }

        public async Task<Response> LoginUser(string email, string password)
        {
            try
            {
                using var connection = new SqlConnection(connect);
                var users = await connection.QueryFirstAsync<User>("select * from t_user where email = @Email and password = @Password and is_delete=0", new { Email = email, Password = password });

                res.data = users;
                res.message = "User berhasil ditemukan!";
                res.statusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                res.message = ex.Message;
            }

            return res;
        }

        public async Task<Response> UpdateUser(User user)
        {
            using var connection = new SqlConnection(connect);
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    User existUser = (User)GetUserById(user.id).Result.data;
                    if (existUser != null)
                    {
                        var delete = "update t_user set biodata_id = @Biodata_id, email=@Email, password = @Password, akses_id = @Akses_id, update_on=getdate(), update_by=1 where id = @Id";
                        var users = await connection.ExecuteAsync(delete,
                            new { Id = user.id,
                                Biodata_id = user.biodata_id,
                                Email = user.email,
                                Password = user.password,
                                Akses_id = user.akses_id,
                            }, tran);
                        tran.Commit();

                        res.data = GetAllUsers().Result.data;
                        res.message = "User berhasil diperbaharui";
                        res.statusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.message = "User tidak berhasil diperbaharui";
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
