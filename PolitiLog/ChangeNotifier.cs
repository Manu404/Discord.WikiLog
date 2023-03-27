using Discord;
using Discord.Webhook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PolitiLog
{
    class ChangeNotifier
    {
        private EmbedBuilderHelper _builder;
        private SimpleLogger _logger;
        private string _webhookUrl;
        private string _wikiUrl;
        private int _queryLimit;

        public ChangeNotifier(SimpleLogger logger, string webhookUrl, string wikiUrl, int queryLimit)
        {
            _logger = logger;
            _webhookUrl = webhookUrl;
            _wikiUrl = wikiUrl;
            _queryLimit = queryLimit;
            _builder = new EmbedBuilderHelper(_wikiUrl, _logger);
        }

        public DateTime SendRevisionSinceLastRevision(DateTime lastChange)
        {
            var changes = GetChangesFromApi(lastChange).ToList();

            SendToWebHook(BuildEmbeds(changes));

            _logger.AddLog(String.Format("{0} new changes to publish", changes.Count));

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
                    string queryUrl = String.Format("{0}/w/api.php?action=query&list=recentchanges&rcprop=ids|title|user|comment|timestamp&rclimit={1}&format=json", _wikiUrl, _queryLimit);
                    string json = client.DownloadString(queryUrl);
                    JObject jsonObject = JObject.Parse(json);

                    // for each revisions, only keep edit, new and log type of revision
                    foreach (var change in jsonObject["query"]["recentchanges"])
                        if (change.Value<string>("type") == "edit" || change.Value<string>("type") == "new" || change.Value<string>("type") == "log")
                            if (change.Value<DateTime>("timestamp").ToUniversalTime() > lastChange.ToUniversalTime())
                                revisions.Add(new Change(change));

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
                    client.SendMessageAsync(text: "Nouveau changement sur PolitiWiki !", embeds: new[] { embed }).Wait();
                }
                catch (Exception e){
                    _logger.AddLog(e.Message);
                }
            }
        }
    }
}
   
