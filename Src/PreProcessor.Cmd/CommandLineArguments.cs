using Microsoft.Test.CommandLineParsing;

namespace Amido.PreProcessor.Cmd
{
    public class CommandLineArguments
    {
        [Required]
        public string ManifestFile { get; set; }

        [Required]
        public string Environment { get; set; }
    }
}
