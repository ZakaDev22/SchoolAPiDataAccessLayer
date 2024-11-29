using Microsoft.Data.SqlClient;
using System.Data;


namespace SchoolAPiDataAccessLayer
{
    public record staffDTO(int StaffID, int PersonID, int JobTitleID, int DepartmentID, int SchoolID, int StaffSalaryID);

    public class clsStaffData
    {
        public static async Task<IEnumerable<staffDTO>> GetAllAsync()
        {
            var Staffs = new List<staffDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_staffs_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Staffs.Add(new staffDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("StaffID")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetInt32(reader.GetOrdinal("JobTitleID")),
                                        reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                        reader.GetInt32(reader.GetOrdinal("SchoolID")),
                                        reader.GetInt32(reader.GetOrdinal("StaffSalaryID"))
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

            return Staffs;
        }

        public static async Task<staffDTO> GetByIdAsync(int staffID)
        {
            staffDTO staff = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_staffs_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", staffID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                staff = new staffDTO
                                             (
                                                  reader.GetInt32(reader.GetOrdinal("StaffID")),
                                                  reader.GetInt32(reader.GetOrdinal("PersonID")),
                                                  reader.GetInt32(reader.GetOrdinal("JobTitleID")),
                                                  reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                                  reader.GetInt32(reader.GetOrdinal("SchoolID")),
                                                  reader.GetInt32(reader.GetOrdinal("StaffSalaryID"))
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

            return staff;
        }


        public static async Task<int> AddAsync(staffDTO staff)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_staffs_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", staff.PersonID);
                        command.Parameters.AddWithValue("@JobTitleID", staff.JobTitleID);
                        command.Parameters.AddWithValue("@DepartmentID", staff.DepartmentID);
                        command.Parameters.AddWithValue("@SchoolID", staff.SchoolID);
                        command.Parameters.AddWithValue("@StaffSalaryID", staff.StaffSalaryID);

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

        public static async Task<bool> UpdateAsync(staffDTO staff)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_staffs_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", staff.StaffID);
                        command.Parameters.AddWithValue("@PersonID", staff.PersonID);
                        command.Parameters.AddWithValue("@JobTitleID", staff.JobTitleID);
                        command.Parameters.AddWithValue("@DepartmentID", staff.DepartmentID);
                        command.Parameters.AddWithValue("@SchoolID", staff.SchoolID);
                        command.Parameters.AddWithValue("@StaffSalaryID", staff.StaffSalaryID);

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

        public static async Task<bool> DeleteAsync(int staffID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_staffs_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", staffID);

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

        public static async Task<bool> IsExistsAsync(int staffID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {

                    using (var command = new SqlCommand("sp_staffs_Exist", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", staffID);

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

        public static async Task<bool> IsExistsByPersonIDAsync(int PersonID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {

                    using (var command = new SqlCommand("sp_staffs_ExistByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", PersonID);

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
