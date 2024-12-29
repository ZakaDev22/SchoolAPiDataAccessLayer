using Microsoft.Data.SqlClient;
using System.Data;
namespace SchoolAPiDataAccessLayer
{
    public record countryDTO(int ID, string Name, string Code);
    public class clsCountriesData
    {
        public static async Task<IEnumerable<countryDTO>> GetAllAsync()
        {
            var countries = new List<countryDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_Country_GetAllCountries", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                countries.Add(new countryDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("CountryID")),
                                        reader.GetString(reader.GetOrdinal("CountryName")),
                                        reader.GetString(reader.GetOrdinal("CountryCode"))

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

            return countries;
        }

        public static async Task<countryDTO> GetByIdAsync(int ID)
        {
            countryDTO address = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_Country_FindCountryByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CountryID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                address = new countryDTO
                                                (
                                                     reader.GetInt32(reader.GetOrdinal("CountryID")),
                                        reader.GetString(reader.GetOrdinal("CountryName")),
                                        reader.GetString(reader.GetOrdinal("CountryCode"))

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

        public static async Task<countryDTO> GetByCountryNameAsync(string Name)
        {
            countryDTO country = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_Country_FindCountryByName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CountryName", Name);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                country = new countryDTO
                                                (
                                                     reader.GetInt32(reader.GetOrdinal("CountryID")),
                                        reader.GetString(reader.GetOrdinal("CountryName")),
                                        reader.GetString(reader.GetOrdinal("CountryCode"))

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

            return country;
        }

        public static async Task<countryDTO> GetByCountryCodeAsync(string Code)
        {
            countryDTO country = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_Country_FindCountryByCode", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CountryCode", Code);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                country = new countryDTO
                                                (
                                                     reader.GetInt32(reader.GetOrdinal("CountryID")),
                                        reader.GetString(reader.GetOrdinal("CountryName")),
                                        reader.GetString(reader.GetOrdinal("CountryCode"))

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

            return country;
        }
    }
}
