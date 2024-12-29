using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record addressDTO(int ID, string Street, string City, int StateID, int CountryID);
    public class clsAddressesData
    {
        public static async Task<IEnumerable<addressDTO>> GetAllAsync()
        {
            var addresses = new List<addressDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_addresses_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                addresses.Add(new addressDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("AddressID")),
                                        reader.GetString(reader.GetOrdinal("Street")),
                                        reader.GetString(reader.GetOrdinal("City")),
                                        reader.GetInt32(reader.GetOrdinal("StateID")),
                                        reader.GetInt32(reader.GetOrdinal("CountryID"))

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

            return addresses;
        }

        public static async Task<addressDTO> GetByIdAsync(int ID)
        {
            addressDTO address = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_addresses_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AddressID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                address = new addressDTO
                                                (
                                                   reader.GetInt32(reader.GetOrdinal("AddressID")),
                                        reader.GetString(reader.GetOrdinal("Street")),
                                        reader.GetString(reader.GetOrdinal("City")),
                                        reader.GetInt32(reader.GetOrdinal("StateID")),
                                        reader.GetInt32(reader.GetOrdinal("CountryID"))

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

            return address;
        }

        public static async Task<IEnumerable<addressDTO>> GetByCityNameAsync(string city)
        {
            var addresses = new List<addressDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_addresses_FindByCity", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@City", city);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                addresses.Add(new addressDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("AddressID")),
                                        reader.GetString(reader.GetOrdinal("Street")),
                                        reader.GetString(reader.GetOrdinal("City")),
                                        reader.GetInt32(reader.GetOrdinal("StateID")),
                                        reader.GetInt32(reader.GetOrdinal("CountryID"))

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

            return addresses;
        }

        public static async Task<bool> DeleteAsync(int ID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_addresses_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@AddressID", ID);

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

        public static async Task<int> AddAsync(addressDTO address)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_addresses_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Street", address.Street);
                        command.Parameters.AddWithValue("@City", address.City);
                        command.Parameters.AddWithValue("@StateID", address.StateID);
                        command.Parameters.AddWithValue("@CountryID", address.CountryID);


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

        public static async Task<bool> UpdateAsync(addressDTO address)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_addresses_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@AddressID", address.ID);
                        command.Parameters.AddWithValue("@Street", address.Street);
                        command.Parameters.AddWithValue("@City", address.City);
                        command.Parameters.AddWithValue("@StateID", address.StateID);
                        command.Parameters.AddWithValue("@CountryID", address.CountryID);

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
    }
}
