using Prism.Mvvm;
using SoundChecker.Models;

namespace SoundChecker.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        public TextWrapper TextWrapper { get; } = new ();
    }
}