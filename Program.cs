using System;

namespace WebHookTest
{

    partial class Program
    {
        static void Main(string[] args)
        {
            SimpleLogger logger = new SimpleLogger();
            logger.AddLog("-----------------");
            logger.AddLog("Start application");

            FileHelper fileHelper = new FileHelper(logger, "./last_change", "./logs");

            DateTime lastChangeDate = fileHelper.GetLastChangeDateTime();
            logger.AddLog(String.Format("Last change dates from {0}", lastChangeDate.ToUniversalTime().ToString("o")));

            ChangeNotifier changeNotfier = new ChangeNotifier(logger);

            DateTime newestDate = changeNotfier.SendRevisionSinceLastRevision(lastChangeDate).ToUniversalTime();

            logger.AddLog("Stop Application");
            logger.WriteLogsToConsole();
            fileHelper.SaveNewDate(newestDate);
            fileHelper.SaveLogs();
        }


    }
}
   
