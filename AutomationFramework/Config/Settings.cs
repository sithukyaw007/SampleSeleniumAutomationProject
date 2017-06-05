using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutomationFramework.Base.Browser;

namespace AutomationFramework.Config
{
    public class Settings
    {       
        public static string ApplicationUrl { get; set; }        
        public static string TestType { get; set; }
        public static string AQATestDB { get; set; }
        public static string AppDB { get; set; }
        public static BrowserType BrowserType { get; set; }
        public static string Build { get; set; }
        public static string IsLog { get; set; }
        public static string LogPath { get; set; }
        public static string LogFileName { get; set; }
    }
}
