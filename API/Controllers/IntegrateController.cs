using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using static API.DTOs.HrModelsDto;
using static API.Entities.ExcelModels;
using static API.Entities.HrModels;


namespace API.Controllers
{
    // [Authorize]
    public class IntegrateController : BaseApiController
    {
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly LogService _logService;
        private readonly IHrService _hrService;
        private readonly DataContext _context;
        private readonly IMediaService _mediaService;
        public IntegrateController(IExcelService excelService,
                                   DataContext context,
                                   IMediaService mediaService,
                                   IMapper mapper,
                                   LogService logService,
                                   IHrService hrService)
        {
            _mediaService = mediaService;
            _context = context;
            _excelService = excelService;
            _mapper = mapper;
            _logService = logService;
            _hrService = hrService;
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
                _logService.AddErrorLogs(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("save-excel-data")]
        public async Task<ActionResult> SaveExcelData([FromForm] ExcelDataDto dto)
        {
            try
            {
                List<ExcelData> excelDataList = new List<ExcelData>();

                string publicId = GeneratePublicId();

                var excel = new ExcelData
                {
                    PublicId = publicId,
                    Title = dto.Title,
                    DateUploaded = dto.DateUploaded,
                    DateFrom = dto.DateFrom,
                    DateTo = dto.DateTo
                };

                // Process files
                if (dto.ExcelFile != null)
                {
                    foreach (var file in dto.ExcelFile)
                    {
                        try
                        {

                            var filename = Guid.NewGuid().ToString();
                            var fileExtension = System.IO.Path.GetExtension(file.FileName);
                            // Save the file to wwwroot/uploads
                            if (await _mediaService.AddExcelFileAsync(file, filename, fileExtension))
                            {

                                var fileEntity = new ExcelFile
                                {
                                    Url = "wwwroot/uploads",
                                    PublicId = filename + fileExtension
                                };

                                // Add the file entity to the list
                                excel.Files.Add(fileEntity);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error saving file: {ex}");
                            // Handle the error as needed
                            return StatusCode(500, "Error saving file");
                        }
                    }
                }

                // Add the ExcelData entity to the database
                _excelService.Add(excel);
                await _excelService.SaveAllAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                Console.WriteLine($"Internal server error: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            var filePath = _mediaService.GetFilePath(fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Set the appropriate content type for Excel files
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // Return the file as a FileStreamResult
            return File(System.IO.File.OpenRead(filePath), contentType, fileName);
        }




        // [HttpPost("save-excel-data")]
        // public async Task<ActionResult> SaveExcelData([FromForm] ExcelDataDto dto)
        // {
        //     try
        //     {

        //         string publicId = GeneratePublicId();

        //         var excel = new ExcelData
        //         {
        //             PublicId = publicId,
        //             Title = dto.Title,
        //             DateUploaded = dto.DateUploaded,
        //             DateFrom = dto.DateFrom,
        //             DateTo = dto.DateTo

        //         };

        //         // Process files
        //         if (dto.ExcelFile != null)
        //         {
        //             foreach (var file in dto.ExcelFile)
        //             {
        //                 var uploadedFile = await _mediaService.AddExcelFileAsync(file);

        //                 if (uploadedFile.Error != null)
        //                 {
        //                     Console.WriteLine(uploadedFile.Error.Message);
        //                     return Ok(new { message = "Error in Uploading File:" + uploadedFile.Error.Message });
        //                 }

        //                 var fileEntity = new ExcelFile
        //                 {
        //                     Url = uploadedFile.SecureUrl.AbsoluteUri,
        //                     PublicId = uploadedFile.PublicId,
        //                 };
        //                 excel.Files.Add(fileEntity);
        //             }
        //         }

        //         _excelService.Add(excel);
        //         await _excelService.SaveAllAsync();

        //         return Ok(new { message = "Excel uploaded successfully" });

        //     }
        //     catch (Exception ex)
        //     {
        //         _logService.AddErrorLogs(ex.ToString());
        //         return StatusCode(500, "Internal server error: " + ex.Message);
        //     }
        // }
        public int GetNextPublicId()
        {
            int nextId = _context.ExcelData.Any() ? _context.ExcelData.Max(e => e.Id) + 5001 : 5001;
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
        public async Task<ActionResult<IEnumerable<GetExcelDto>>> GetExcelData()
        {
            try
            {
                // Retrieve the list of documentations from your repository
                var data = await _excelService.GetExcelDataAsync(); // Adjust this based on your repository method

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
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
                _logService.AddErrorLogs(ex.ToString());
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("delete-data/{id}")]
        public IActionResult DeleteData(int id)
        {
            _excelService.DeleteData(id);

            return Ok(_excelService.GetExcelDataAsync());
        }

        [HttpPost("save-certificate")]
        public async Task<ActionResult> SaveCertFile([FromForm] UploadCertDto dto)
        {
            try
            {
                List<Certificates> certList = new List<Certificates>();

                var cert = new Certificates
                {
                    Title = dto.Title,
                    UploadDate = dto.UploadDate,
                    CertType = dto.CertType
                };

                // Process files
                if (dto.CertFile != null)
                {
                    foreach (var file in dto.CertFile)
                    {
                        try
                        {

                            var filename = Guid.NewGuid().ToString();
                            var fileExtension = System.IO.Path.GetExtension(file.FileName);
                            // Save the file to wwwroot/uploads
                            if (await _mediaService.AddCertFileAsync(file, filename, fileExtension))
                            {

                                var fileEntity = new HrFiles
                                {
                                    FilePath = "wwwroot/uploads/hr/cert",
                                    FileName = filename + fileExtension,
                                    FileType = fileExtension
                                };

                                // Add the file entity to the list
                                cert.CertFiles.Add(fileEntity);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error saving file: {ex}");
                            // Handle the error as needed
                            return StatusCode(500, "Error saving file");
                        }
                    }
                }

                // Add the ExcelData entity to the database
                _excelService.AddCert(cert);
                await _excelService.SaveAllAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                Console.WriteLine($"Internal server error: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("get-cert/{id}")]
        public async Task<ActionResult<IEnumerable<Certificates>>> GetCert(int id)
        {
            try
            {
                var certDetails = await _hrService.GetCertByIdAsync(id);

                return Ok(certDetails);
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("get-all-cert")]
        public async Task<ActionResult<IEnumerable<GetCertDto>>> GetAllCert()
        {
            try
            {
                // Retrieve the list of documentations from your repository
                var certs = await _hrService.GetAllCertAsync(); // Adjust this based on your repository method

                return Ok(certs);
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("get-all-board-resolution-cert")]
        public async Task<ActionResult<IEnumerable<GetCertDto>>> GetAllBoardResolutionCerts()
        {
            try
            {
                var certs = await _hrService.GetAllBoardResolutionCertsAsync();
                return Ok(certs);
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("get-all-retirement-fund-cert")]
        public async Task<ActionResult<IEnumerable<GetCertDto>>> GetAllRetirementFundCerts()
        {
            try
            {
                var certs = await _hrService.GetAllRetirementFundCertsAsync();
                return Ok(certs);
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("get-all-employment-cert")]
        public async Task<ActionResult<IEnumerable<GetCertDto>>> GetAllEmploymentCerts()
        {
            try
            {
                var certs = await _hrService.GetAllEmploymentCertsAsync();
                return Ok(certs);
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }




        [HttpPost("add-cert/{certId}")]
        public async Task<ActionResult<List<HrFileDto>>> AddCertFiles(int certId, List<IFormFile> certFiles)
        {
            try
            {
                var existingCert = await _hrService.FindCertByIdAsync(certId);

                if (existingCert == null)
                {
                    return NotFound("Certificate not found");
                }

                foreach (var file in certFiles)
                {
                    var filename = Guid.NewGuid().ToString();
                    var fileExtension = System.IO.Path.GetExtension(file.FileName);

                    // Save the file to wwwroot/uploads/hr/cert
                    if (await _mediaService.AddCertFileAsync(file, filename, fileExtension))
                    {
                        var fileEntity = new HrFiles
                        {
                            FilePath = "wwwroot/uploads/hr/cert",
                            FileName = filename + fileExtension,
                            FileType = "certificate",
                            CertId = certId  // Assigning the provided 'id' to the CertId property
                        };

                        existingCert.CertFiles.Add(fileEntity);

                    }
                }

                _hrService.UpdateCert(existingCert);
                await _hrService.SaveAllAsync();
                return Ok();


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding certificate files: {ex}");
                // Handle the error as needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete-cert-file/{certId}")]
        public async Task<ActionResult> DeleteCertFile(int certId, string fileName)
        {
            // Fetch all HrFiles by certId
            var hrFiles = await _hrService.GetHrFilesByCertIdAsync(certId);

            // Find the specific file by its FileName
            var hrFileToDelete = hrFiles.FirstOrDefault(cf => cf.FileName == fileName);

            if (hrFileToDelete == null)
                return NotFound($"Certificate file with name {fileName} not found in the certificate.");

            try
            {
                // Delete the certificate file from the file system using its filename
                _mediaService.DeleteCertFile(hrFileToDelete.FileName);

                // Remove the reference to the file (HrFiles) from the database
                _context.HrFiles.Remove(hrFileToDelete);

                // Save changes to the database
                if (await _hrService.SaveAllAsync())
                    return Ok($"Certificate file with name {fileName} deleted successfully.");

                return BadRequest("Problem saving changes to the database.");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Problem deleting certificate file.");
            }
        }

        [HttpDelete("delete-certificate/{certId}")]
        public async Task<ActionResult> DeleteCertificate(int certId)
        {
            var cert = await _hrService.GetCertByIdAsync(certId);

            if (cert == null)
                return NotFound($"Certificate with ID {certId} not found.");

            var hrFiles = await _hrService.GetHrFilesByCertIdAsync(certId);

            var hrFileToDelete = hrFiles.FirstOrDefault(cf => cf.CertId == certId);

            // if (hrFiles == null || !hrFiles.Any())
            //     return NotFound($"Certificate file with name not found in the certificate.");

            try
            {
                // Delete the physical files from wwwroot folder
                foreach (var hrFile in hrFiles)
                {
                    _mediaService.DeleteCertificate(hrFile.FileName);
                }

                // Delete the certificate entity
                _hrService.DeleteCert(cert);

                // Save changes to the database
                if (await _excelService.SaveAllAsync())
                    return Ok();

                return BadRequest("Problem saving changes to the database.");
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                Console.WriteLine($"Error deleting certificate: {ex}");
                return StatusCode(500, "Error deleting certificate");
            }
        }








        [HttpPost("save-memo")]
        public async Task<ActionResult> SaveMemoFile([FromForm] UploadMemoDto dto)
        {
            try
            {
                List<Memos> certList = new List<Memos>();

                string publicId = GeneratePublicId();

                var memo = new Memos
                {
                    Title = dto.Title,
                    UploadDate = dto.UploadDate,
                    MemoType = dto.MemoType
                };

                // Process files
                if (dto.MemoFile != null)
                {
                    foreach (var file in dto.MemoFile)
                    {
                        try
                        {

                            var filename = Guid.NewGuid().ToString();
                            var fileExtension = System.IO.Path.GetExtension(file.FileName);
                            // Save the file to wwwroot/uploads
                            if (await _mediaService.AddMemoFileAsync(file, filename, fileExtension))
                            {

                                var fileEntity = new HrFiles
                                {
                                    FilePath = "wwwroot/uploads/hr/memo",
                                    FileName = filename + fileExtension,
                                    FileType = fileExtension
                                };

                                // Add the file entity to the list
                                memo.MemoFiles.Add(fileEntity);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error saving file: {ex}");
                            // Handle the error as needed
                            return StatusCode(500, "Error saving file");
                        }
                    }
                }

                // Add the ExcelData entity to the database
                _excelService.AddMemo(memo);
                await _excelService.SaveAllAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                Console.WriteLine($"Internal server error: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-memo/{id}")]
        public async Task<ActionResult<IEnumerable<Memos>>> GetMemo(int id)
        {
            try
            {
                var memoDetails = await _hrService.GetMemoByIdAsync(id);

                return Ok(memoDetails);
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("get-all-memo")]
        public async Task<ActionResult<IEnumerable<GetMemoDto>>> GetAllMemo()
        {
            try
            {
                // Retrieve the list of documentations from your repository
                var memos = await _hrService.GetAllMemoAsync(); // Adjust this based on your repository method

                return Ok(memos);
            }
            catch (Exception ex)
            {
                _logService.AddErrorLogs(ex.ToString());
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("add-memo/{memoId}")]
        public async Task<ActionResult<List<HrFileDto>>> AddMemoFiles(int memoId, List<IFormFile> memoFiles)
        {
            try
            {
                var existingMemo = await _hrService.FindMemoByIdAsync(memoId);

                if (existingMemo == null)
                {
                    return NotFound("Certificate not found");
                }

                foreach (var file in memoFiles)
                {
                    var filename = Guid.NewGuid().ToString();
                    var fileExtension = System.IO.Path.GetExtension(file.FileName);

                    // Save the file to wwwroot/uploads/hr/cert
                    if (await _mediaService.AddMemoFileAsync(file, filename, fileExtension))
                    {
                        var fileEntity = new HrFiles
                        {
                            FilePath = "wwwroot/uploads/hr/memo",
                            FileName = filename + fileExtension,
                            FileType = fileExtension,
                            MemoId = memoId  // Assigning the provided 'id' to the CertId property
                        };

                        existingMemo.MemoFiles.Add(fileEntity);

                    }
                }

                _hrService.UpdateMemo(existingMemo);
                await _hrService.SaveAllAsync();
                return Ok();


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding certificate files: {ex}");
                // Handle the error as needed
                return StatusCode(500, "Internal server error");
            }
        }









    }
}

