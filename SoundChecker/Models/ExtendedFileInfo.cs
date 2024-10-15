using System.IO;
using System.IO.Abstractions;
using System.Text.Json.Serialization;
using Prism.Mvvm;

namespace SoundChecker.Models
{
    public class ExtendedFileInfo : BindableBase
    {
        private bool playing;
        private string comment;

        /// <summary>
        /// Json への読み書きの際に使用する。
        /// </summary>
        public ExtendedFileInfo()
        {
        }

        public ExtendedFileInfo(IFileInfo fileInfo)
        {
            FileInfo = fileInfo;
            FullName = fileInfo.FullName;
        }

        [JsonIgnore]
        public bool Playing { get => playing; set => SetProperty(ref playing, value); }

        public string Comment { get => comment; set => SetProperty(ref comment, value); }

        [JsonIgnore]
        public IFileInfo FileInfo { get; private set; }

        public string FullName { get; set; }

        public bool IsSoundFile()
        {
            var extension = Path.GetExtension(FileInfo.Name).ToLower();
            return extension is ".mp3" or ".wav" or ".ogg";
        }
    }
}