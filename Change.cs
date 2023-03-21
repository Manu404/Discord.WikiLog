using Newtonsoft.Json.Linq;
using System;

namespace WebHookTest
{
    public class Change
    {
        public string User { get; private set; }
        public string Title { get; private set; }
        public string Comment { get; private set; }
        public DateTime Date { get; private set; }
        public string Type { get; private set; }
        public string Page { get; private set; }
        public int RevId { get; private set; }
        public int OldRevId { get; private set; }

        public Change(JToken data)
        {
            User = data.Value<string>("user");
            Title = data.Value<string>("title");
            Comment = data.Value<string>("comment");
            Page = data.Value<string>("page");
            Type = data.Value<string>("type");
            Date = DateTime.Parse(data.Value<string>("timestamp"));
            RevId = data.Value<int>("revid");
            OldRevId = data.Value<int>("old_revid");
        }
    }
}
   
