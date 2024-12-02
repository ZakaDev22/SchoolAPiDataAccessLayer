
using Microsoft.Data.SqlClient;
using System.Data;
namespace SchoolAPiDataAccessLayer
{
    public record sgLogDTO(int ID, int StudentID, int SubjectID, decimal Grade, DateTime LogDate, string Comments);

    public class clsStudentGradesLogData
    {
        public static async Task<IEnumerable<sgLogDTO>> GetAllAsync()
        {
            var sgLogs = new List<sgLogDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_studentsGradesLog_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                sgLogs.Add(new sgLogDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("ID")),
                                        reader.GetInt32(reader.GetOrdinal("StudentID")),
                                        reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                        reader.GetDecimal(reader.GetOrdinal("Grade")),
                                        reader.GetDateTime(reader.GetOrdinal("LogDate")),
                                        reader.GetString(reader.GetOrdinal("Comments"))

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

            return sgLogs;
        }

        public static async Task<sgLogDTO> GetByIdAsync(int ID)
        {
            sgLogDTO sglog = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_studentsGradesLog_Find", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                sglog = new sgLogDTO
                                             (
                                                reader.GetInt32(reader.GetOrdinal("ID")),
                                        reader.GetInt32(reader.GetOrdinal("StudentID")),
                                        reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                        reader.GetDecimal(reader.GetOrdinal("Grade")),
                                        reader.GetDateTime(reader.GetOrdinal("LogDate")),
                                        reader.GetString(reader.GetOrdinal("Comments"))

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

            return sglog;
        }

        public static async Task<int> AddAsync(sgLogDTO sglog)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_studentsGradesLog_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;


                        command.Parameters.AddWithValue("@StudentID", sglog.StudentID);
                        command.Parameters.AddWithValue("@SubjectID", sglog.SubjectID);
                        command.Parameters.AddWithValue("@Grade", sglog.Grade);
                        command.Parameters.AddWithValue("@Comments", sglog.Comments);


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

        public static async Task<bool> UpdateAsync(sgLogDTO sglog)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_studentsGradesLog_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", sglog.ID);
                        command.Parameters.AddWithValue("@StudentID", sglog.StudentID);
                        command.Parameters.AddWithValue("@SubjectID", sglog.SubjectID);
                        command.Parameters.AddWithValue("@Grade", sglog.Grade);
                        command.Parameters.AddWithValue("@Comments", sglog.Comments);

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
                    using (var command = new SqlCommand("sp_studentsGradesLog_Delete", connection))
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

                    using (var command = new SqlCommand("sp_studentsGradesLog_Exist", connection))
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
