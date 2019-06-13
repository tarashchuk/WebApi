using System;
using Xunit;
using API.Controllers;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace XUnitTest
{
    public class UnitTestEF
    {
        ADOController ADOController;
        EFController EFController;

        public UnitTestEF()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DocumentContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Documentdb;Integrated Security=True;");
            ADOController = new ADOController();
            EFController = new EFController(new EFDocumentsRepository(new DocumentContext(optionsBuilder.Options)));
        }

        [Fact]
        public void TestGetAll()
        {
            var responseADO = ADOController.Get();
            Assert.IsType<OkObjectResult>(responseADO);

            var responseEF = EFController.Get();
            Assert.IsType<OkObjectResult>(responseEF);

        }

        [Fact]
        public void TestGet()
        {
            var responseADO = ADOController.Get(1);
            Assert.IsType<OkObjectResult>(responseADO);

            var responseEF = EFController.Get(1);
            Assert.IsType<OkObjectResult>(responseEF);

            var notFoundResponceADO = ADOController.Get(1000);
            Assert.IsType<NotFoundResult>(notFoundResponceADO);

            var notFoundResponceEF = EFController.Get(1000);
            Assert.IsType<NotFoundResult>(notFoundResponceEF);
        }

        [Fact]
        public void TestPost()
        {
            var testItem = new Document()
            {
                Amount = 17,
                Description = "Hii"
            };

            var responseADO = ADOController.Post(testItem);
            Assert.IsType<OkResult>(responseADO);

            var responseEF = EFController.Post(testItem);
            Assert.IsType<OkResult>(responseEF);
        }

        [Fact]
        public void TestDelete()
        {
            
            var responseADO = ADOController.Delete(1);
            Assert.IsType<OkResult>(responseADO);

            var responseEF = EFController.Delete(3);
            Assert.IsType<OkResult>(responseEF);
        }



    }
}
