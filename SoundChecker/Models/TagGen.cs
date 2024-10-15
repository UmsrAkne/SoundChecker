using Prism.Mvvm;

namespace SoundChecker.Models
{
    public class TagGen : BindableBase
    {
        private string templateText;
        private string fileName;
        private int repeatCount;

        public TagGen()
        {
            TemplateText = @$"<se fileName""{FileNamePlaceholder}"" repeatCount=""{RepeatCountPlaceholder}"" />";
        }

        public string TemplateText { get => templateText; set => SetProperty(ref templateText, value); }

        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }

        public int RepeatCount { get => repeatCount; set => SetProperty(ref repeatCount, value); }

        private string FileNamePlaceholder { get; set; } = "$FileName$";

        private string RepeatCountPlaceholder { get; set; } = "$RepeatCount$";

        public string GetTag()
        {
            return TemplateText.Replace(FileNamePlaceholder, FileName)
                .Replace(RepeatCountPlaceholder, RepeatCount.ToString());
        }
    }
}