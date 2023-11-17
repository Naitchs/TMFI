using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static API.Entities.ExcelModels;


namespace API.Controllers
{
    [Authorize]
    public class IntegrateController : BaseApiController
    {
    private readonly IExcelService _excelService;
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IMediaService _mediaService;
     public IntegrateController(IExcelService excelService,
                                DataContext context,
                                IMediaService mediaService,
                                IMapper mapper)
        {
      _mediaService = mediaService;
      _context = context;
      _excelService = excelService;
       _mapper = mapper;
            
        }

//    [HttpPost("upload-excel")]
//         public IActionResult UploadFile(IFormFile file)
//         {
//             if (file == null || file.Length == 0)
//                 return BadRequest("Invalid file");

//             var allowedExtensions = new[] { ".xlsx", ".xlsm", ".xltx", ".xltm" };
//             var fileExtension = Path.GetExtension(file.FileName).ToLower();

//             if (!allowedExtensions.Contains(fileExtension))
//             {
//                 return BadRequest("Invalid file format. Supported formats are .xlsx, .xlsm, .xltx, .xltm");
//             }

//             try
//             {
//                 var filePath = Path.ChangeExtension(Path.GetTempFileName(), ".xlsx"); // temporary file path with .xlsx extension

//                 using (var stream = new FileStream(filePath, FileMode.Create))
//                 {
//                     file.CopyTo(stream);
//                 }

//                 _excelService.ProcessUploadedFile(filePath);

//                 // Optional: Clean up temporary file
//                 System.IO.File.Delete(filePath);

//                 return Ok("File uploaded successfully");
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Internal server error: {ex}");
//             }
//         }

        // [HttpPost("upload-excel")]
        // public IActionResult UploadFile(IFormFile file)
        // {
        //     if (file == null || file.Length == 0)
        //         return BadRequest("Invalid file");

        //     var allowedExtensions = new[] { ".xlsx", ".xlsm", ".xltx", ".xltm" };
        //     var fileExtension = Path.GetExtension(file.FileName).ToLower();

        //     if (!allowedExtensions.Contains(fileExtension))
        //     {
        //         return BadRequest("Invalid file format. Supported formats are .xlsx, .xlsm, .xltx, .xltm");
        //     }

        //     try
        //     {
        //         var filePath = Path.ChangeExtension(Path.GetTempFileName(), ".xlsx");

        //         using (var stream = new FileStream(filePath, FileMode.Create))
        //         {
        //             file.CopyTo(stream);
        //         }

        //         List<ExcelDataRow> excelDataRowList = _excelService.ProcessUploadedFile(filePath);

        //         // Optional: Clean up temporary file
        //         System.IO.File.Delete(filePath);

        //         return Ok(excelDataRowList);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex}");
        //     }
        // }

        [HttpPost("upload-excel")]
public IActionResult UploadFile(IFormFile file)
{
    if (file == null || file.Length == 0)
        return BadRequest("Invalid file");

    var allowedExtensions = new[] { ".xlsx", ".xlsm", ".xltx", ".xltm" };
    var fileExtension = Path.GetExtension(file.FileName).ToLower();

    if (!allowedExtensions.Contains(fileExtension))
    {
        return BadRequest("Invalid file format. Supported formats are .xlsx, .xlsm, .xltx, .xltm");
    }

    try
    {
        var filePath = Path.ChangeExtension(Path.GetTempFileName(), ".xlsx");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        Dictionary<string, List<ExcelDataRow>> excelDataDictionary = _excelService.ProcessUploadedFile(filePath);

        // Optional: Clean up temporary file
        System.IO.File.Delete(filePath);

        return Ok(excelDataDictionary);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex}");
    }
}

[HttpPost("save-excel-data")]
public async Task<ActionResult> SaveExcelData([FromForm] ExcelDataDto dto) 
{
    try
    {

        string publicId = GeneratePublicId();
        
        var excel = new ExcelData
        {
            PublicId = publicId,
            Title = dto.Title,
            DateUploaded = dto.DateUploaded,
          
        };

         // Process files
        if (dto.ExcelFile != null)
        {
            foreach (var file in dto.ExcelFile)
            {
                var uploadedFile = await _mediaService.AddExcelFileAsync(file);

                if (uploadedFile.Error != null)
                {
                     Console.WriteLine(uploadedFile.Error.Message);
                     return Ok(new { message = "Error in Uploading File:" + uploadedFile.Error.Message });
                }

                var fileEntity = new ExcelFile
                {
                    Url = uploadedFile.SecureUrl.AbsoluteUri,
                    PublicId = uploadedFile.PublicId,
                };
                excel.Files.Add(fileEntity);
            }
        }

        _excelService.Add(excel);
        await _excelService.SaveAllAsync();

         return Ok(new { message = "Excel uploaded successfully" });
  
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Internal server error: " + ex.Message);
    }
}
public int GetNextPublicId()
{
   int nextId = _context.ExcelData.Any() ? _context.ExcelData.Max(e => e.Id) + 1 : 0001;
   return nextId;
}

public string GeneratePublicId()
{
    int nextId = GetNextPublicId();
    return "TMFI" + nextId.ToString("D4");
}

        // [HttpPost("save-excel-data")]
        // public IActionResult SaveExcelData([FromBody] ExcelDataDto excelDataDto)
        // {
        //     try
        //     {
        //         _excelService.SaveExcelDataToDatabase(excelDataDto);
        //           return Ok(new { message = "Excel uploaded successfully" });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex}");
        //     }
        // }


[HttpGet("get-exceldata")]
public async Task<ActionResult<IEnumerable<ExcelDataDto>>> GetExcelData()
{
    try
    {
        // Retrieve the list of documentations from your repository
        var data = await _excelService.GetExcelDataAsync(); // Adjust this based on your repository method

        return Ok(data);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Internal server error: " + ex.Message);
    }
}

// [HttpGet("get-exceldata/{publicId}")]
// public async Task<ActionResult<GetExcelDto>> GetDocumentation(string publicId)
// {
//     try
//     {
//         var dataDetails = await _excelService.GetExcelDataByPublicIdAsync(publicId);

//         if (dataDetails != null)
//         {
//             var dto = _mapper.Map<GetExcelDto>(dataDetails);
//             return Ok(dto);
//         }

//         return NotFound(); // Kung walang nahanap na ExcelData.
//     }
//     catch (Exception ex)
//     {
//         return StatusCode(500, "Internal server error: " + ex.Message);
//     }
// }


[HttpGet("get-exceldata/{publicId}")]
public async Task<ActionResult<IEnumerable<ExcelDataDto>>> GetDocumentation(string publicId)
{
    try
    {
        var dataDetails = await _excelService.GetExcelDataByPublicIdAsync(publicId); 

        return Ok(dataDetails);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Internal server error: " + ex.Message);
    }
}

// [HttpGet("get-excel-file/{publicId}")]
// public async Task<IActionResult> GetExcelFile(string publicId)
// {
//     var excelData = await _context.ExcelData
//         .FirstOrDefaultAsync(ed => ed.PublicId == publicId);

//     if (excelData == null)
//     {
//         return NotFound();
//     }

//     var fileBytes = await _excelService.GetExcelFileBytesAsync(excelData);

//     return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelData.Title);
// }




    }
}

