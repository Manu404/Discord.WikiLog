using CommandLine;
using System;

namespace PolitiLog
{
    partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<CommandLineOptions>(args)
                    .WithParsed<CommandLineOptions>(option =>
                    {
                        try
                        {
                            SimpleLogger logger = new SimpleLogger();
                            logger.AddLog("-----------------");
                            logger.AddLog("Start application");

                            FileHelper fileHelper = new FileHelper(logger, "./last_change", option.LogFile);

                            DateTime lastChangeDate = fileHelper.GetLastChangeDateTime();
                            logger.AddLog(String.Format("Last change dates from {0}", lastChangeDate.ToUniversalTime().ToString("o")));

                            ChangeNotifier changeNotfier = new ChangeNotifier(logger, option.WebHook, option.Wiki, option.Limit);

                            DateTime newestDate = changeNotfier.SendRevisionSinceLastRevision(lastChangeDate).ToUniversalTime();

                            logger.AddLog("Stop Application");

                            if (!option.Silent)
                                logger.WriteLogsToConsole();

                            fileHelper.SaveNewDate(newestDate);

                            if (!option.NoLog)
                                fileHelper.SaveLogs();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    });
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
    }
}
   
