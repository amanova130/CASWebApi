using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CASWebApi.Models;
using CASWebApi.Services;
using Exportable.Engines;
using Exportable.Engines.Excel;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileHandlerController : ControllerBase
    {
        HelperService _helperService;
        public FileHandlerController(HelperService helperService)
        {
            _helperService = helperService;
        }

        /// <summary>
        /// Function to upload image to folder -Resources/Images
        /// </summary>
        /// <returns>Return path to image in backend, we will save it in database</returns>
        [HttpPost("uploadImage", Name = nameof(UploadImage)), DisableRequestSizeLimit]
        public IActionResult UploadImage()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    //Return dbPath to FrontEnd, so we can save it in our object.
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// ExportToExcell- function is responsible to export a file to excel
        /// </summary>
        /// <param name="className">Class name, it's collection that we want to export tot excel</param>
        /// <returns>Return file content, file content Type and fileName</returns>
        [HttpGet("exportToExcell", Name = nameof(ExportToExcell))]
        public FileContentResult ExportToExcell(string className)
        {
            if (className != null)
            {
                var list = _helperService.GetAll(className);
                if (list.Count > 0)
                {
                    var excelStream = _helperService.ToExcelFile(list);
                    var excelOptions = SaveOptions.XlsDefault;
                    return File(excelStream.ToArray(), excelOptions.ContentType, "ReportStudents.xls");
                }
                else
                    return null;
                
            }
            else
                return null;
           
        }
    }
}
