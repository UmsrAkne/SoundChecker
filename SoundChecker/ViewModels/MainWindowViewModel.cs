using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Linq;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using SoundChecker.Models;

namespace SoundChecker.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase, IDisposable
    {
        private readonly SoundPlayer soundPlayer = new ();
        private IFileSystem fileSystem;
        private string currentDirectoryPath;
        private ObservableCollection<ExtendedFileInfo> files = new ();

        public MainWindowViewModel()
        {
            SetDummies();
        }

        public MainWindowViewModel(IContainerProvider containerRegistry)
        {
            SetDummies();
            fileSystem = containerRegistry.Resolve<IFileSystem>();
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
                Files = new ObservableCollection<ExtendedFileInfo>(GetFiles(currentDirectoryPath));
            }
        }

        public ObservableCollection<ExtendedFileInfo> Files
        {
            get => files;
            set => SetProperty(ref files, value);
        }

        public TagGen TagGen { get; } = new ();

        public DelegateCommand<ExtendedFileInfo> PlaySoundCommand => new DelegateCommand<ExtendedFileInfo>((param) =>
        {
            if (param == null || !param.IsSoundFile())
            {
                return;
            }

            switch (param.FileInfo.Extension.ToLower())
            {
                case ".mp3":
                    soundPlayer.PlayMp3(param);
                    break;
                case ".ogg":
                    soundPlayer.PlayOgg(param);
                    break;
            }
        });

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            soundPlayer.Dispose();
        }

        private IEnumerable<ExtendedFileInfo> GetFiles(string targetDirectoryPath)
        {
            var filePaths = new FileSystem().Directory.GetFiles(targetDirectoryPath);
            return filePaths.Select(p => new ExtendedFileInfo(new FileSystem().FileInfo.New(p)));
        }

        [Conditional("DEBUG")]
        private void SetDummies()
        {
            Files.Add(new ExtendedFileInfo(new FileSystem().FileInfo.New("test1.ogg")) { Comment = "Comment1", Playing = true, });
            Files.Add(new ExtendedFileInfo(new FileSystem().FileInfo.New("test2.ogg")) { Comment = "Comment1", });
            Files.Add(new ExtendedFileInfo(new FileSystem().FileInfo.New("test3.ogg")) { Comment = "Comment1", });
            Files.Add(new ExtendedFileInfo(new FileSystem().FileInfo.New("test4.ogg")) { Comment = "Comment1", });
            Files.Add(new ExtendedFileInfo(new FileSystem().FileInfo.New("test5.ogg")) { Comment = string.Empty, });
        }
    }
}