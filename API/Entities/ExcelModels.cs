using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class ExcelModels
    {
        public class ExcelDataRow
        {
            public int RowNumber { get; set; }
            public Dictionary<string, string> ColumnData { get; set; }
            public string SheetName { get; set; }
        }

        public class ExcelData
        {
            public int Id { get; set; }
            public string PublicId { get; set; }
            //  = Guid.NewGuid().ToString(); // Automatic unique ID
            public string Title { get; set; } // Title
            public DateTime DateUploaded { get; set; } // Date upload
            [MaxLength]
            // public string JsonData { get; set; } // I-save ang JSON representation dito
            // // public string WorksheetName { get; internal set; }
            // public string WorksheetName { get; set; }

            public DateTime DateFrom { get; set; } 
            public DateTime DateTo { get; set; }

            public ICollection<ExcelFile> Files { get; set; } = new List<ExcelFile>();
        }

        public class ExcelFile
        {
            public int Id { get; set; }
            public string Url { get; set; }
            public string PublicId { get; set; }

            // Add this property to establish the relationship
            public int ExcelDataId { get; set; }
            public ExcelData ExcelData { get; set; }
        }
    }
}