using System;
using System.Collections.Generic;

namespace PolitiLog
{
    class SimpleLogger
    {
        public List<string> _logs = new List<string>();
        private bool _realTime;

        public SimpleLogger(bool realtime)
        {
            _realTime = realtime;
        }

        public void AddLog(string log)
        {
            var logtoAdd = String.Format("[{0}] {1}", DateTime.Now.ToUniversalTime().ToString("o"), log);
            if (_realTime)
                Console.WriteLine(logtoAdd);
            _logs.Add(logtoAdd);
        }

        public void WriteLogsToConsole()
        {
            foreach (var log in _logs)
                Console.WriteLine(log);
        }
        
        public IEnumerable<string> GetLogs()
        {
            return _logs;
        }
    }
}
   
