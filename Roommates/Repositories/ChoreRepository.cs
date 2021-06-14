using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Repositories
{
    class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> chores = new List<Chore>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue,
                        };

                        chores.Add(chore);
                    }
                    reader.Close();
                    return chores;
                }
            }
        }

        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Chore chore = null;

                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        };
                    }
                    reader.Close();
                    return chore;
                }
            }
        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "@INSERT INTO Chore (Name) OUTPUT INSERTED.Id VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();
                    chore.Id = id;
                }
            }
        }

        public List<Chore> GetUnassigned()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
        SELECT Chore.Name, Roommate.FirstName 
        FROM Chore 
        LEFT JOIN RoommateChore ON RoommateChore.ChoreId = Chore.Id
        LEFT JOIN Roommate ON Roommate.Id = RoommateChore.RoommateId
        Where FirstName is null
";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> unassigned = new List<Chore>();

                    while (reader.Read())
                    {
                        string nameValue = reader.GetString(reader.GetOrdinal("Name"));

                        Chore chore = new Chore()
                        {
                            Name = nameValue
                        };
                        unassigned.Add(chore);
                    }
                    reader.Close();
                    return unassigned;
                }
            }
        }

        public void AssignChore(int roommateId, int choreId)
        {

        }
    }
}
