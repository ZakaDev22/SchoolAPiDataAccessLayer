using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record studentDTO(int StudentID, int PersonID, int StudentGradeID, int SchoolID, DateTime EnrollmentDate, bool IsActive);

    public class clsStudentsData
    {

        public static async Task<IEnumerable<studentDTO>> GetAllAsync()
        {
            var Students = new List<studentDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_students_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Students.Add(new studentDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("StudentID")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetInt32(reader.GetOrdinal("StudentGradeID")),
                                        reader.GetInt32(reader.GetOrdinal("SchoolID")),
                                        reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                                        reader.GetBoolean(reader.GetOrdinal("IsActive"))
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

            return Students;
        }

        public static async Task<studentDTO> GetByIdAsync(int StudentID)
        {
            studentDTO student = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_students_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentID", StudentID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                student = new studentDTO
                                             (
                                                 reader.GetInt32(reader.GetOrdinal("StudentID")),
                                                 reader.GetInt32(reader.GetOrdinal("PersonID")),
                                                 reader.GetInt32(reader.GetOrdinal("StudentGradeID")),
                                                 reader.GetInt32(reader.GetOrdinal("SchoolID")),
                                                 reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                                                 reader.GetBoolean(reader.GetOrdinal("IsActive"))
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

            return student;
        }

        public static async Task<studentDTO> GetByPersonIDAsync(int PersonID)
        {
            studentDTO student = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_students_FindByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                student = new studentDTO
                                             (
                                                 reader.GetInt32(reader.GetOrdinal("StudentID")),
                                                 reader.GetInt32(reader.GetOrdinal("PersonID")),
                                                 reader.GetInt32(reader.GetOrdinal("StudentGradeID")),
                                                 reader.GetInt32(reader.GetOrdinal("SchoolID")),
                                                 reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                                                 reader.GetBoolean(reader.GetOrdinal("IsActive"))
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

            return student;
        }


        public static async Task<int> AddAsync(studentDTO student)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_students_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", student.PersonID);
                        command.Parameters.AddWithValue("@StudentGradeID", student.StudentGradeID);
                        command.Parameters.AddWithValue("@SchoolID", student.SchoolID);
                        command.Parameters.AddWithValue("@IsActive", student.IsActive);

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

        public static async Task<bool> UpdateAsync(studentDTO student)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_students_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentID", student.StudentID);
                        //command.Parameters.AddWithValue("@PersonID", student.PersonID);
                        command.Parameters.AddWithValue("@StudentGradeID", student.StudentGradeID);
                        command.Parameters.AddWithValue("@SchoolID", student.SchoolID);
                        command.Parameters.AddWithValue("@IsActive", student.IsActive);

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

        public static async Task<bool> DeleteAsync(int studentID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_students_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentID", studentID);

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

        public static async Task<bool> IsExistsAsync(int studentID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {

                    using (var command = new SqlCommand("sp_students_Exists", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentID", studentID);

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
