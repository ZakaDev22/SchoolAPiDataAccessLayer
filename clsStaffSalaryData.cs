using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record staffSalaryDTO(int ID, int StaffID, decimal Salary, DateTime EffectiveDate, decimal Bonus, decimal Deductions);

    public class clsStaffSalaryData
    {
        public static async Task<IEnumerable<staffSalaryDTO>> GetAllAsync()
        {
            var Staffs = new List<staffSalaryDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_staffSalary_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Staffs.Add(new staffSalaryDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("ID")),
                                        reader.GetInt32(reader.GetOrdinal("StaffID")),
                                        reader.GetDecimal(reader.GetOrdinal("Salary")),
                                        reader.GetDateTime(reader.GetOrdinal("EffectiveDate")),
                                        reader.GetDecimal(reader.GetOrdinal("Bonus")),
                                        reader.GetDecimal(reader.GetOrdinal("Deductions"))
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

        public static async Task<staffSalaryDTO> GetByIdAsync(int ID)
        {
            staffSalaryDTO staff = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_staffSalary_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                staff = new staffSalaryDTO
                                             (
                                                   reader.GetInt32(reader.GetOrdinal("ID")),
                                        reader.GetInt32(reader.GetOrdinal("StaffID")),
                                        reader.GetDecimal(reader.GetOrdinal("Salary")),
                                        reader.GetDateTime(reader.GetOrdinal("EffectiveDate")),
                                        reader.GetDecimal(reader.GetOrdinal("Bonus")),
                                        reader.GetDecimal(reader.GetOrdinal("Deductions"))
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


        public static async Task<int> AddAsync(staffSalaryDTO staff)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_staffSalary_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StaffID", staff.StaffID);
                        command.Parameters.AddWithValue("@Salary", staff.Salary);
                        command.Parameters.AddWithValue("@Bonus", staff.Bonus);
                        command.Parameters.AddWithValue("@Deductions", staff.Deductions);

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

        public static async Task<bool> UpdateAsync(staffSalaryDTO staff)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_staffSalary_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", staff.ID);
                        command.Parameters.AddWithValue("@Salary", staff.Salary);
                        command.Parameters.AddWithValue("@Bonus", staff.Bonus);
                        command.Parameters.AddWithValue("@Deductions", staff.Deductions);

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

        public static async Task<bool> DeleteAsync(int staffSalaryID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_staffSalary_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", staffSalaryID);

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

        public static async Task<bool> IsExistsAsync(int StaffSalaryID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {

                    using (var command = new SqlCommand("sp_staffSalary_Exist", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", StaffSalaryID);

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
