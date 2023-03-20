using Discord;
using Discord.Webhook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebHookTest
{
    class Program
    {
        public class Change {
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

        static void Main(string[] args)
        {
            var p = new Program();
            p.SendEmbeds(p.GetEmbedsFromApiQuery());
        }

        private IEnumerable<Embed> GetEmbedsFromApiQuery()
        {
            List<Embed> embeds = new List<Embed>();
            try
            {
                using (WebClient client = new WebClient())
                {
                    List<Change> changes = new List<Change>();
                    string queryUrl = $"https://politiwiki.fr/w/api.php?action=query&list=recentchanges&rcprop=ids|title|user|comment|timestamp&rclimit=50&format=json";
                    string json = client.DownloadString(queryUrl);
                    JObject jsonObject = JObject.Parse(json);
                    foreach (var change in jsonObject["query"]["recentchanges"])
                        if (change.Value<string>("type") == "edit" || change.Value<string>("type") == "new" || change.Value<string>("type") == "log")
                            changes.Add(new Change(change));

                    changes.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

                    foreach (var change in changes)
                        embeds.Add(CreateEmbedSingleField(change, false));

                    embeds.RemoveAll(o => o == null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return embeds;
        }

        private Embed CreateEmbedSingleField(Change data, bool inline)
        {
            try
            {
                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.AppendLine(String.Format("**Contributeur·ice**: {0}", data.User));
                messageBuilder.AppendLine(String.Format("**Page**: {0}", String.Format("[{0}](https://politiwiki.fr/wiki/{1})", data.Title, data.Title.Replace(' ', '_'))));
                messageBuilder.AppendLine(String.Format("**Modification**: {0}", String.IsNullOrEmpty(data.Comment) ? "?" : data.Comment));

                if (data.Type == "edit")
                    messageBuilder.AppendLine(String.Format("**Diff**: {0}", String.Format("[Consulter]({0})", BuildDiffUrl(data.Title, data.RevId, data.OldRevId))));

                messageBuilder.AppendLine(String.Format("**Date**: {0}", data.Date.ToString("MM/dd/yyyy HH:mm:ss")));


                var footer = new EmbedFooterBuilder()
                        .WithText("Merci pour ton travail !")
                        .WithIconUrl("https://emmanuelistace.be/misc/heart2.png");

                var builder = new EmbedBuilder()
                        .WithAuthor(BuildTitle(data))
                        .WithFooter(footer)
                        .WithColor(data.Type == "log" ? Color.LightOrange : Color.Green);

                return builder.Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        private Embed CreateEmbed(Change data, bool inline)
        {
            try
            {
                var author = new EmbedAuthorBuilder()
                        .WithName(data.Type == "edit" ? "Edition d'une page" : "Création d'une page")
                        .WithIconUrl(data.Type == "edit" ? "https://emmanuelistace.be/misc/pencil.png" : "https://emmanuelistace.be/misc/add.png")
                        .WithUrl("https://emmanuelistace.be");
                var footer = new EmbedFooterBuilder()
                        .WithText("Merci pour ton travail !")
                        .WithIconUrl("https://emmanuelistace.be/politibot/heart.png");
                var user = new EmbedFieldBuilder()
                        .WithName("Contributeur·ice")
                        .WithValue(data.User)
                        .WithIsInline(inline);
                var url = new EmbedFieldBuilder()
                        .WithName("Page")
                        .WithValue(String.Format("[{0}](https://politiwiki.fr/wiki/{1})", data.Title, data.Title.Replace(' ', '_')))
                        .WithIsInline(inline);
                var message = new EmbedFieldBuilder()
                        .WithName("Modification")
                        .WithValue(String.IsNullOrEmpty(data.Title) ? "?" : data.Title)
                        .WithIsInline(inline);
                var date = new EmbedFieldBuilder()
                    .WithName("Date")
                    .WithValue(data.Date.ToString("MM/dd/yyyy HH:mm:ss"))
                    .WithIsInline(inline);

                var builder = new EmbedBuilder()
                        .AddField(user)
                        .AddField(message)
                        .AddField(url)
                        .WithAuthor(author)
                        .WithFooter(footer)
                        .WithColor(Color.Green);

                if (data.Type == "edit")
                {
                    var diff = new EmbedFieldBuilder();
                    if (data.Type == "edit")
                        diff.WithName("Diff")
                        .WithValue(String.Format("[Voir]({0})", BuildDiffUrl(data.Title, data.RevId, data.OldRevId)))
                        .WithIsInline(inline);
                    builder.AddField(diff);
                }

                builder.AddField(date);

                return builder.Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
            return String.Format("https://politiwiki.fr/w/index.php?title={0}&diff={1}&oldid={2}", paneName.Replace(' ', '_'), diff, oldid);
        }

        public void SendEmbeds(IEnumerable<Embed> embedsList)
        {           
            foreach(var embed in embedsList)
            using (var client = new DiscordWebhookClient("https://discord.com/api/webhooks/1086991179004006550/R5mvOdDG0qQHKsG57J7aLzVm2C0TzSU08-EETfYeRxaUNp03F_mdOVL_vlS_v3yEvzbw"))
            {
                client.SendMessageAsync(text: "Nouveau changement sur politiwiki !", embeds: new[] { embed }).Wait();
            }
        }
    }
}
   
