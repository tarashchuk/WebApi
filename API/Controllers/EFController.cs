using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models;

namespace API.Controllers
{
    [Route("api/EF")]
    [ApiController]
    public class EFController : Controller
    {
        private IDocumentsRepository db;

        public EFController(IDocumentsRepository documents)
        {
            db = documents;
        }


        // GET: api/EF/  
        [HttpGet]
        public ActionResult Get()
        {
            var documents = db.GetDocuments();
            if (documents == null)
            {
                return NotFound();
            }
            return new OkObjectResult(documents);
        }

        // GET: api/EF/5
        [HttpGet("{id}")]
        public ActionResult Get([FromRoute] int id)
        {
           
            var document = db.GetDocument(id);

            if (document == null)
            {
                return NotFound();
            }

            return new OkObjectResult(document);
        }

        // POST: api/EF/
        [HttpPost]
        public IActionResult Post([FromBody]Document document)
        {
            if (document == null)
            {
                return BadRequest();
            }

            db.CreateDocument(document);
            db.Save();
            return Ok();
        }

        // DELETE api/EF/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            db.DeleteDocument(id);
            db.Save();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}