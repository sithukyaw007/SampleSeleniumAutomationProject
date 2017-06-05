using OpenQA.Selenium;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace AutomationFramework.Helpers
{
    public class LogHelper
    {
        private static string _logFileTimeStamp = string.Format("{0:yyyymmddhhmmss}", DateTime.Now);
        private static StreamWriter _streamWriter = null;
        private static readonly Dictionary<Guid, string> _knownImageFormats =
            (from p in typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public)
             where p.PropertyType == typeof(ImageFormat)
             let value = (ImageFormat)p.GetValue(null, null)
             select new { Guid = value.Guid, Name = value.ToString() })
            .ToDictionary(p => p.Guid, p => p.Name);

        /// <summary>
        /// This method Creates a log file on one of the specified system directories
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="logFileName"></param>
        public static void CreateLogFile(string logFileName, string dir = null)
        {
            if (dir == null)
            {
                dir = System.IO.Path.GetFullPath(@"..\..\") + @"AutoTestRunLogs\";
            }
            logFileName = logFileName + "_" + _logFileTimeStamp;
            if (Directory.Exists(dir))
            {
                _streamWriter = File.AppendText(dir + logFileName + ".log");
            }
            else
            {
                Directory.CreateDirectory(dir);
                _streamWriter = File.AppendText(dir + logFileName + ".log");
            }
        }

        /// <summary>
        /// This method Wites a text into the Log file
        /// </summary>
        public static void WriteTextToLog(string logMsg)
        {
            try
            {
                if (_streamWriter != null)
                {
                    _streamWriter.Write("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                    _streamWriter.WriteLine("   {0}", logMsg);
                    _streamWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in WriteTextToLog : " + ex.Message);
            }
        }

        public static void CloseLogFile()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Close();
            }
        }

        /// <summary>
        /// Get the name of the Image format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>

        public static string GetImageFormatName(ImageFormat format)
        {
            string name;
            if (_knownImageFormats.TryGetValue(format.Guid, out name))
                return name;
            return null;
        }

        /// <summary>
        /// Takes the screenshot of the browser object
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="fileName"></param>
        /// <param name="saveLocation"></param>
        /// <param name="imageFormat"></param>
        public static void TakeScreenShot(IWebDriver driver, string fileName, string saveLocation, ImageFormat imageFormat)
        {
            try
            {
                string screenShotFormat = GetImageFormatName(imageFormat);
                var location = saveLocation + "\\" + fileName + "." + screenShotFormat.ToString().Trim().ToLower();
                var ssdriver = driver as ITakesScreenshot;
                var screenshot = ssdriver.GetScreenshot();
                screenshot.SaveAsFile(location, imageFormat);
            }
            catch (Exception ex)
            {
                WriteTextToLog("Excaption,Failed to Capture an image : " + ex.Message);                
            }
        }

    }

}
