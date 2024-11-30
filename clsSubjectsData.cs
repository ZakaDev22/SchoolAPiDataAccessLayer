﻿
using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record subjectDTO(int SubjectID, string SubjectName, string SubjectCode, int SchoolID);

    public class clsSubjectsData
    {

        public static async Task<IEnumerable<subjectDTO>> GetAllAsync()
        {
            var subjects = new List<subjectDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_subjects_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                subjects.Add(new subjectDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                        reader.GetString(reader.GetOrdinal("SubjectName")),
                                        reader.GetString(reader.GetOrdinal("SubjectCode")),
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

            return subjects;
        }

        public static async Task<subjectDTO> GetByIdAsync(int subjectID)
        {
            subjectDTO subject = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_subjects_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SubjectID", subjectID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                subject = new subjectDTO
                                             (
                                                  reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                        reader.GetString(reader.GetOrdinal("SubjectName")),
                                        reader.GetString(reader.GetOrdinal("SubjectCode")),
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

            return subject;
        }

        public static async Task<subjectDTO> GetBySubjectCodeAsync(string subjectCode)
        {
            subjectDTO subject = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_subjects_FindBySubjectCode", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SubjectCode", subjectCode);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                subject = new subjectDTO
                                       (reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                           reader.GetString(reader.GetOrdinal("SubjectName")),
                                           reader.GetString(reader.GetOrdinal("SubjectCode")),
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

            return subject;
        }
        public static async Task<int> AddAsync(subjectDTO subject)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_subjects_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SubjectName", subject.SubjectName);
                        command.Parameters.AddWithValue("@SubjectCode", subject.SubjectCode);
                        command.Parameters.AddWithValue("@SchoolID", subject.SchoolID);


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

        public static async Task<bool> UpdateAsync(subjectDTO subject)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_subjects_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SubjectID", subject.SubjectID);
                        command.Parameters.AddWithValue("@SubjectName", subject.SubjectName);
                        command.Parameters.AddWithValue("@SubjectCode", subject.SubjectCode);
                        command.Parameters.AddWithValue("@SchoolID", subject.SchoolID);

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

        public static async Task<bool> DeleteAsync(int subjectID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_subjects_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SubjectID", subjectID);

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

                    using (var command = new SqlCommand("sp_subjects_Exist", connection))
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