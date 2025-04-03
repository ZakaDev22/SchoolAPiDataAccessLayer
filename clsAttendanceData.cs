using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record attendanceDTO(int ID, int StudentID, int ClassID, DateTime Date, bool Status);

    public class clsAttendanceData
    {
        public static async Task<IEnumerable<attendanceDTO>> GetAllAsync()
        {
            var attendaces = new List<attendanceDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_attendance_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                attendaces.Add(new attendanceDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("AttendanceID")),
                                        reader.GetInt32(reader.GetOrdinal("StudentID")),
                                        reader.GetInt32(reader.GetOrdinal("ClassID")),
                                        reader.GetDateTime(reader.GetOrdinal("AttendanceDate")),
                                        reader.GetBoolean(reader.GetOrdinal("Status"))

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

            return attendaces;
        }

        public static async Task<attendanceDTO> GetByIdAsync(int ID)
        {
            attendanceDTO attendance = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_attendance_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                attendance = new attendanceDTO
                                                (
                                                    reader.GetInt32(reader.GetOrdinal("AttendanceID")),
                                                    reader.GetInt32(reader.GetOrdinal("StudentID")),
                                                    reader.GetInt32(reader.GetOrdinal("ClassID")),
                                                    reader.GetDateTime(reader.GetOrdinal("AttendanceDate")),
                                                    reader.GetBoolean(reader.GetOrdinal("Status"))

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

            return attendance;
        }

        public static async Task<int> AddAsync(attendanceDTO Attendance)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_attendance_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentID", Attendance.StudentID);
                        command.Parameters.AddWithValue("@ClassID", Attendance.ClassID);
                        command.Parameters.AddWithValue("@Status", Attendance.Status);


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

        public static async Task<bool> UpdateAsync(attendanceDTO Attendance)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_attendance_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@AttendanceID", Attendance.ID);
                        command.Parameters.AddWithValue("@StudentID", Attendance.StudentID);
                        command.Parameters.AddWithValue("@ClassID", Attendance.ClassID);
                        command.Parameters.AddWithValue("@Status", Attendance.Status);

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
                    using (var command = new SqlCommand("sp_attendance_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@AttendanceID", ID);

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

                    using (var command = new SqlCommand("sp_attendance_Exist", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@AttendanceID", ID);

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
