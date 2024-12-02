
using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record UserDTO(int ID, int PersonID, string UserName, int Permissions, int? AddedByUserID);
    public record FullUserDTO(int ID, int PersonID, string UserName, string Password, int Permissions, int? AddedByUserID);

    public class clsUsersData
    {
        public static async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = new List<UserDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_users_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                users.Add(new UserDTO(
                                    reader.GetInt32(reader.GetOrdinal("ID")),
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("UserName")),
                                     reader.GetInt32(reader.GetOrdinal("Permissions")),
                                    reader.IsDBNull(reader.GetOrdinal("AddedByUserID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AddedByUserID"))
                                ));
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return users;
        }

        public static async Task<FullUserDTO> GetByIdAsync(int userId)
        {
            FullUserDTO user = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_users_GetByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", userId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                user = new FullUserDTO(
                                    reader.GetInt32(reader.GetOrdinal("ID")),
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("UserName")),
                                     reader.GetString(reader.GetOrdinal("PasswordHash")),
                                     reader.GetInt32(reader.GetOrdinal("Permissions")),
                                    reader.IsDBNull(reader.GetOrdinal("AddedByUserID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AddedByUserID"))
                                );
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return user;
        }

        public static async Task<int> AddAsync(FullUserDTO user)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_users_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", user.PersonID);
                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@PasswordHash", user.Password);
                        command.Parameters.AddWithValue("@Permissions", user.Permissions);
                        command.Parameters.AddWithValue("@AddedByUserID", user.AddedByUserID ?? (object)DBNull.Value);

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
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<bool> UpdateAsync(FullUserDTO user)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_users_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", user.PersonID);
                        command.Parameters.AddWithValue("@ID", user.ID);
                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@PasswordHash", user.Password);
                        command.Parameters.AddWithValue("@Permissions", user.Permissions);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<bool> DeleteAsync(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_users_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", userId);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<bool> ExistsByID(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_users_ExistsByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", userId);

                        return await command.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<FullUserDTO> FindByUserNameAndPasswordAsync(string userName, string passwordHash)
        {
            FullUserDTO fullUserDTO = null;
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_users_FindByUserNameAndPasswordHash", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                fullUserDTO = new FullUserDTO(
                                    reader.GetInt32(reader.GetOrdinal("ID")),
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("UserName")),
                                     reader.GetString(reader.GetOrdinal("PasswordHash")),
                                     reader.GetInt32(reader.GetOrdinal("Permissions")),
                                    reader.IsDBNull(reader.GetOrdinal("AddedByUserID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AddedByUserID"))
                                );
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return fullUserDTO;
        }
    }
}
