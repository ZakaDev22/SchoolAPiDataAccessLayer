using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record EmailDTO(int ID, string Address, int PersonID, int EmailTypeID, bool IsPrimary);
    public class clsEmailsData
    {
        public static async Task<IEnumerable<EmailDTO>> GetAllAsync()
        {
            var emails = new List<EmailDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_emails_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                emails.Add(new EmailDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("EmailID")),
                                        reader.GetString(reader.GetOrdinal("EmailAddress")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetInt32(reader.GetOrdinal("EmailTypeID")),
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

            return emails;
        }

        public static async Task<EmailDTO> GetByIdAsync(int EmailID)
        {
            EmailDTO email = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_emails_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmailID", EmailID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                email = new EmailDTO
                                             (
                                                reader.GetInt32(reader.GetOrdinal("EmailID")),
                                        reader.GetString(reader.GetOrdinal("EmailAddress")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetInt32(reader.GetOrdinal("EmailTypeID")),
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

            return email;
        }


    }
}
