using Discord;
using Discord.Webhook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace WikiDiscordNotifier
{
    class ChangeNotifier
    {
        private EmbedBuilderHelper _builder;
        private SimpleLogger _logger;
        private l18n _localization;

        private string _webhookUrl;
        private string _api;
        private string _wiki;
        private string _domain;
        private int _queryLimit;

        public ChangeNotifier(SimpleLogger logger, string webhookUrl, string domain, string api, string wiki, int queryLimit, l18n localization)
        {
            _logger = logger;
            _localization = localization;

            _webhookUrl = webhookUrl;
            _api = api;
            _wiki = wiki;
            _domain = domain;
            _queryLimit = queryLimit;

            _builder = new EmbedBuilderHelper(_domain, _wiki, _logger, localization);
        }

        public DateTime SendRevisionSinceLastRevision(DateTime lastChange)
        {
            var changes = GetChangesFromApi(lastChange).ToList();

            SendToWebHook(BuildEmbeds(changes));

            _logger.AddLog(changes.Count + " " + "new changes to publish");

            // if no changes since last check, return
            if (changes.Count == 0)
                return lastChange;

            return changes.Last().Date;
        }

        private IEnumerable<Embed> BuildEmbeds(List<Change> changes)
        {
            List<Embed> embeds = new List<Embed>();

            // build embeds
            foreach (var change in changes)
                embeds.Add(_builder.CreateEmbed(change));

            // remove errors
            embeds.RemoveAll(o => o == null);

            return embeds;
        }

        private IEnumerable<Change> GetChangesFromApi(DateTime lastChange)
        {
            List<Change> revisions = new List<Change>();
            try
            {
                using (WebClient client = new WebClient())
                {
                    // Query api and parse json
                    string queryUrl = HandleDoubleDash($"{_domain}/{_api}?action=query&list=recentchanges&rcprop=ids|title|user|comment|timestamp&rclimit={_queryLimit}&format=json");
                    string json = client.DownloadString(queryUrl);
                    JObject jsonObject = JObject.Parse(json);

                    // for each revisions, only keep edit, new and log type of revision
                    foreach (var change in jsonObject["query"]["recentchanges"])
                        if (change.Value<string>("type") == "edit" || change.Value<string>("type") == "new" || change.Value<string>("type") == "log")
                            if (change.Value<DateTime>("timestamp").ToUniversalTime() > lastChange.ToUniversalTime())
                                revisions.Add(new Change(change, _localization));

                    // sort by date
                    revisions.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
                }
            }
            catch (Exception ex)
            {
                _logger.AddLog(ex.Message);
            }
            return revisions;
        }

        private void SendToWebHook(IEnumerable<Embed> embedsList)
        {
            foreach (var embed in embedsList)
            {
                try
                {
                    var client = new DiscordWebhookClient(_webhookUrl);
                    client.SendMessageAsync(text: _localization.WebhookMessage, embeds: new[] { embed }).Wait();
                }
                catch (Exception e){
                    _logger.AddLog(e.Message);
                }
            }
        }


        private string HandleDoubleDash(string url) => url.Replace("//", "/").Replace("https:/", "https://").Replace("http:/", "http://");
    }
}
   
