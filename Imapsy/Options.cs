using CommandLine;
using CommandLine.Text;

namespace Imapsy
{
    public class Options
    {
        [Option("sourceHost", Required = true, HelpText = "The address of the source IMAP server.")]
        public string SourceHost { get; set; }

        [Option("sourcePort", DefaultValue = 143, HelpText = "The port of the source IMAP server.")]
        public int SourcePort { get; set; }

        [Option("sourceUsername", Required = true, HelpText = "The username for the source IMAP server.")]
        public string SourceUsername { get; set; }

        [Option("sourcePassword", Required = true, HelpText = "The password for the source IMAP server.")]
        public string SourcePassword { get; set; }

        [Option("sourceFolder", Required = true, HelpText = "The folder on the source IMAP server.")]
        public string SourceFolder { get; set; }

        [Option("sourceUseSsl", DefaultValue = false, HelpText = "Whether or not to use SSL to connect to the source IMAP server.")]
        public bool SourceUseSsl { get; set; }


        [Option("targetHost", Required = true, HelpText = "The address of the destination IMAP server.")]
        public string TargetHost { get; set; }

        [Option("targetPort", DefaultValue = 143, HelpText = "The port of the destination IMAP server.")]
        public int TargetPort { get; set; }

        [Option("targetUsername", Required = true, HelpText = "The username for the destination IMAP server.")]
        public string TargetUsername { get; set; }

        [Option("targetPassword", Required = true, HelpText = "The password for the destination IMAP server.")]
        public string TargetPassword { get; set; }

        [Option("targetFolder", Required = true, HelpText = "The folder on the destination IMAP server.")]
        public string TargetFolder { get; set; }

        [Option("targetUseSsl", DefaultValue = false, HelpText = "Whether or not to use SSL to connect to the target IMAP server.")]
        public bool TargetUseSsl { get; set; }


        [Option("startCopyingAt", DefaultValue = 1, HelpText = "Item index (1-based) on which to start the copying. Useful when you had to stop copying before and don't want the start at the begin again.")]
        public int StartCopyingAt { get; set; }

        [Option("ignoreExistingItems", DefaultValue = false, HelpText = "Whether or not you want to skip the duplicate check.")]
        public bool IgnoreExistingItems { get; set; }

        [HelpOption]
        public string GetHelpText()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
