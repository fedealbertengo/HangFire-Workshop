namespace HangFire_Workshop
{
    public class MailjetSettings
    {

        public string MailApiKey { get; set; }
        public string MailApiSecret { get; set; }

        public string MailMessageFromEmail { get; set; }

        public string MailMessageFromName { get; set; }

        public MailjetSettings()
        {
            this.MailApiKey = "4511574b30ea4db9050ef7c080fac7a9";
            this.MailApiSecret = "815f0bd3a7d267bcbc58cfb6638603f8";
            this.MailMessageFromEmail = "federico.albertengo@concentrix.com";
            this.MailMessageFromName = "HangFire Workshop";
        }
    }
}
