using Discord;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace PolitiLog
{
    class EmbedBuilderHelper
    {
        private SimpleLogger _logger;
        private string _wikiUrl;

        public EmbedBuilderHelper(string wikiUrl, SimpleLogger logger)
        {
            _logger = logger;
            _wikiUrl = wikiUrl; 
        }

        public Embed CreateEmbed(Change data)
        {
            try
            {
                if (data.IsNewUser())
                    return CreateNewUserEmbed(data);
                else if (data.IsFileUpload())
                    return CreateFileUploadEmbed(data);
                else
                    return CreateStandardEmbed(data);
            }
            catch (Exception ex)
            {
                _logger.AddLog(ex.Message);
                return null;
            }
        }

        private Embed CreateStandardEmbed(Change data)
        {
            try
            {
                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.AppendLine(GetContributor(data));
                messageBuilder.AppendLine(GetPage(data));
                messageBuilder.AppendLine(GetComment(data));

                if (data.Type == "edit")
                    messageBuilder.AppendLine(GetRevDiff(data));

                messageBuilder.AppendLine(GetDate(data));

                var content = new EmbedFieldBuilder()
                        .WithName("Description")
                        .WithValue(messageBuilder.ToString())
                        .WithIsInline(false);

                var footer = new EmbedFooterBuilder()
                        .WithText("Merci pour ton travail !")
                        .WithIconUrl("https://emmanuelistace.be/politibot/heart.png");

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

        private Embed CreateFileUploadEmbed(Change data)
        {
            try
            {
                var imageUrl = String.Empty;
                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.AppendLine(GetContributor(data));
                messageBuilder.AppendLine(GetPage(data));
                messageBuilder.AppendLine(GetComment(data));
                messageBuilder.AppendLine(GetDate(data));

                var content = new EmbedFieldBuilder()
                        .WithName("Description")
                        .WithValue(messageBuilder.ToString())
                        .WithIsInline(false);

                var footer = new EmbedFooterBuilder()
                        .WithText("Merci pour ton travail !")
                        .WithIconUrl("https://emmanuelistace.be/politibot/heart.png");

                imageUrl = GetImageUrl(data);

                var builder = new EmbedBuilder()
                        .WithAuthor(BuildTitle(data))
                        .AddField(content)
                        .WithFooter(footer)
                        .WithImageUrl(imageUrl)
                        .WithColor(Color.Gold);

                return builder.Build();
            }
            catch (Exception ex)
            {
                _logger.AddLog(ex.Message);
                return null;
            }
        }
        private Embed CreateNewUserEmbed(Change data)
        {
            try
            {
                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.AppendLine("Afin de nous aider le plus efficacement possible:");
                messageBuilder.AppendLine("• Tu peux consulter les règles du wiki [ici](https://politiwiki.fr/wiki/R%C3%A8gles)");
                messageBuilder.AppendLine("• Et tu peux consulter le guide de contribution [ici](https://politiwiki.fr/wiki/Guide_de_contribution)");
                messageBuilder.AppendLine("A bientôt !");

                var content = new EmbedFieldBuilder()
                        .WithName(String.Format("Bienvenue parmi nous {0} !", data.User))
                        .WithValue(messageBuilder.ToString())
                        .WithIsInline(false);

                var footer = new EmbedFooterBuilder()
                        .WithText("Merci de nous avoir rejoint !")
                        .WithIconUrl("https://emmanuelistace.be/politibot/heart.png");

                var builder = new EmbedBuilder()
                        .WithAuthor(BuildTitle(data))
                        .AddField(content)
                        .WithFooter(footer)
                        .WithColor(Color.Blue);

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
            if (data.IsNewUser())
                return new EmbedAuthorBuilder()
                    .WithName("Un·e nouveau·elle contributeur·ice nous à rejoint !")
                    .WithIconUrl("https://emmanuelistace.be/politibot/hand_wave.png");
            else if (data.Type == "edit")
                return new EmbedAuthorBuilder()
                    .WithName("Edition d'une page")
                    .WithIconUrl("https://emmanuelistace.be/politibot/pencil.png");
            else if (data.Type == "new")
                return new EmbedAuthorBuilder()
                    .WithName("Création d'une page")
                    .WithIconUrl("https://emmanuelistace.be/politibot/add.png");
            else if (data.Type == "log" && !data.IsFileUpload())
                return new EmbedAuthorBuilder()
                    .WithName("Action de maintenance")
                    .WithIconUrl("https://emmanuelistace.be/politibot/settings.png");
            else if (data.Type == "log" && data.IsFileUpload())
                return new EmbedAuthorBuilder()
                    .WithName("Ajout d'un nouveau fichier")
                    .WithIconUrl("https://emmanuelistace.be/politibot/camera.png");
            else
                return new EmbedAuthorBuilder();
        }

        private string GetComment(Change data) => String.Format("**Commentaire**: {0}", String.IsNullOrEmpty(data.Comment)? String.Empty : data.Comment);

        private string GetContributor(Change data) => String.Format("**Contributeur·ice**: [{0}](https://politiwiki.fr/wiki/Utilisateur:{1})", data.User, data.User.Replace(' ', '_'));

        private string GetPage(Change data) => String.Format("**Page**: [{0}]({1})", data.Title, GetWikiPageUrl(data));

        private string GetDate(Change data) =>  String.Format("**Date**: {0}", data.Date.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss"));

        private string GetRevDiff(Change data) => String.Format("**Modification**: [Consulter]({0})", BuildDiffUrl(data.Title, data.RevId, data.OldRevId));
        
        private string GetImageUrl(Change data)
        {
            string imageUrl = String.Empty;
            using (WebClient client = new WebClient())
            {
                string pageContent = client.DownloadString(GetWikiPageUrl(data));
                Regex regex = new Regex(@"(fullImageLink).*?(href=)(.*?)(>)");
                Match match = regex.Match(pageContent);
                imageUrl = String.Format("{0}/{1}", _wikiUrl, match.Groups[3].Value.Replace("\"", " ").Trim());
            }
            return imageUrl;
        }

        private string GetWikiPageUrl(Change data) => String.Format("{0}/wiki/{1}", _wikiUrl, data.Title.Replace(' ', '_'));

        private string BuildDiffUrl(string paneName, int diff, int oldid) => String.Format("{0}/w/index.php?title={1}&diff={2}&oldid={3}", _wikiUrl, paneName.Replace(' ', '_'), diff, oldid);
    }
}
   
