using System;
using System.Collections.Generic;

namespace WikiDiscordNotifier
{
    public class SimpleLogger
    {
        public List<string> _logs = new List<string>();
        private bool _silent;

        public SimpleLogger(bool silent)
        {
            _silent = silent;
        }

        public void AddLog(string log)
        {
            if (_silent) return;
            var logtoAdd = String.Format("[{0}] {1}", DateTime.Now.ToLocalTime().ToString("o"), log);
            Console.WriteLine(logtoAdd);
            _logs.Add(logtoAdd);
        }
        
        public IEnumerable<string> GetLogs()
        {
            return _logs;
        }
    }
}
   
