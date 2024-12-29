using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record stateDTO(int ID, int CountryID, string Name);
    public class clsStatesData
    {
        public static async Task<IEnumerable<stateDTO>> GetAllAsync()
        {
            var states = new List<stateDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_States_GetAllStates", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                states.Add(new stateDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("StateID")),
                                        reader.GetInt32(reader.GetOrdinal("CountryID")),
                                        reader.GetString(reader.GetOrdinal("StateName"))

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

            return states;
        }

        public static async Task<IEnumerable<stateDTO>> GetAllStatesByCountryIDAsync(int ID)
        {
            var states = new List<stateDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_States_GetAllStatesByCountryID", connection))
                    {
                        command.Parameters.AddWithValue("@CountryID", ID);

                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                states.Add(new stateDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("StateID")),
                                        reader.GetInt32(reader.GetOrdinal("CountryID")),
                                        reader.GetString(reader.GetOrdinal("StateName"))

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

            return states;
        }

        public static async Task<stateDTO> GetByIdAsync(int ID)
        {
            stateDTO state = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_States_FindStateByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StateID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                state = new stateDTO
                                                (
                                                     reader.GetInt32(reader.GetOrdinal("StateID")),
                                        reader.GetInt32(reader.GetOrdinal("CountryID")),
                                        reader.GetString(reader.GetOrdinal("StateName"))

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

            return state;
        }

        public static async Task<stateDTO> GetByStateNameAsync(string Name)
        {
            stateDTO state = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_States_FindStateByName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StateName", Name);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                state = new stateDTO
                                                (
                                                  reader.GetInt32(reader.GetOrdinal("StateID")),
                                        reader.GetInt32(reader.GetOrdinal("CountryID")),
                                        reader.GetString(reader.GetOrdinal("StateName"))

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

            return state;
        }
    }
}
