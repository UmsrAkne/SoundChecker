using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Abstractions;
using Prism.Mvvm;
using SoundChecker.Models;

namespace SoundChecker.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private string currentDirectoryPath;
        private ObservableCollection<ExtendedFileInfo> files = new ();

        public MainWindowViewModel()
        {
            SetDummies();
        }

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

        public ObservableCollection<ExtendedFileInfo> Files
        {
            get => files;
            set => SetProperty(ref files, value);
        }

        [Conditional("DEBUG")]
        private void SetDummies()
        {
            Files.Add(new ExtendedFileInfo(new FileSystem().FileInfo.New("test1.ogg")) { Comment = "Comment1", });
            Files.Add(new ExtendedFileInfo(new FileSystem().FileInfo.New("test2.ogg")) { Comment = "Comment1", });
            Files.Add(new ExtendedFileInfo(new FileSystem().FileInfo.New("test3.ogg")) { Comment = "Comment1", });
            Files.Add(new ExtendedFileInfo(new FileSystem().FileInfo.New("test4.ogg")) { Comment = "Comment1", });
            Files.Add(new ExtendedFileInfo(new FileSystem().FileInfo.New("test5.ogg")) { Comment = string.Empty, });
        }
    }
}