using Microsoft.Data.SqlClient;
using System.Data;
namespace SchoolAPiDataAccessLayer
{
    public record departmentDTO(int ID, string Name, int SchoolID);

    public class clsDepartmentsData
    {
        public static async Task<IEnumerable<departmentDTO>> GetAllAsync()
        {
            var department = new List<departmentDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_departments_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                department.Add(new departmentDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                        reader.GetString(reader.GetOrdinal("DepartmentName")),
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

            return department;
        }



        public static async Task<departmentDTO> GetByIdAsync(int ID)
        {
            departmentDTO department = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_departments_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DepartmentID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                department = new departmentDTO
                                             (
                                                reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                        reader.GetString(reader.GetOrdinal("DepartmentName")),
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

            return department;
        }
    }
}
