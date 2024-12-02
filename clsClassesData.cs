using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record classDTO(int ID, string Name, string Code, short Capacity, int SchoolID);
    public class clsClassesData
    {
        public static async Task<IEnumerable<classDTO>> GetAllAsync()
        {
            var classes = new List<classDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_classes_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                classes.Add(new classDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("ID")),
                                        reader.GetString(reader.GetOrdinal("Name")),
                                        reader.GetString(reader.GetOrdinal("Code")),
                                        reader.GetInt16(reader.GetOrdinal("Capacity")),
                                        reader.GetInt32(reader.GetOrdinal("SchoolID"))
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

            return classes;
        }

        public static async Task<classDTO> GetByIdAsync(int classID)
        {
            classDTO @class = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_classes_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", classID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                @class = new classDTO
                                             (
                                                 reader.GetInt32(reader.GetOrdinal("ID")),
                                        reader.GetString(reader.GetOrdinal("Name")),
                                        reader.GetString(reader.GetOrdinal("Code")),
                                        reader.GetInt16(reader.GetOrdinal("Capacity")),
                                        reader.GetInt32(reader.GetOrdinal("SchoolID"))
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

            return @class;
        }


        public static async Task<int> AddAsync(classDTO @class)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_classes_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Name", @class.Name);
                        command.Parameters.AddWithValue("@Code", @class.Code);
                        command.Parameters.AddWithValue("@Capacity", @class.Capacity);
                        command.Parameters.AddWithValue("@SchoolID", @class.SchoolID);

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

        public static async Task<bool> UpdateAsync(classDTO @class)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_classes_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", @class.ID);
                        command.Parameters.AddWithValue("@Name", @class.Name);
                        command.Parameters.AddWithValue("@Code", @class.Code);
                        command.Parameters.AddWithValue("@Capacity", @class.Capacity);
                        command.Parameters.AddWithValue("@SchoolID", @class.SchoolID);

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
                    using (var command = new SqlCommand("sp_classes_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", ID);

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

        public static async Task<bool> IsExistsAsync(int ID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {

                    using (var command = new SqlCommand("sp_classes_Exist", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", ID);

                        await connection.OpenAsync();
                        var result = await command.ExecuteScalarAsync();
                        return Convert.ToBoolean(result);
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
