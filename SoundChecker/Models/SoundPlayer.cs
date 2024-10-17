using System;
using NAudio.Vorbis;
using NAudio.Wave;

namespace SoundChecker.Models
{
    public sealed class SoundPlayer : IDisposable
    {
        private readonly WaveOutEvent waveOut;
        private Mp3FileReader mp3FileReader;
        private VorbisWaveReader vorbisWaveReader;
        private ExtendedFileInfo playingFileInfo;

        public SoundPlayer()
        {
            waveOut = new WaveOutEvent();
            waveOut.PlaybackStopped += (_, _) =>
            {
                if (waveOut.PlaybackState != PlaybackState.Stopped)
                {
                    return;
                }

                if (playingFileInfo != null)
                {
                    playingFileInfo.Playing = false;
                }

                playingFileInfo = null;
            };
        }

        public void PlayMp3(ExtendedFileInfo extendedFileInfo)
        {
            Stop();

            playingFileInfo = extendedFileInfo;
            playingFileInfo.Playing = true;
            mp3FileReader = new Mp3FileReader(extendedFileInfo.FileInfo.FullName);
            waveOut.Init(mp3FileReader);
            waveOut.Play();
        }

        public void PlayOgg(ExtendedFileInfo extendedFileInfo)
        {
            Stop();

            playingFileInfo = extendedFileInfo;
            playingFileInfo.Playing = true;
            vorbisWaveReader = new VorbisWaveReader(extendedFileInfo.FileInfo.FullName);
            waveOut.Init(vorbisWaveReader);
            waveOut.Play();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            mp3FileReader.Dispose();
            vorbisWaveReader.Dispose();
            waveOut.Dispose();
        }

        private void Stop()
        {
            if (playingFileInfo != null)
            {
                playingFileInfo.Playing = false;
                playingFileInfo = null;
            }

            waveOut.Stop();
            vorbisWaveReader?.Dispose();
            mp3FileReader?.Dispose();
            vorbisWaveReader = null;
            mp3FileReader = null;
        }
    }
}