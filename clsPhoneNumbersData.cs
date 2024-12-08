
using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record phoneNumberDTO(int ID, string Number, int PersonID, int PhoneTypeID, bool IsPrimary);

    public class clsPhoneNumbersData
    {
        public static async Task<IEnumerable<phoneNumberDTO>> GetAllAsync()
        {
            var numbers = new List<phoneNumberDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_phoneNumbers_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                numbers.Add(new phoneNumberDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("PhoneNumberID")),
                                        reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetInt32(reader.GetOrdinal("PhoneTypeID")),
                                        reader.GetBoolean(reader.GetOrdinal("IsPrimary"))
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

            return numbers;
        }

        public static async Task<phoneNumberDTO> GetByIdAsync(int numberID)
        {
            phoneNumberDTO phoneNumber = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_phoneNumbers_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PhoneNumberID", numberID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                phoneNumber = new phoneNumberDTO
                                             (
                                               reader.GetInt32(reader.GetOrdinal("PhoneNumberID")),
                                        reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetInt32(reader.GetOrdinal("PhoneTypeID")),
                                        reader.GetBoolean(reader.GetOrdinal("IsPrimary"))
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

            return phoneNumber;
        }

        public static async Task<int> AddAsync(phoneNumberDTO number)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_phoneNumbers_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PhoneNumber", number.Number);
                        command.Parameters.AddWithValue("@PersonID", number.PersonID);
                        command.Parameters.AddWithValue("@PhoneTypeID", number.PhoneTypeID);
                        command.Parameters.AddWithValue("@IsPrimary", number.IsPrimary);

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

        public static async Task<bool> UpdateAsync(phoneNumberDTO number)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_phoneNumbers_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PhoneNumberID", number.ID);
                        command.Parameters.AddWithValue("@PhoneNumber", number.Number);
                        command.Parameters.AddWithValue("@PersonID", number.PersonID);
                        command.Parameters.AddWithValue("@PhoneTypeID", number.PhoneTypeID);
                        command.Parameters.AddWithValue("@IsPrimary", number.IsPrimary);



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
                    using (var command = new SqlCommand("sp_phoneNumbers_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PhoneNumberID", ID);

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

                    using (var command = new SqlCommand("sp_phoneNumbers_Exist", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PhoneNumberID", ID);

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
