using Microsoft.AspNetCore.Mvc;
using newOne.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using newOne.Models;
using NuGet.Packaging.Signing;
using DbTask = newOne.Models.Task;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace newOne.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        public DocumentsController(IDocumentRepository documentRepository)
        {
            documentRepository = _documentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDocumentsByIds([FromQuery]IEnumerable<int> ids)
        {
            // if there is no Id, return bad request
            if(ids.Count() == 0 || !ids.Any())
            {
                return BadRequest("atleast one Id is required");
            }

            var document = await _documentRepository.GetByDocumentId(ids);

            if (document.Count() == 0 || !document.Any())
            {
                return NotFound("documents not found");
            }

            return Ok(document);
        }

        [HttpDelete("deleteDoc/documentId/{id}")]
        public async Task<IActionResult> DeleteDocumentsById(int id)
        {
            await _documentRepository.DeleteDocument(id);
            return Ok();
        }

        [HttpPost("updateDocument}")]
        public async Task<IActionResult> AddDocument([FromQuery ]Document document)
        {
            await _documentRepository.AddDocument(document);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateDocument([FromQuery] Document doc)
        {
            await _documentRepository.UpdateDocument(doc);
            return Ok();
        }

    }
}
