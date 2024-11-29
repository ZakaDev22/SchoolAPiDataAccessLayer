
using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record userRegisterDTO(int RegisterID, int UserID, DateTime LoginTime, DateTime? LogoutTime, string IPAddress, int? SessionDuration);
    public class clsUserRegisterData
    {
        public static async Task<IEnumerable<userRegisterDTO>> GetAllAsync()
        {
            var Registers = new List<userRegisterDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_usersRegister_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Registers.Add(new userRegisterDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("userRegisterID")),
                                        reader.GetInt32(reader.GetOrdinal("UserID")),
                                        reader.GetDateTime(reader.GetOrdinal("LoginTime")),
                                        reader["LogoutTime"] != DBNull.Value ? reader.GetDateTime(reader.GetOrdinal("LogoutTime")) : null,
                                        reader.GetString(reader.GetOrdinal("IPAddress")),
                                        reader["SessionDuration"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("SessionDuration")) : null
                                    ));
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return Registers;
        }

        public static async Task<userRegisterDTO> GetByIdAsync(int ID)
        {
            userRegisterDTO register = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_usersRegister_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RegisterID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                register = new userRegisterDTO
                                             (
                                       reader.GetInt32(reader.GetOrdinal("userRegisterID")),
                                        reader.GetInt32(reader.GetOrdinal("UserID")),
                                        reader.GetDateTime(reader.GetOrdinal("LoginTime")),
                                        reader["LogoutTime"] != DBNull.Value ? reader.GetDateTime(reader.GetOrdinal("LogoutTime")) : null,
                                        reader.GetString(reader.GetOrdinal("IPAddress")),
                                        reader["SessionDuration"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("SessionDuration")) : null
                                              );
                            };

                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return register;
        }


        public static async Task<int> AddAsync(userRegisterDTO register)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_usersRegister_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@UserID", register.UserID);
                        command.Parameters.AddWithValue("@IPAddress", register.IPAddress);

                        var outputIdParam = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outputIdParam);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        return (int)outputIdParam.Value;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method WIll Only Update The Login Register Record With Logout Time Inside The Database 
        /// No Need To Send Time As a variable here
        /// </summary>
        /// <param name="registerID"></param>
        /// <returns>True or False</returns>
        public static async Task<bool> UpdateAsync(int registerID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_usersRegister_UpdateLogout", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RegisterID", registerID);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<bool> DeleteAsync(int ID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_usersRegister_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RegisterID", ID);

                        await connection.OpenAsync();
                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
