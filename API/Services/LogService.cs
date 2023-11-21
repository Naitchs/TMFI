using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;

namespace API.Services
{
    public class LogService
    {
        private readonly DataContext _context;

        public LogService(DataContext context)
        {
            _context = context;
        }

        public void AddLoginLogs(AppUser user) {
            AppLogs logs = new AppLogs();
            logs.Date = DateTime.UtcNow;
            logs.UserId = user.Id.ToString();
            logs.LogType = Enums.LogTypeEnum.User;
            _context.Logs.Add(logs);
            _context.SaveChanges();
        }

        public void AddErrorLogs(string error) {
             AppLogs logs = new AppLogs();
            logs.Date = DateTime.UtcNow;
            logs.Error = error;
            logs.LogType = Enums.LogTypeEnum.Error;
    
            _context.Logs.Add(logs);
            _context.SaveChanges();

        }
    }
}