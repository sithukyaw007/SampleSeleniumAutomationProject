using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using static AutomationFramework.Base.Browser;

namespace AutomationFramework.Config
{
    public class ConfigReader
    {
        public static void SetFrameworkSettings()
        {
            XPathItem appUrl;            
            XPathItem testType;
            XPathItem aqaTestDB;
            XPathItem appDB;
            XPathItem browserType;
            XPathItem build;
            XPathItem isLog;
            XPathItem logPath;
            XPathItem logFileName;

            string xmlFileName = Environment.CurrentDirectory.ToString() + "\\Config\\GlobalConfig.xml";

            FileStream stream = new FileStream(xmlFileName, FileMode.Open);
            XPathDocument documnet = new XPathDocument(stream);
            XPathNavigator navigator = documnet.CreateNavigator();

            appUrl = navigator.SelectSingleNode("AutomationFramework/RunSettings/AppUrl");
            testType = navigator.SelectSingleNode("AutomationFramework/RunSettings/TestType");
            aqaTestDB = navigator.SelectSingleNode("AutomationFramework/RunSettings/AQATestDB");
            appDB = navigator.SelectSingleNode("AutomationFramework/RunSettings/AppDB");
            browserType = navigator.SelectSingleNode("AutomationFramework/RunSettings/BrowserType");
            build = navigator.SelectSingleNode("AutomationFramework/RunSettings/Build");
            isLog = navigator.SelectSingleNode("AutomationFramework/RunSettings/IsLog");
            logPath = navigator.SelectSingleNode("AutomationFramework/RunSettings/LogPath");
            logFileName = navigator.SelectSingleNode("AutomationFramework/RunSettings/LogFileName");


            Settings.ApplicationUrl = appUrl.ToString();            
            Settings.TestType = testType.ToString();
            Settings.AQATestDB = aqaTestDB.ToString();
            Settings.AppDB = appDB.ToString();
            Settings.BrowserType = (BrowserType)Enum.Parse(typeof(BrowserType),browserType.ToString());
            //Settings.BrowserType = browserType.ToString();
            Settings.Build = build.ToString();
            Settings.IsLog = isLog.ToString();
            Settings.LogPath = logPath.ToString();
            Settings.LogFileName = logFileName.ToString();            

        }
    }
}
