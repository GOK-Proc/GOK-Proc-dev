using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class SoundPlayer : ITimeProvider, ISoundPlayable
    {
        private readonly AudioSource _audioSource;
        private readonly AudioClip _audioClip;
        private readonly IDictionary<string, AudioClip> _soundData;

        public SoundPlayer(AudioSource source, AudioClip clip, IDictionary<string, AudioClip> soundData)
        {
            _audioSource = source;
            _audioClip = clip;
            _soundData = soundData;
            
            _audioSource.clip = _audioClip;
        }

        public double Time { get => (double)_audioSource.timeSamples / _audioClip.frequency; set => _audioSource.timeSamples = (int)(value * _audioClip.frequency); }

        public void PlayMusic()
        {
            _audioSource.Play();
        }

        public void StopMusic()
        {
            _audioSource.Stop();
        }

        public void PauseMusic()
        {
            _audioSource.Pause();
        }

        public void UnPauseMusic()
        {
            _audioSource.UnPause();
        }

        public void PlaySE(string id)
        {
            if (_soundData.TryGetValue(id, out var clip))
            {
                _audioSource.PlayOneShot(clip);
            }
        }
    }
}