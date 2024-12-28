using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record JobTitleDTO(int ID, string Name);

    public class clsJobTitlesData
    {
        public static async Task<IEnumerable<JobTitleDTO>> GetAllAsync()
        {
            var jobTitle = new List<JobTitleDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_jobtitles_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                jobTitle.Add(new JobTitleDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("JobTitleID")),
                                        reader.GetString(reader.GetOrdinal("Title"))
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

            return jobTitle;
        }



        public static async Task<JobTitleDTO> GetByIdAsync(int ID)
        {
            JobTitleDTO jobTitle = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_jobtitles_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@JobTitleID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                jobTitle = new JobTitleDTO
                                             (
                                                reader.GetInt32(reader.GetOrdinal("JobTitleID")),
                                        reader.GetString(reader.GetOrdinal("Title"))
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

            return jobTitle;
        }
        public static async Task<JobTitleDTO> GetByJobTitleNameAsync(string title)
        {
            JobTitleDTO jobTitle = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_jobtitles_FindByName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@JobTitle", title);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                jobTitle = new JobTitleDTO
                                             (
                                                reader.GetInt32(reader.GetOrdinal("JobTitleID")),
                                                reader.GetString(reader.GetOrdinal("Title"))
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

            return jobTitle;
        }
    }
}
