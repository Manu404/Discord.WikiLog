using System;
using System.Collections.Generic;

namespace PolitiLog
{
    class SimpleLogger
    {
        public List<string> _logs = new List<string>();

        public void AddLog(string log)
        {
            _logs.Add(String.Format("[{0}] {1}", DateTime.Now.ToUniversalTime().ToString("o"), log));
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
   
