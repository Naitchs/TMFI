using System.Globalization;
using API.Data;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClosedXML.Excel;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static API.Entities.ExcelModels;

namespace API.Services
{
    public class ExcelService : IExcelService
    {

    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    private readonly Cloudinary _cloudinary;
    private readonly DataContext _context;
        public ExcelService (IMapper mapper, 
                          IMediaService mediaService, 
                          IOptions<CloudinarySettings> config,
                          DataContext context){
            
            var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
            _cloudinary = new Cloudinary(acc);
            _mediaService = mediaService;
           
            _mapper = mapper;
            _context = context;

        }

//  public List<ExcelDataRow> ProcessUploadedFile(string filePath)
// {
//     List<ExcelDataRow> excelDataRowList = new List<ExcelDataRow>();

//     try
//     {
//         using (var workbook = new XLWorkbook(filePath))
//         {
//             foreach (IXLWorksheet worksheet in workbook.Worksheets)
//             {
//                 IXLRange range = worksheet.RangeUsed();

//                 if (range != null)
//                 {
//                     var rowCount = range.RowCount();
//                     var colCount = range.ColumnCount();

//                     for (int row = 1; row <= rowCount; row++)
//                     {
//                         ExcelDataRow excelDataRow = new ExcelDataRow
//                         {
//                             RowNumber = row,
//                             ColumnData = new Dictionary<string, string>(),
//                             SheetName = worksheet.Name.ToString()
//                         };

//                         for (int col = 1; col <= colCount; col++)
//                         {
//                             string columnName = worksheet.Cell(1, col).CachedValue.ToString();
//                             string cellValue = worksheet.Cell(row, col).CachedValue.ToString();

//                             if (!excelDataRow.ColumnData.ContainsKey(columnName))
//                             {
//                                 // Suriin kung maari maging DateTime
//                                 if (DateTime.TryParse(cellValue, out DateTime dateValue))
//                                 {
//                                     // Maaring i-convert sa DateTime, ito ay i-convert muna sa string
//                                     excelDataRow.ColumnData.Add(columnName, dateValue.ToString());
//                                 }
//                                 else
//                                 {
//                                     // Hindi maaring i-convert sa DateTime, ito ay i-include as is
//                                     excelDataRow.ColumnData.Add(columnName, cellValue);
//                                 }
//                             }
//                         }

//                         excelDataRowList.Add(excelDataRow);
//                     }
//                 }
//             }
//         }
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine(ex.Message);
//         Console.WriteLine(ex.StackTrace);
//     }

//     return excelDataRowList;
// }

public Dictionary<string, List<ExcelDataRow>> ProcessUploadedFile(string filePath)
{
    Dictionary<string, List<ExcelDataRow>> sheetData = new Dictionary<string, List<ExcelDataRow>>();

    try
    {
        using (var workbook = new XLWorkbook(filePath))
        {
            foreach (IXLWorksheet worksheet in workbook.Worksheets)
            {
                IXLRange range = worksheet.RangeUsed();

                if (range != null)
                {
                    var rowCount = range.RowCount();
                    var colCount = range.ColumnCount();

                    List<ExcelDataRow> excelDataRowList = new List<ExcelDataRow>();

                    for (int row = 1; row <= rowCount; row++)
                    {
                        ExcelDataRow excelDataRow = new ExcelDataRow
                        {
                            RowNumber = row,
                            ColumnData = new Dictionary<string, string>(),
                            SheetName = worksheet.Name.ToString()
                        };

                        for (int col = 1; col <= colCount; col++)
                        {
                            string columnName = worksheet.Cell(1, col).CachedValue.ToString();
                            string cellValue = worksheet.Cell(row, col).CachedValue.ToString();

                            if (!excelDataRow.ColumnData.ContainsKey(columnName))
                            {
                                excelDataRow.ColumnData.Add(columnName, cellValue);
                            }
                        }

                        excelDataRowList.Add(excelDataRow);
                    }

                    sheetData.Add(worksheet.Name.ToString(), excelDataRowList);
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
    }

    return sheetData;
}



// public void SaveExcelDataToDatabase(ExcelDataDto excelDataDto)
// {
//     try
//     {
//         foreach (var sheetName in excelDataDto.SheetData.Keys)
//         {
//             var excelDataRowList = excelDataDto.SheetData[sheetName];
//             string jsonData = JsonConvert.SerializeObject(excelDataRowList);

//             string publicId = GeneratePublicId();

//             var excelData = new ExcelData
//             {
//                 PublicId = publicId,
//                 Title = excelDataDto.Title,
//                 DateUploaded = excelDataDto.DateUploaded,
//                 JsonData = jsonData,
//                 WorksheetName = sheetName // Ito ang pangalan ng worksheet
//             };

//             _context.ExcelData.Add(excelData);
//         }

//         _context.SaveChanges();

//         Console.WriteLine("Data saved successfully");
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Internal server error: {ex}");
//     }
// }


// public void SaveExcelDataToDatabase(ExcelDataDto excelDataDto, Dictionary<string, List<ExcelDataRow>> sheetData)
// {
//     try
//     {
//         List<ExcelData> excelDataList = new List<ExcelData>();
//         string jsonData = JsonConvert.SerializeObject(sheetData); // Serialize ang sheetData

//         string publicId = GeneratePublicId();

//         excelDataList.Add(new ExcelData
//         {
//             PublicId = publicId,
//             Title = excelDataDto.Title,
//             DateUploaded = excelDataDto.DateUploaded,
//             JsonData = jsonData,
//             WorksheetName = "Combined" // Ito ay naka-depende sa iyong desisyon
//         });

//         _context.ExcelData.AddRange(excelDataList);
//         _context.SaveChanges();

//         Console.WriteLine("Data saved successfully");
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Internal server error: {ex}");
//     }
// }





// public void SaveExcelDataToDatabase(ExcelDataDto excelDataDto)
// {
//     try
//     {
//         List<ExcelData> excelDataList = new List<ExcelData>();
//         string jsonData = JsonConvert.SerializeObject(excelDataDto.ExcelDataRowList);
        
//          string publicId = GeneratePublicId();

//         excelDataList.Add(new ExcelData
//         {
//             PublicId = publicId,
//             Title = excelDataDto.Title,
//             DateUploaded = excelDataDto.DateUploaded,
//             JsonData = jsonData
//         });

//         _context.ExcelData.AddRange(excelDataList);
//         _context.SaveChanges();

//         Console.WriteLine("Data saved successfully");
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Internal server error: {ex}");
//     }
// }

public async void SaveExcelDataToDatabase(ExcelDataDto dto)
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
                }

                var fileEntity = new ExcelFile
                {
                    Url = uploadedFile.SecureUrl.AbsoluteUri,
                    PublicId = uploadedFile.PublicId,
                };
                excel.Files.Add(fileEntity);
            }
        }
       
        Add(excel);
        await SaveAllAsync();

        Console.WriteLine("Data saved successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Internal server error: {ex}");
    }
}


public int GetNextPublicId()
{
   int nextId = _context.ExcelData.Any() ? _context.ExcelData.Max(e => e.Id) + 1 : 4001;
   return nextId;
}

public string GeneratePublicId()
{
    int nextId = GetNextPublicId();
    return "TMFI" + nextId.ToString("D4");
}

        public async Task<IEnumerable<ExcelDataDto>> GetExcelDataAsync()
        {
            return await _context.ExcelData
              .ProjectTo<ExcelDataDto>(_mapper.ConfigurationProvider)
              .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        public void Add(ExcelData excelData)
        {
              _context.ExcelData.Add(excelData);
        }



  public async Task<GetExcelDto> GetExcelDataByPublicIdAsync(string publicId)
{
    var excelDataDto = await _context.ExcelData
        .Where(ed => ed.PublicId == publicId)
        .ProjectTo<GetExcelDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();

    return excelDataDto;
}





        // wag i delete sa get to
        //     public async Task<ExcelDataDto> GetExcelDataByPublicIdAsync(string publicId)
        //     {
        //             var excelData = await _context.ExcelData
        //     .Where(ed => ed.PublicId == publicId)
        //     .FirstOrDefaultAsync();

        // if (excelData != null)
        // {
        //     List<ExcelDataRow> excelDataRowList = JsonConvert.DeserializeObject<List<ExcelDataRow>>(excelData.JsonData);

        //     return new ExcelDataDto
        //     {
        //         PublicId = excelData.PublicId,
        //         Title = excelData.Title,
        //         DateUploaded = excelData.DateUploaded,
        //         ExcelDataRowList = excelDataRowList
        //     };
        // }

        // return null;
        //     }










        // public void ProcessUploadedFile(string filePath)
        // {
        //     try
        //     {
        //         using (var workbook = new XLWorkbook(filePath))
        //         {
        //             foreach (IXLWorksheet worksheet in workbook.Worksheets)
        //             {
        //                 // Process each sheet here
        //                 ProcessSheet(worksheet);
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {

        //         Console.WriteLine(ex.Message);
        //         Console.WriteLine(ex.StackTrace);
        //     }
        // }

        // private void ProcessSheet(IXLWorksheet worksheet)
        // {
        //     string sheetName = worksheet.Name;

        //     switch (sheetName)
        //     {
        //         case "Balance Sheet":
        //             ProcessBalanceSheet(worksheet);
        //             break;
        //         case "Profit & Loss by Class":
        //             ProcessProfitLossByClass(worksheet);
        //             break;
        //         case "P&L":
        //             ProcessPL(worksheet);
        //             break;
        //         case "Transaction Detail by Account":
        //             ProcessTransactionDetailByAccount(worksheet);
        //             break;
        //         default:
        //             // Iba pang mga sheet na hindi mo ini-expect
        //             // Dito ay maaaring i-throw ang isang error o itapon ang sheet na ito.
        //             break;
        //     }
        // }

        // private void ProcessBalanceSheet(IXLWorksheet worksheet)
        // {
        //       IXLRange range = worksheet.RangeUsed();

        //     // Check if the sheet contains any data
        //     if (range == null)
        //     {
        //         return;
        //     }

        //     var rowCount = range.RowCount();
        //     var colCount = range.ColumnCount();

        //     List<ExcelData> excelDataList = new List<ExcelData>();

        //     for (int row = 1; row <= rowCount; row++)
        //     {
        //         ExcelDataRow excelDataRow = new ExcelDataRow
        //         {
        //             RowNumber = row,
        //             ColumnData = new Dictionary<string, string>(),
        //             SheetName = "Balance Sheet"
        //         };

        //         for (int col = 1; col <= colCount; col++)
        //         {
        //             string columnName = worksheet.Cell(1, col).CachedValue.ToString(); // Gamitin ang CachedValue
        //             string cellValue = worksheet.Cell(row, col).CachedValue.ToString(); // Gamitin ang CachedValue

        //             if (!excelDataRow.ColumnData.ContainsKey(columnName))
        //             {
        //                 // Suriin kung maari maging DateTime
        //                 if (DateTime.TryParse(cellValue, out DateTime dateValue))
        //                 {
        //                     // Maaring i-convert sa DateTime, ito ay i-convert muna sa string
        //                     excelDataRow.ColumnData.Add(columnName, dateValue.ToString());
        //                 }
        //                 else
        //                 {
        //                     // Hindi maaring i-convert sa DateTime, ito ay i-include as is
        //                     excelDataRow.ColumnData.Add(columnName, cellValue);
        //                 }

        //             }
        //         }

        //         string jsonData = JsonConvert.SerializeObject(excelDataRow);
        //         excelDataList.Add(new ExcelData { JsonData = jsonData });
        //     }

        //     _context.ExcelData.AddRange(excelDataList);
        //     _context.SaveChanges();
        // }


        // private void ProcessProfitLossByClass(IXLWorksheet worksheet)
        // {
        //     IXLRange range = worksheet.RangeUsed();

        //     // Check if the sheet contains any data
        //     if (range == null)
        //     {
        //         return;
        //     }

        //     var rowCount = range.RowCount();
        //     var colCount = range.ColumnCount();

        //     List<ExcelData> excelDataList = new List<ExcelData>();

        //     for (int row = 1; row <= rowCount; row++)
        //     {
        //         ExcelDataRow excelDataRow = new ExcelDataRow
        //         {
        //             RowNumber = row,
        //             ColumnData = new Dictionary<string, string>(),
        //             SheetName = "Profit & Loss by Class"
        //         };

        //         for (int col = 1; col <= colCount; col++)
        //         {
        //             string columnName = worksheet.Cell(1, col).CachedValue.ToString();
        //             string cellValue = worksheet.Cell(row, col).CachedValue.ToString();

        //             if (!excelDataRow.ColumnData.ContainsKey(columnName))
        //             {
        //                 excelDataRow.ColumnData.Add(columnName, cellValue);
        //             }
        //         }

        //         string jsonData = JsonConvert.SerializeObject(excelDataRow);
        //         excelDataList.Add(new ExcelData { JsonData = jsonData });
        //     }

        //     _context.ExcelData.AddRange(excelDataList);
        //     _context.SaveChanges();
        // }



        // private void ProcessPL(IXLWorksheet worksheet)
        // {
        //   IXLRange range = worksheet.RangeUsed();

        //     // Check if the sheet contains any data
        //     if (range == null)
        //     {
        //         return;
        //     }

        //     var rowCount = range.RowCount();
        //     var colCount = range.ColumnCount();

        //     List<ExcelData> excelDataList = new List<ExcelData>();

        //     for (int row = 1; row <= rowCount; row++)
        //     {
        //         ExcelDataRow excelDataRow = new ExcelDataRow
        //         {
        //             RowNumber = row,
        //             ColumnData = new Dictionary<string, string>(),
        //             SheetName = "P&L"
        //         };

        //         for (int col = 1; col <= colCount; col++)
        //         {
        //             string columnName = worksheet.Cell(1, col).CachedValue.ToString();
        //             string cellValue = worksheet.Cell(row, col).CachedValue.ToString();

        //             if (!excelDataRow.ColumnData.ContainsKey(columnName))
        //             {
        //                 excelDataRow.ColumnData.Add(columnName, cellValue);
        //             }
        //         }

        //         string jsonData = JsonConvert.SerializeObject(excelDataRow);
        //         excelDataList.Add(new ExcelData { JsonData = jsonData });
        //     }

        //     _context.ExcelData.AddRange(excelDataList);
        //     _context.SaveChanges();
        // }

        // private void ProcessTransactionDetailByAccount(IXLWorksheet worksheet)
        // {
        //   IXLRange range = worksheet.RangeUsed();

        //     // Check if the sheet contains any data
        //     if (range == null)
        //     {
        //         return;
        //     }

        //     var rowCount = range.RowCount();
        //     var colCount = range.ColumnCount();

        //     List<ExcelData> excelDataList = new List<ExcelData>();

        //     for (int row = 1; row <= rowCount; row++)
        //     {
        //         ExcelDataRow excelDataRow = new ExcelDataRow
        //         {
        //             RowNumber = row,
        //             ColumnData = new Dictionary<string, string>(),
        //             SheetName = "Transaction Detail by Account"
        //         };

        //         for (int col = 1; col <= colCount; col++)
        //         {
        //             string columnName = worksheet.Cell(1, col).CachedValue.ToString();
        //             string cellValue = worksheet.Cell(row, col).CachedValue.ToString();

        //             if (!excelDataRow.ColumnData.ContainsKey(columnName))
        //             {
        //                 excelDataRow.ColumnData.Add(columnName, cellValue);
        //             }
        //         }

        //         string jsonData = JsonConvert.SerializeObject(excelDataRow);
        //         excelDataList.Add(new ExcelData { JsonData = jsonData });
        //     }

        //     _context.ExcelData.AddRange(excelDataList);
        //     _context.SaveChanges();
        // }



    }
}