using CommandLine;

namespace WikiDiscordNotifier
{
    partial class Program
    {
        public class CommandLineOptions
        {
            [Option("webhook", Required = true, HelpText = "Webhook url used to post notifications, provided by your discord server.")]
            public string WebHook { get; set; }

            [Option("wiki", Required = true, HelpText = "Url of the wiki to monitor. eg: https://mywiki.com/")]
            public string Wiki { get; set; }

            [Option("api", Required = true, HelpText = "Url of the api.php file. eg: https://mywiki.com/w/api.php")]
            public string Api { get; set; }

            [Option("language", Required = false, HelpText = "Language file")]
            public string Language { get; set; }

            [Option("log-file", Required = false, Default = "./log", HelpText = "File to write logs of the application")]
            public string LogFile { get; set; }

            [Option('l', "limit", Required = false, Default = 100, HelpText = "Number of changes requested by query. Default 100.")]
            public int Limit { get; set; }

            [Option('s', "silent", Required = false, HelpText = "Produce no output in the console.")]
            public bool Silent { get; set; }

            [Option("real-time-log", Required = false, Default = true, HelpText = "Output log to console in real time.")]
            public bool RealTimeLog { get; set; }
        }
    }
}
   
