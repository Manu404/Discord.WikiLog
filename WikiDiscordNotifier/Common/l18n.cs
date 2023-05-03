using Newtonsoft.Json;

namespace WikiDiscordNotifier
{
    internal class l18n
    {

        [JsonProperty] public string EditTitle { get; set; }
        [JsonProperty] public string CreatePageTitle { get; set; }
        [JsonProperty] public string MaintenanceActionTitle { get; set; }
        [JsonProperty] public string FileUploadTitle { get; set; }
        [JsonProperty] public string NewUserTitle { get; set; }

        [JsonProperty] public string Commentary { get; set; }
        [JsonProperty] public string Contributor { get; set; }
        [JsonProperty] public string Page { get; set; }
        [JsonProperty] public string Date { get; set; }
        [JsonProperty] public string Consult { get; set; }
        [JsonProperty] public string Modification { get; set; }

        [JsonProperty] public string DefaulContentTitle { get; set; }
        [JsonProperty] public string DefaultFooter { get; set; }
        [JsonProperty] public string DefaultFooterLogoUrl { get; set; }

        [JsonProperty] public string FileUploadContentTitle { get; set; }
        [JsonProperty] public string FileUploadFooter { get; set; }
        [JsonProperty] public string FileUploadFooterLogoUrl { get; set; }

        [JsonProperty] public string NewUserWelcomeMessageContentTitle { get; set; }
        [JsonProperty] public string NewUserWelcomeMessage { get; set; }
        [JsonProperty] public string NewUserWelcomeMessageFooter { get; set; }
        [JsonProperty] public string NewUserWelcomeMessageEmbedLogoUrl { get; set; }

        [JsonProperty] public string NewUserEmbedLogoUrl { get; set; }
        [JsonProperty] public string EditPageEmbedLogoUrl { get; set; }
        [JsonProperty] public string CreatePageEmbedLogoUrl { get; set; }
        [JsonProperty] public string MaintenanceActionEmbedLogoUrl { get; set; }
        [JsonProperty] public string FileUploadEmbedLogoUrl { get; set; }

        [JsonProperty] public string WebhookMessage { get; set; }

        public l18n()
        {
            EditTitle = "A Page has been edited";
            CreatePageTitle = "A new page was created";
            MaintenanceActionTitle = "Maintenance action";

            Commentary = "Commentary";
            Contributor = "Contributor";
            Page = "Page";
            Modification = "Modification";
            Date = "Date";
            Consult = "Consult";

            DefaulContentTitle = "Description";
            DefaultFooter = "Thanks you for your work !";
            DefaultFooterLogoUrl = "https://discordwikilog.emmanuelistace.be/visuals/visuals/heart.png";

            FileUploadTitle = "A new file has been added";
            FileUploadContentTitle = "Description";
            FileUploadFooter = "Thanks for your work !";
            FileUploadFooterLogoUrl = "https://discordwikilog.emmanuelistace.be/visuals/visuals/heart.png";

            NewUserTitle = "A new user joined us !";
            NewUserWelcomeMessageContentTitle = "Welcome amoung us";
            NewUserWelcomeMessage = @"Welcome among us, to help us the best you can:\n • Consult the wiki rules\n • Consult the contribution guide\nSee you soon !";
            NewUserWelcomeMessageFooter = "Thanks for joining us";
            NewUserWelcomeMessageEmbedLogoUrl = "https://discordwikilog.emmanuelistace.be/visuals/visuals/heart.png";

            NewUserEmbedLogoUrl = "https://discordwikilog.emmanuelistace.be/visuals/visuals/hand_wave.png";
            EditPageEmbedLogoUrl = "https://discordwikilog.emmanuelistace.be/visuals/visuals/pencil.png";
            CreatePageEmbedLogoUrl = "https://discordwikilog.emmanuelistace.be/visuals/visuals/add.png";
            MaintenanceActionEmbedLogoUrl = "https://discordwikilog.emmanuelistace.be/visuals/visuals/settings.png";
            FileUploadEmbedLogoUrl = "https://discordwikilog.emmanuelistace.be/visuals/visuals/camera.png";

            WebhookMessage = "New changes are available !";
        }
    }
}
