using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static IDE_cordova.MainWindow;

namespace IDE_cordova
{
    public partial class Session
    {
        
        private long startTime;
        private long endTime;
        private string sessionPath;
        public Session()
        {
            startTime = UnixTimeNow();
            sessionPath = "c:\\Users\\" + Environment.UserName;
        }
        public string getSessionPath()
        {
            return sessionPath;
        }
        public void setSessionPath(string p)
        {
            sessionPath = p;
        }
        public long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        ~Session()
        {
            endTime = UnixTimeNow();
        }
    }

}
