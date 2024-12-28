
using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolAPiDataAccessLayer
{
    public record RelationshipTypeDTO(int ID, string Name);
    public class clsRelationshipTypesData
    {
        public static async Task<IEnumerable<RelationshipTypeDTO>> GetAllAsync()
        {
            var relationshipType = new List<RelationshipTypeDTO>();

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_relationshiptypes_FindAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                relationshipType.Add(new RelationshipTypeDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("RelationshipTypeID")),
                                        reader.GetString(reader.GetOrdinal("RelationshipTypeName"))
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

            return relationshipType;
        }



        public static async Task<RelationshipTypeDTO> GetByIdAsync(int ID)
        {
            RelationshipTypeDTO relationshipType = null;

            try
            {
                using (var connection = new SqlConnection(DataGlobal._connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_relationshiptypes_FindByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RelationshipTypeID", ID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                relationshipType = new RelationshipTypeDTO
                                             (
                                               reader.GetInt32(reader.GetOrdinal("RelationshipTypeID")),
                                                 reader.GetString(reader.GetOrdinal("RelationshipTypeName"))
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

            return relationshipType;
        }
    }
}
