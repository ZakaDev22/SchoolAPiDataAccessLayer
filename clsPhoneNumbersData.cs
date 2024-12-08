
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
    }
}
