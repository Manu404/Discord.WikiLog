using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiscordWikiLog
{
    class FileHelper
    {
        private string _changeFileName;
        private string _logFileName;
        private SimpleLogger _logger;

        public FileHelper(SimpleLogger logger, string changeFileName, string logFileName)
        {
            _logger = logger;
            _changeFileName = changeFileName;
            _logFileName = logFileName;

            _logger.AddLog(String.Format("Read date from: {0}", changeFileName));
            _logger.AddLog(String.Format("Output logs to : {0}", logFileName));
        }

        public DateTime GetLastChangeDateTime()
        {
            try
            {
                return DateTime.Parse(File.ReadAllText(_changeFileName)).ToUniversalTime();
            }
            catch (Exception e)
            {
                _logger.AddLog(e.Message);
                return DateTime.Now;
            }
        }

        public void SaveLastChangeDateTime()
        {
            try
            {

            }
            catch (Exception e)
            {
                _logger.AddLog(e.Message);
            }
        }

        public void SaveLogs()
        {
            try
            {
                if (!File.Exists(_logFileName))
                    File.Create(_logFileName);
                File.AppendAllLines(_logFileName, _logger.GetLogs());
            }
            catch(Exception e)
            {
                _logger.AddLog(e.Message);
            }
        }

        public void SaveNewDate(DateTime newDate)
        {
            try
            {
                if (!File.Exists(_changeFileName))
                    File.Create(_changeFileName);
                File.WriteAllText(_changeFileName, newDate.ToString("O"));
            }
            catch (Exception e)
            {
                _logger.AddLog(e.Message);
            }
        }

        public void Savel10n(string filename, l18n loclaisation)
        {
            try
            {
                if (File.Exists(filename))
                    File.Delete(filename);

                File.WriteAllText(filename, JsonConvert.SerializeObject(loclaisation, Formatting.Indented));
            }
            catch (Exception e)
            {
                _logger.AddLog(e.Message);
            }
        }

        public l18n Loadl10n(string filename)
        {
            try
            {
                string json = File.ReadAllText(filename);
                return JsonConvert.DeserializeObject<l18n>(json);

            }
            catch (Exception e)
            {
                _logger.AddLog(e.Message);
                return new l18n();
            }
        }
    }
}
   
