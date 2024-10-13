using System.IO.Abstractions;
using Prism.Mvvm;
using SoundChecker.Models;

namespace SoundChecker.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private string currentDirectoryPath;

        public TextWrapper TextWrapper { get; } = new ();

        public IDirectoryInfo CurrenDirectoryInfo { get; set; }

        public string CurrentDirectoryPath
        {
            get => currentDirectoryPath;
            set
            {
                SetProperty(ref currentDirectoryPath, value);
                if (string.IsNullOrWhiteSpace(currentDirectoryPath))
                {
                    return;
                }

                CurrenDirectoryInfo = new FileSystem().DirectoryInfo.New(currentDirectoryPath);
            }
        }
    }
}