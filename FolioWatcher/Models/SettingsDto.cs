namespace FolioWatcher.Models
{
    public class SettingsDto
    {
        public string SmtpHost { get; set; } = null!;

        public int SmtpPort { get; set; }
        
        public string SmtpEncryption { get; set; } = null!;
        
        public string SmtpUsername { get; set; } = null!;
        
        public string SmtpPassword { get; set; } = null!;
        
        public string SmtpDefaultFromEmail { get; set; } = null!;
        
        public string SmtpDefaultFromName { get; set; } = null!;

        public string SmtpToEmail { get; set; } = null!;

        public string SmtpToName { get; set; } = null!;

        public IEnumerable<string> Products { get; set; } = null!;
    }
}
