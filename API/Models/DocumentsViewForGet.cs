using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    //Етот класс будет использоватся для вывода результата
    public class DocumentsViewForGet
    {
        public int DocumentId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
    }
}
