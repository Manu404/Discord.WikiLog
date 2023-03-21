using CommandLine;

namespace PolitiLog
{
    partial class Program
    {
        public class CommandLineOptions
        {
            [Option("webhook", Required = true, HelpText = "Webhook url used to post notifications, provided by your discord server.")]
            public string WebHook { get; set; }

            [Option("wiki", Required = true, HelpText = "Url of the wiki to monitor. eg: https://mywiki.com/")]
            public string Wiki { get; set; }

            [Option("log-file", Required = false, Default = "./log", HelpText = "File to write logs of the application")]
            public string LogFile { get; set; }

            [Option('n', "no-log", Required = false, Default = false, HelpText = "Disable writting logs to the log file")]
            public bool NoLog { get; set; }

            [Option('l', "limit", Required = false, Default = 100, HelpText = "Number of changes requested by query. Default 100.")]
            public int Limit { get; set; }

            [Option('s', "silent", Required = false, HelpText = "Produce no output in the console.")]
            public bool Silent { get; set; }
        }
    }
}
   
