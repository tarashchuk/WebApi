using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Document
    {
     
        public int DocumentId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }

        public DocumentStatus DS { get; set; }
    }

    public class DocumentStatus
    {
    
        public int DocumentStatusId { get; set; }

        public int DocumentId { get; set; }
        
        public int StatusId { get; set; }
        public DateTime Date { get; set; }

        public Document Documents { get; set; }
        public Status Statuses { get; set; }
    }

    public class Status
    {

        public int StatusId { get; set; }
        public string Name { get; set; }


        public DocumentStatus DocumentStatuses { get; set; }
    }
}
