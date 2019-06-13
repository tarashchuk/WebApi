using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using System.Data.SqlClient;
using System.Data;


namespace API.Controllers
{
    public class ADODocumentsRepository : IDocumentsRepository
    {
        private string connectionString;
        private SqlConnection connection;

        public ADODocumentsRepository()
        {
            this.connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Documentdb;Integrated Security=True;";
            this.connection = new SqlConnection(connectionString);
        }

        public void CreateDocument(Document document)
        {
            
            connection.Open();
            SqlCommand sqlCmd = new SqlCommand("INSERT INTO dbo.Documents (dbo.Documents.Amount, dbo.Documents.Description) " +
                                                                        "Values (@Amount,@Description)", connection);
            
            sqlCmd.Parameters.AddWithValue("@Amount", document.Amount);
            sqlCmd.Parameters.AddWithValue("@Description", document.Description);
            sqlCmd.ExecuteNonQuery();
            sqlCmd.CommandText = "SELECT @@IDENTITY";
            int lastId = Convert.ToInt32(sqlCmd.ExecuteScalar());

            connection.Close();

             sqlCmd = new SqlCommand("INSERT INTO  dbo.DocumentStatuses ( dbo.DocumentStatuses.DocumentId, dbo.DocumentStatuses.StatusId, dbo.DocumentStatuses.Date)" +
               "Values (@DocumentId,@StatusId,@Date)", connection);
           
            sqlCmd.Parameters.AddWithValue("@DocumentId", lastId);
            sqlCmd.Parameters.AddWithValue("@StatusId", lastId);
            sqlCmd.Parameters.AddWithValue("@Date", DateTime.Now);
            connection.Open();
            sqlCmd.ExecuteNonQuery();
            connection.Close();

            sqlCmd = new SqlCommand("INSERT INTO  dbo.Statuses ( dbo.Statuses.StatusId, dbo.Statuses.Name)" +
               "Values (@StatusId,@Name)", connection);

            sqlCmd.Parameters.AddWithValue("@StatusId", lastId);
            sqlCmd.Parameters.AddWithValue("@Name", "CREATED");
            connection.Open();
            sqlCmd.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteDocument(int id)
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Documentdb;Integrated Security=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand sqlCmd = new SqlCommand("Update dbo.Statuses Set dbo.Statuses.Name = 'DELETED' " +
                                 "WHERE dbo.Statuses.StatusId=" + id + "",connection);
           

            sqlCmd.ExecuteNonQuery();
            connection.Close();
        }


        public DocumentsViewForGet GetDocument(int id)
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Documentdb;Integrated Security=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand sqlCmd = new SqlCommand("Select dbo.Documents.DocumentId,dbo.Documents.Amount,dbo.Documents.Description,dbo.DocumentStatuses.Date " +
               "from dbo.Documents " +
               "join dbo.DocumentStatuses on dbo.Documents.DocumentId = dbo.DocumentStatuses.DocumentId " +
               "join dbo.Statuses on dbo.DocumentStatuses.StatusId = dbo.Statuses.StatusId " +
               "Where dbo.Statuses.Name = 'CREATED' " +
               "AND dbo.Documents.DocumentId=" + id + "", connection);
            connection.Open();
            SqlDataReader reader = sqlCmd.ExecuteReader();
            DocumentsViewForGet document = null;
            while (reader.Read())
            {
                document = new DocumentsViewForGet();
                document.DocumentId = Convert.ToInt32(reader.GetValue(0));
                document.Amount = Convert.ToInt32(reader.GetValue(1));
                document.Description = reader.GetValue(2).ToString();
                document.Date = reader.GetValue(3).ToString(); 
            }

            connection.Close();

            if (document == null)
                return null;

            return document;
        }

        public IEnumerable<DocumentsViewForGet> GetDocuments()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Documentdb;Integrated Security=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand sqlCmd = new SqlCommand("Select dbo.Documents.DocumentId,dbo.Documents.Amount,dbo.Documents.Description,dbo.DocumentStatuses.Date " +
                "from dbo.Documents " +
                "join dbo.DocumentStatuses on dbo.Documents.DocumentId = dbo.DocumentStatuses.DocumentId " +
                "join dbo.Statuses on dbo.DocumentStatuses.StatusId = dbo.Statuses.StatusId " +
                "Where dbo.Statuses.Name = 'CREATED'", connection);

              connection.Open();
              SqlDataReader reader = sqlCmd.ExecuteReader();
              List<DocumentsViewForGet> documentsList = new List<DocumentsViewForGet>();

              DocumentsViewForGet document = null;
            while (reader.Read())
            {
                document = new DocumentsViewForGet();
                document.DocumentId = Convert.ToInt32(reader.GetValue(0));
                document.Amount = Convert.ToInt32(reader.GetValue(1));
                document.Description = reader.GetValue(2).ToString();
                document.Date = reader.GetValue(3).ToString();
                documentsList.Add(document);
            }
            connection.Close();

            if (documentsList.All(d => d == null))
                    return null;
                
            return documentsList;
        }

        public void Save()
        {
            connection.Close();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    connection.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
