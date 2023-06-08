using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightDocumentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISignatureRepository _signatureRepository;

        public DocumentsController(IDocumentRepository documentRepository, ICategoryRepository categoryRepository, IFlightRepository flightRepository, IAccountRepository accountRepository, ISignatureRepository signatureRepository)
        {
            _documentRepository = documentRepository;
            _categoryRepository = categoryRepository;
            _flightRepository = flightRepository;
            _accountRepository = accountRepository;
            _signatureRepository = signatureRepository;
        }

        // GET: api/Documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
        {
            var result = await _documentRepository.GetAllDocumentAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get all successfully",
                Data = result
            });
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(Guid id)
        {
            var result = await _documentRepository.FindDocumentById(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This document doesn't exist",
                    Data = null
                });
            }
            return Ok(new Notification
            {
                Success = true,
                Message = "Get all successfully",
                Data = result
            });
        }

        // POST: api/Documents/SendFile
        [HttpPost("SendFile")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Document>> PostSendDocument([FromForm] DocumentDTO document)
        {
            if (document.File == null || document.File.Length == 0)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please choose a file",
                    Data = null
                });
            }

            var category = await _categoryRepository.FindCategoryByIdAsync(document.CategoryId);
            if (category == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This category doesn't exist",
                    Data = null
                });
            }

            var flight = await _flightRepository.FindFlightByIdAsync(document.FlightId);
            if (flight == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This flight doesn't exist",
                    Data = null
                });
            }

            var account = await _accountRepository.FindAccountByIdAsync(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (account == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This account doesn't exist",
                    Data = null
                });
            }

            var result = await _documentRepository.InsertSentDocumentAsync(document, account, flight);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert successfully",
                Data = result
            });
        }

        // POST: api/Documents/ReturnFile/{documentId}
        [HttpPost("ReturnFile/{documentId}")]
        [Authorize(Roles = "Pilot")]
        public async Task<ActionResult<Document>> PostReturnDocument(Guid documentId, IFormFile file, IFormFile signatureFile)
        {
            var document = await _documentRepository.FindDocumentById(documentId);
            if (document == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This document doesn't exist",
                    Data = null
                });
            }

            if (file == null || file.Length == 0)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please choose a file",
                    Data = null
                });
            }

            if (signatureFile == null || signatureFile.Length == 0)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please choose a signature file",
                    Data = null
                });
            }

            if (await _documentRepository.CheckDocumentReturnAsync(document) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This document you have already returned",
                    Data = null
                });
            }

            var account = await _accountRepository.FindAccountByIdAsync(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (account == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This account doesn't exist",
                    Data = null
                });
            }

            var result = await _documentRepository.InsertReturnDocumentAsync(document, account, file);
            await _signatureRepository.InsertSignatureAsync(signatureFile, result);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert successfully",
                Data = result
            });
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            var document = await _documentRepository.FindDocumentById(id);
            if (document == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This document doesn't exist",
                    Data = null
                });
            }

            if (await Cloudinary.Destroy(document.PublicUrl!) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Delete error",
                    Data = null
                });
            }
            else
            {
                var signature = await _signatureRepository.FindSignatureByDocumentId(document.DocumentId);
                if (signature != null)
                {
                    if (await Cloudinary.Destroy(signature.PublicUrl!) == false)
                    {
                        return Ok(new Notification
                        {
                            Success = false,
                            Message = "Delete error",
                            Data = null
                        });
                    }
                    else
                    {
                        await _signatureRepository.DeleteSignatureAsync(signature);
                    }
                }
                await _documentRepository.DeleteDocumentAsync(document);
            }
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete successfully",
                Data = null
            });
        }
    }
}
