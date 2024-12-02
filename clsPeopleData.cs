
using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    // record Fpr Person DTO To Send And Retrive His Informations Instead Of Full Object
    public record PersonDTO(int ID, string FirstName, string LastName, DateTime DateOfBirth, bool Gender, int SchoolID, int AddressID);


    public class clsPeopleData
    {


        public static async Task<IEnumerable<PersonDTO>> GetAllAsync()
        {
            var persons = new List<PersonDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_people_GetAllPeople", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                persons.Add(new PersonDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("ID")),
                                        reader.GetString(reader.GetOrdinal("FirstName")),
                                        reader.GetString(reader.GetOrdinal("LastName")),
                                        reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                        reader.GetBoolean(reader.GetOrdinal("Gender")),
                                         reader.GetInt32(reader.GetOrdinal("SchoolID")),
                                          reader.GetInt32(reader.GetOrdinal("AddressID"))
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

            return persons;
        }

        public static async Task<PersonDTO> GetByIdAsync(int personId)
        {
            PersonDTO person = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_people_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", personId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                person = new PersonDTO
                                             (
                                                 reader.GetInt32(reader.GetOrdinal("ID")),
                                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                                 reader.GetString(reader.GetOrdinal("LastName")),
                                                 reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                                 reader.GetBoolean(reader.GetOrdinal("Gender")),
                                                 reader.GetInt32(reader.GetOrdinal("SchoolID")),
                                                 reader.GetInt32(reader.GetOrdinal("AddressID"))
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

            return person;
        }

        public static async Task<PersonDTO> GetByNameAsync(string FirstName, string LastName)
        {
            PersonDTO person = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_people_FindByName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@LastName", LastName);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                person = new PersonDTO
                                             (
                                                 reader.GetInt32(reader.GetOrdinal("ID")),
                                                 reader.GetString(reader.GetOrdinal("FirstName")),
                                                 reader.GetString(reader.GetOrdinal("LastName")),
                                                 reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                                 reader.GetBoolean(reader.GetOrdinal("Gender")),
                                                 reader.GetInt32(reader.GetOrdinal("SchoolID")),
                                                 reader.GetInt32(reader.GetOrdinal("AddressID"))
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

            return person;
        }


        public static async Task<int> AddAsync(PersonDTO person)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    using (var command = new SqlCommand("sp_people_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@FirstName", person.FirstName);
                        command.Parameters.AddWithValue("@LastName", person.LastName);
                        command.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
                        command.Parameters.AddWithValue("@Gender", person.Gender);
                        command.Parameters.AddWithValue("@SchoolID", person.SchoolID);
                        command.Parameters.AddWithValue("@AddressID", person.AddressID);

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

        public static async Task<bool> UpdateAsync(PersonDTO person)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_people_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", person.ID);
                        command.Parameters.AddWithValue("@FirstName", person.FirstName);
                        command.Parameters.AddWithValue("@LastName", person.LastName);
                        command.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
                        command.Parameters.AddWithValue("@Gender", person.Gender);
                        command.Parameters.AddWithValue("@SchoolID", person.SchoolID);
                        command.Parameters.AddWithValue("@AddressID", person.AddressID);

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

        public static async Task<bool> DeleteAsync(int personId)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_people_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", personId);

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

        public static async Task<bool> IsExistsByIDAsync(int PersonID)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {

                    using (var command = new SqlCommand("sp_people_ExistsByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", PersonID);

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

        public static async Task<bool> IsExistsByNameAsync(string FirstName, string LastName)
        {
            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {

                    using (var command = new SqlCommand("sp_people_ExistsByName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@LastName", LastName);

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