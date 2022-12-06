namespace WMS.Service.WebAPI
{
    public class AppSettings
    {
        /// <summary>
        /// How long the cache cab be inactive
        /// </summary>
        /// <remarks>
        /// A cache entry will expire if it is not used by anyone for this particular time period
        /// </remarks>
        public int DefaultSlidingCacheMinutes { get; set; }

        /// <summary>
        /// Actual fixed expiration time for cached value
        /// </summary>
        /// <remarks>
        /// Once the time is reached, then the cache entry will be removed
        /// </remarks>
        public int DefaultAbosoluteCacheMinutes { get; set; } 

        //public string AppVersion { get; set; } 
        //public bool UsePerfLogs { get; set; } 
        //public bool TrackUsage { get; set; } 

        //public SecRole SecRole { get; set; }
        //public TinyPNG TinyPNG { get; set; }
        //public SMTPserver SMTP { get; set; }
        //public URLs URLs { get; set; }
        //public Paths Paths { get; set; }
        //public EmailTemplate EmailTemplate { get; set; }

    }

    //public class SecRole
    //{
    //    public string Level1 { get; set; }
    //    public string Level2 { get; set; }
    //    public string Admin { get; set; }
    //    public string LockoutHours { get; set; }
    //    public string MaxLoginAttempts { get; set; }
    //}

    //public class TinyPNG
    //{
    //    public string ApiKey { get; set; }
    //}

    //public class SMTPserver
    //{
    //    public string FromEmail { get; set; }
    //    public string FromName { get; set; }
    //    public string AdminEmail { get; set; }
    //    public string IP { get; set; }
    //    public string Port { get; set; }
    //    public string SSL { get; set; }
    //    public string UserName { get; set; }
    //    public string UserPassword { get; set; }
    //}

    //public class URLs
    //{
    //    public string HomeDomain { get; set; }
    //    public string ImageSite { get; set; }
    //    public string ImageHeaderFile { get; set; }
    //    public string RecipesRecipe { get; set; }
    //    public string JournalBatch { get; set; }
    //    public string ImageRecipes { get; set; }
    //    public string Stream { get; set; }
    //    public string StreamThumbs { get; set; }
    //}

    //public class Paths
    //{
    //    public string EmailTemplateFolder { get; set; }
    //}

    //public class EmailTemplate
    //{
    //    public string BodyTemplateFileName { get; set; }
    //    public string WelcomeTemplateFileName { get; set; }
    //    public string PasswordResetTemplateFileName { get; set; }
    //    public string WelcomeName { get; set; }
    //    public string BodyHeaderImage { get; set; }
    //    public string BodyMessage { get; set; }
    //    public string BodyButtonText { get; set; }
    //    public string BodyButtonHref { get; set; }
    //}

}
