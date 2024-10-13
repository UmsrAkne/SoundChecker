using System;
using NAudio.Wave;

namespace SoundChecker.Models
{
    public sealed class SoundPlayer : IDisposable
    {
        private Mp3FileReader mp3FileReader;
        private WaveOutEvent waveOut;

        public void PlayMp3(string filePath)
        {
            mp3FileReader = new Mp3FileReader(filePath);
            waveOut = new WaveOutEvent();

            waveOut.Init(mp3FileReader);
            waveOut.Play();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            mp3FileReader.Dispose();
            waveOut.Dispose();
        }
    }
}