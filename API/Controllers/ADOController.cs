using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using System.Data;

namespace API.Controllers
{
    [Route("api/ADO")]
    [ApiController]
    public class ADOController : Controller
    {
        private ADODocumentsRepository db;

        public ADOController()
        {
            db = new ADODocumentsRepository();
        }

        // GET: api/ADO/
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


        // GET: api/ADO/5
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

        // POST: api/ADO/
        [HttpPost]
        public IActionResult Post([FromBody]Document document)
        {
            if (document == null)
            {
                return BadRequest();
            }

            db.CreateDocument(document);
           
            return Ok();
        }

        // DELETE api/ADO/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            db.DeleteDocument(id);
           
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}