using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace API.Models
{
    public class DocumentContext : DbContext
    {
        
        public DocumentContext(DbContextOptions<DocumentContext> options)
       : base(options)
        {
            //Для того, что бы создалась база данных в Package Manager Console  нужно ввести Update-database
            Database.EnsureCreated();
            // Для того, что бы было с миграциями, меняем на метод  Database.Migrate();  в Package Manager Console  нужно ввести Add-Migration API.Models.DocumentContext, а потом Update-database
            //Миграции позоляют вносить изменения в базу данных при изменениях моделей и контекста данных
            // Database.Migrate();
        }
       
       
        

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentStatus> DocumentStatuses { get; set; }
        public DbSet<Status> Statuses { get; set; }

        //Во время создания базы данных этот метод сопоставляет между классами и их свойствами и таблицами и их столбцами
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>().HasKey(d => d.DocumentId);
            modelBuilder.Entity<Document>().Property(d => d.Amount).IsRequired();// не null
            modelBuilder.Entity<Document>().Property(d => d.Description).IsRequired();
            

            modelBuilder.Entity<DocumentStatus>().HasKey(ds => ds.DocumentStatusId);
            modelBuilder.Entity<DocumentStatus>().Property(ds => ds.Date).IsRequired();
            modelBuilder.Entity<DocumentStatus>().Property(ds => ds.StatusId).IsRequired();
            modelBuilder.Entity<DocumentStatus>().HasOne(ds => ds.Documents).WithOne(d => d.DS).HasForeignKey<DocumentStatus>(ds=>ds.DocumentId);

           
            modelBuilder.Entity<Status>().Property(s => s.Name).IsRequired();
            modelBuilder.Entity<Status>().HasOne(s => s.DocumentStatuses).WithOne(ds => ds.Statuses)
                .HasForeignKey<Status>(s=>s.StatusId).HasPrincipalKey<DocumentStatus>(ds=>ds.StatusId);
            // Метод HasPrincipalKey указывает на свойство связанной сущности, на которую будет ссылаться свойство - внешний ключ.
            //Кроме того, для свойства, указанного в HasPrincipalKey(), будет создавать альтернативный ключ


            base.OnModelCreating(modelBuilder);
        }
        
    }
}
