using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE_cordova
{
    class Project : Session
    {
        public Project()
        {
            createTime = UnixTimeNow();
            startTime = UnixTimeNow();
        }
        public long startTime;
        public long endTime;
        private string name;
        private string path;
        private string buildPath;
        private long createTime;
        private string createArgs;
        private long buildTime;
        private string buildTimeString;
        //private string cfgFile;
        private string createTimeString;
        private string cdpPath;
        private string reverseDomain;
        private string username;

        public void makeNewProject(string projectName, string projectPath)
        {
            path = projectPath;
            name = projectName;
            createTime = UnixTimeNow(); //Inherited from MainWindow
            buildTime = -1;
            createTimeString = createTime.ToString();
            buildTimeString = buildTime.ToString();
            cdpPath = projectPath + "\\" + projectName + "\\" + projectName + ".cdp";
            username = Environment.UserName;
            reverseDomain = "com." + username + "." + projectName;
            buildPath = path + "\\" + name + "\\";
            //create project and .cdp file
            //string toCMD1 = "cd " + projectPath;
            //submitCMD("cmd.exe", toCMD1);
            string toCMD2 = "create " + projectName + " " + reverseDomain + " " + projectName;
            createArgs = toCMD2;
            }
        public void loadProject(string n, string p, string c, string b, string cdp)
        {
            name = n;
            path = p;
            buildPath = path + "\\" + name + "\\";
            createTimeString = c;
            createTime = Convert.ToInt64(c);
            buildTimeString = b;
            buildTime = Convert.ToInt64(b);
            cdpPath = cdp;

        }
        public void buildUpdate()
        {
            buildTime = UnixTimeNow();
            buildTimeString = buildTime.ToString();
        }
        public string getName()
        {
            return name;
        }
        public string getPath()
        {
            return path;
        }
        public string getCdpPath()
        {
            return cdpPath;
        }
        public string getCreateArgs()
        {
            return createArgs;
        }
        public string getCreateTimeStr()
        {
            return createTimeString;
        }
        public string getBuildTimeStr()
        {
            return buildTimeString;
        }
        public string getBuildPath()
        {
            return buildPath;
        }

        ~Project()
        {
            endTime = UnixTimeNow();
        }
    }
}
