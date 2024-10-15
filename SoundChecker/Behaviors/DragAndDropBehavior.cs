using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.Json;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using SoundChecker.Models;
using SoundChecker.ViewModels;

namespace SoundChecker.Behaviors
{
    public class DragAndDropBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            // ファイルをドラッグしてきて、コントロール上に乗せた際の処理
            AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;

            // ファイルをドロップした際の処理
            AssociatedObject.Drop += AssociatedObject_Drop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewDragOver -= AssociatedObject_PreviewDragOver;
            AssociatedObject.Drop -= AssociatedObject_Drop;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            // ファイルパスの一覧の配列
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            var file = files?.FirstOrDefault();

            if (file == null)
            {
                return;
            }

            if (Path.GetExtension(file).ToLower() != ".json")
            {
                return;
            }

            var fileSystem = new FileSystem();
            var jsonString = fileSystem.File.ReadAllText(file);
            var fileList = JsonSerializer.Deserialize<List<ExtendedFileInfo>>(jsonString)
                .Select(f => new ExtendedFileInfo(fileSystem.FileInfo.New(f.FullName)) { Comment = f.Comment, })
                .ToDictionary(f => f.FullName);

            var notFounds = JsonSerializer.Deserialize<List<ExtendedFileInfo>>(jsonString)
                .Select(f => new ExtendedFileInfo(fileSystem.FileInfo.New(f.FullName)) { Comment = f.Comment, })
                .ToDictionary(f => f.FullName);

            if (((Window)sender).DataContext is MainWindowViewModel vm)
            {
                // Json から生成したオブジェクトの情報を読み込む。
                foreach (var extendedFileInfo in vm.Files)
                {
                    fileList.TryGetValue(extendedFileInfo.FullName, out var f);
                    if (f != null)
                    {
                        extendedFileInfo.Comment = f.Comment;
                        notFounds.Remove(extendedFileInfo.FullName);
                    }
                }

                // Json から生成したリストにあって、アプリ側のリストに存在しない要素は追加する。
                foreach (var notfound in notFounds)
                {
                    vm.Files.Add(notfound.Value);
                }
            }
        }

        private void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }
    }
}