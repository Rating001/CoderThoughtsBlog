namespace CoderThoughtsBlog.ViewModels
{
    public class MailSettings
    {
        //Settings to configure and use the SMTP server
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string MailPassword { get; set; }
        public string MailHost { get; set; }
        public int MailPort { get; set; }

    }
}
