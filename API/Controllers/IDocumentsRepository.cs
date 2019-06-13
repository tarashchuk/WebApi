using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Controllers
{
    public interface IDocumentsRepository: IDisposable
        
    {
        IEnumerable<DocumentsViewForGet> GetDocuments(); // получение всех объектов, для метода Get
        DocumentsViewForGet GetDocument(int id); // получение одного объекта по id, для метода Get/id
        void CreateDocument(Document document); // создание объекта, для метода Post
        void DeleteDocument(int id); // удаление объекта по id, для метода Delete

        void Save();  // сохранение изменений
    }
}

