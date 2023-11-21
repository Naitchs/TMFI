using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Enums;

namespace API.Entities
{
    public class AppLogs
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }

        public string Error { get; set; }
        public LogTypeEnum LogType { get; set; }
    }
}