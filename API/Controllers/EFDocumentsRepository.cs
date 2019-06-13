using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class EFDocumentsRepository:IDocumentsRepository
    {
        private DocumentContext DocumentDB;

        //Получаем контекст данных
        public EFDocumentsRepository(DocumentContext context)
        {
            this.DocumentDB = context;
        }

        public IEnumerable<DocumentsViewForGet> GetDocuments()
        {
            
          var documents = DocumentDB.Documents.Include(ds=>ds.DS).Where(d => d.DS.Statuses.Name == "CREATED").ToList();
            
            List<DocumentsViewForGet> documentsList = new List<DocumentsViewForGet>();
            foreach (var d in documents)
            {
                DocumentsViewForGet document = new DocumentsViewForGet();
                document.DocumentId = d.DocumentId;
                document.Amount = d.Amount;
                document.Description = d.Description;
                document.Date = d.DS.Date.ToString();
                documentsList.Add(document);
            }

        if(documentsList.All(d=>d==null))
            return null;

            return documentsList;
        }

        public DocumentsViewForGet GetDocument(int id)
        {
            var documentdate = DocumentDB.Documents.Include(ds => ds.DS).FirstOrDefault(d => d.DocumentId == id && d.DS.Statuses.Name == "CREATED");
            if (documentdate == null)
                return null;

                DocumentsViewForGet document = new DocumentsViewForGet();
                document.DocumentId = documentdate.DocumentId;
                document.Amount = documentdate.Amount;
                document.Description = documentdate.Description;
                document.Date = documentdate.DS.Date.ToString();
                
            

            return document;
        }
            

        public void CreateDocument(Document document)
        {
      
            DocumentDB.Documents.Add(document);
            DocumentDB.SaveChanges();

            DocumentStatus documentStatus = new DocumentStatus {DocumentId = document.DocumentId,StatusId=document.DocumentId,Date = DateTime.Now };
            DocumentDB.DocumentStatuses.Add(documentStatus);
            DocumentDB.SaveChanges();

            Status status = new Status { StatusId=documentStatus.StatusId,Name = "CREATED" };
            DocumentDB.Statuses.Add(status);

            DocumentDB.SaveChanges();

        }


        public void DeleteDocument(int id)
        {
            var document = DocumentDB.Documents.Include(s => s.DS.Statuses).FirstOrDefault(d => d.DocumentId==id);
            
            DocumentDB.Entry(document).State = EntityState.Modified;
            document.DS.Statuses.Name = "DELETED";
        }

        public void Save()
        {
            DocumentDB.SaveChanges();
        }


        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DocumentDB.Dispose();
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
