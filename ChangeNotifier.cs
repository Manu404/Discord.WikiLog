using Discord;
using Discord.Webhook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PolitiLog
{
    class ChangeNotifier
    {
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
                embeds.Add(CreateEmbed(change, false));

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
                            if (DateTime.Parse(change.Value<string>("timestamp")) > lastChange)
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

        private Embed CreateEmbed(Change data, bool inline)
        {
            try
            {
                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.AppendLine(String.Format("**Contributeur·ice**: {0}", String.Format("[{0}](https://politiwiki.fr/wiki/Utilisateur:{1})", data.User, data.User)));
                messageBuilder.AppendLine(String.Format("**Page**: {0}", String.Format("[{0}]({1}/wiki/{2})", data.Title, _wikiUrl, data.Title.Replace(' ', '_'))));
                messageBuilder.AppendLine(String.Format("**Commentaire**: {0}", String.IsNullOrEmpty(data.Comment) ? "?" : data.Comment));

                if (data.Type == "edit")
                    messageBuilder.AppendLine(String.Format("**Modification**: {0}", String.Format("[Consulter]({0})", BuildDiffUrl(data.Title, data.RevId, data.OldRevId))));

                messageBuilder.AppendLine(String.Format("**Date**: {0}", data.Date.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss")));

                var content = new EmbedFieldBuilder()
                        .WithName("Description")
                        .WithValue(messageBuilder.ToString())
                        .WithIsInline(inline);

                var footer = new EmbedFooterBuilder()
                        .WithText("Merci pour ton travail !")
                        .WithIconUrl("https://emmanuelistace.be/misc/heart2.png");

                var builder = new EmbedBuilder()
                        .WithAuthor(BuildTitle(data))
                        .AddField(content)
                        .WithFooter(footer)
                        .WithColor(data.Type == "log" ? Color.LightOrange : Color.Green);

                return builder.Build();
            }
            catch (Exception ex)
            {
                _logger.AddLog(ex.Message);
                return null;
            }
        }

        private EmbedAuthorBuilder BuildTitle(Change data)
        {
            if (data.Type == "edit")
                return new EmbedAuthorBuilder()
                    .WithName("Edition d'une page")
                    .WithIconUrl("https://emmanuelistace.be/politibot/pencil.png");
            else if (data.Type == "new")
                return new EmbedAuthorBuilder()
                    .WithName("Création d'une page")
                    .WithIconUrl("https://emmanuelistace.be/politibot/add.png");
            else if (data.Type == "log")
                return new EmbedAuthorBuilder()
                    .WithName("Action de maintenance")
                    .WithIconUrl("https://emmanuelistace.be/politibot/settings.png");
            else
                return new EmbedAuthorBuilder();
        }

        private string BuildDiffUrl(string paneName, int diff, int oldid)
        {
            return String.Format("{0}/w/index.php?title={1}&diff={2}&oldid={3}", _wikiUrl, paneName.Replace(' ', '_'), diff, oldid);
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
   
