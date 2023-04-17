using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using static Azure.Core.HttpHeader;

namespace toDoList.Models
{
    public class NoteRepository : INoteRepository
    {
        string? _connectionString = null;
        public NoteRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public List<Note> GetNotes(string sqlString="")
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Note>(sqlString).ToList();
            }
        }
        public List<Category> GetCategories()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Category>("SELECT * FROM Category").ToList();
            }
        }
        public void Create(Note note, string category)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Notes (Name, FinalDate, Status, CategoryId) VALUES(@Name, @FinalDate, @Status, (SELECT Id FROM Category WHERE Name = @category))";
                db.Execute(sqlQuery, new { Name = note.Name, FinalDate = note.FinalDate, Status = note.Status,  category });
            }
        }

        public void Completed(bool status, int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Notes SET Status = @Status WHERE Id = @Id";
                db.Execute(sqlQuery, new {Status = !status, Id = id});
            }
        }
        public List<Note> Sort(string FieldName)
        {
            return GetNotes($"SELECT * FROM Notes ORDER BY {FieldName}");
        }
    }
}
