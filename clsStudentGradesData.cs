using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record sGradeDTO(int ID, string Name, int SchoolID);

    public class clsStudentGradesData
    {
        public static async Task<IEnumerable<sGradeDTO>> GetAllAsync()
        {
            var sGrades = new List<sGradeDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_studentgrades_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                sGrades.Add(new sGradeDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("GradeID")),
                                        reader.GetString(reader.GetOrdinal("GradeName")),
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

            return sGrades;
        }

        public static async Task<sGradeDTO> GetByIdAsync(int ID)
        {
            sGradeDTO sGrade = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_studentgrades_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@GradeID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                sGrade = new sGradeDTO
                                             (
                                                reader.GetInt32(reader.GetOrdinal("GradeID")),
                                        reader.GetString(reader.GetOrdinal("GradeName")),
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

            return sGrade;
        }

        public static async Task<bool> DeleteAsync(int ID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_studentgrades_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@GradeID", ID);

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

                    using (var command = new SqlCommand("sp_studentgrades_Exist", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@GradeID", ID);

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
