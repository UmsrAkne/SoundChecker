using System.IO;
using System.IO.Abstractions;
using Prism.Mvvm;

namespace SoundChecker.Models
{
    public class ExtendedFileInfo : BindableBase
    {
        private bool playing;
        private string comment;

        public ExtendedFileInfo(IFileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public bool Playing { get => playing; set => SetProperty(ref playing, value); }

        public string Comment { get => comment; set => SetProperty(ref comment, value); }

        public IFileInfo FileInfo { get; private set; }

        public bool IsSoundFile()
        {
            var extension = Path.GetExtension(FileInfo.Name).ToLower();
            return extension is ".mp3" or ".wav" or ".ogg";
        }
    }
}