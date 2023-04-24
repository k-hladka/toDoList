using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
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
        public List<Note> GetNotes(string sqlString = "")
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
        public void Create(Note note, string category, string typeStorage)
        {
            if (typeStorage == "sql")
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var sqlQuery = "INSERT INTO Notes (Name, FinalDate, Status, CategoryId) VALUES(@Name, @FinalDate, @Status, (SELECT Id FROM Category WHERE Name = @category))";
                    db.Execute(sqlQuery, new { Name = note.Name, FinalDate = note.FinalDate, Status = note.Status, category });
                }
            else
            {
                CreateXml(note, category);
            }
        }

        public void Completed(bool status, int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Notes SET Status = @Status WHERE Id = @Id";
                db.Execute(sqlQuery, new { Status = !status, Id = id });
            }
        }
        public List<Note> Sort(string FieldName)
        {
            return GetNotes($"SELECT * FROM Notes ORDER BY {FieldName}");
        }
        public void CreateXml(Note note, string category)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Note.xml");
            XmlElement root = xmlDocument.DocumentElement;

            var idNote = int.Parse(root.LastChild.SelectSingleNode("Id").InnerText);
            idNote++;

            XmlDocument xmlCategory = new XmlDocument();
            xmlCategory.Load("Category.xml");
            XmlElement rootCategory = xmlCategory.DocumentElement;

            XmlElement noteElem = xmlDocument.CreateElement("Note");

            XmlElement status = xmlDocument.CreateElement("Status");
            XmlText statusText = xmlDocument.CreateTextNode(note.Status.ToString());
            status.AppendChild(statusText);

            XmlElement idElem = xmlDocument.CreateElement("Id");
            XmlText idText = xmlDocument.CreateTextNode(idNote.ToString());
            idElem.AppendChild(idText);

            XmlElement name = xmlDocument.CreateElement("Name");
            XmlText nameText = xmlDocument.CreateTextNode(note.Name);
            name.AppendChild(nameText);

            XmlElement categoryId = xmlDocument.CreateElement("CategoryId");
            string idCatergory = String.Empty;
            foreach (XmlNode child in rootCategory.ChildNodes)
            {
                foreach (XmlNode childNode in child.ChildNodes)
                {
                    if (childNode.Name == "Name" && childNode.InnerText == category)
                    {
                        XmlText categoryIdText = xmlDocument.CreateTextNode(idCatergory);
                        categoryId.AppendChild(categoryIdText);
                    }
                    if (childNode.Name == "Id")
                    {
                        idCatergory = childNode.InnerText;
                    }
                }

            }

            XmlElement finalDate = xmlDocument.CreateElement("finalDate");
            XmlText finalDateText = xmlDocument.CreateTextNode(note.FinalDate.ToString());
            finalDate.AppendChild(finalDateText);

            noteElem.AppendChild(idElem);
            noteElem.AppendChild(status);
            noteElem.AppendChild(name);
            noteElem.AppendChild(categoryId);
            noteElem.AppendChild(finalDate);

            root.AppendChild(noteElem);
            xmlDocument.Save("Note.xml");
        }
        public void CompletedXml(bool status, int id)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("Note.xml");
            XmlElement root = xmlDocument.DocumentElement;
            bool check = false;

            foreach (XmlNode child in root.ChildNodes)
            {
                foreach (XmlNode childNode in child.ChildNodes)
                {
                    if (childNode.Name == "Id" && childNode.InnerText == id.ToString())
                        check = true;
                    if (childNode.Name == "Status" && check)
                    {
                        childNode.InnerText = (!bool.Parse(childNode.InnerText)).ToString();
                        check = false;
                        break;
                    }
                }

            }
            xmlDocument.Save("Note.xml");
        }
    }
}