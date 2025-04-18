﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Rhythm
{
    public class SoundPlayer : ITimeProvider, ISoundPlayable, ISoundVolumeAdjustable
    {
        public struct AudioClipData
        {
            public AudioClip Clip;
            public bool IsNoteSe;
            public int SourceCount;
            public bool IsLoop;

            public AudioClipData(AudioClip clip, bool isNoteSe, int sourceCount, bool isLoop)
            {
                Clip = clip;
                IsNoteSe = isNoteSe;
                SourceCount = sourceCount;
                IsLoop = isLoop;
            }
        }

        public struct IntroAudioData
        {
            public AudioClip IntroClip;
            public AudioClip MainClip;
            public bool IsLoop;

            public IntroAudioData(AudioClip introClip, AudioClip mainClip, bool isLoop)
            {
                IntroClip = introClip;
                MainClip = mainClip;
                IsLoop = isLoop;
            }
        }

        private readonly AudioSource _audioSource;
        private readonly AudioClip _audioClip;
        private readonly AudioSource _subAudioSource;
        private readonly IDictionary<string, AudioClipData> _soundData;
        private readonly IDictionary<string, IntroAudioData> _introData;

        private readonly AudioSource _seSource;
        private readonly AudioSource _noteSeSource;
        private readonly IDictionary<string, IList<AudioSource>> _seSources;
        private readonly IDictionary<string, IList<Tweener>> _seFadeOutTweener;
        private readonly IDictionary<string, IntroSoundPlayer> _introSoundPlayers;

        private readonly RhythmVolumeSetting _rhythmVolumeSetting;

        public float BgmVolume
        {
            get => _rhythmVolumeSetting.Track;
            set
            {
                _rhythmVolumeSetting.Track = value;
                _audioSource.volume = _rhythmVolumeSetting.Track;
                _subAudioSource.volume = _rhythmVolumeSetting.Track;
            }
        }

        public float SeVolume
        {
            get => _rhythmVolumeSetting.Se;
            set
            {
                _rhythmVolumeSetting.Se = value;
                _seSource.volume = _rhythmVolumeSetting.Se;

                foreach (var data in _soundData)
                {
                    if (data.Value.SourceCount > 0 && !data.Value.IsNoteSe)
                    {
                        foreach (var source in _seSources[data.Key])
                        {
                            source.volume = _rhythmVolumeSetting.Se;
                        }
                    }
                }

                foreach (var data in _introData)
                {
                    _introSoundPlayers[data.Key].Volume = _rhythmVolumeSetting.Se;
                }
            }
        }

        public float NoteSeVolume
        {
            get => _rhythmVolumeSetting.NoteSe;
            set
            {
                _rhythmVolumeSetting.NoteSe = value;
                _noteSeSource.volume = _rhythmVolumeSetting.NoteSe;

                foreach (var data in _soundData)
                {
                    if (data.Value.SourceCount > 0 && data.Value.IsNoteSe)
                    {
                        foreach (var source in _seSources[data.Key])
                        {
                            source.volume = _rhythmVolumeSetting.NoteSe;
                        }
                    }
                }
            }
        }

        public bool IsPlayingMusic => _audioSource.isPlaying;

        public SoundPlayer(AudioSource source, AudioSource seSource, AudioSource subAudioSource, IntroSoundPlayer introSoundPlayer, AudioClip clip, IDictionary<string, AudioClipData> soundData, IDictionary<string, IntroAudioData> introData, RhythmVolumeSetting rhythmVolumeSetting)
        {
            _audioSource = source;
            _audioClip = clip;
            _subAudioSource = subAudioSource;
            _soundData = soundData;
            _introData = introData;
            
            _audioSource.clip = _audioClip;

            _seSource = Object.Instantiate(seSource, _audioSource.transform);
            _noteSeSource = Object.Instantiate(seSource, _audioSource.transform);
            _seSources = new Dictionary<string, IList<AudioSource>>();
            _seFadeOutTweener = new Dictionary<string, IList<Tweener>>();
            _introSoundPlayers = new Dictionary<string, IntroSoundPlayer>();

            foreach (var data in _soundData)
            {
                if (data.Value.SourceCount > 0)
                {
                    _seSources.Add(data.Key, new List<AudioSource>());
                    _seFadeOutTweener.Add(data.Key, new List<Tweener>());

                    for (int i = 0; i < data.Value.SourceCount; i++)
                    {
                        var audioSource = Object.Instantiate(seSource, _audioSource.transform);
                        audioSource.clip = data.Value.Clip;
                        audioSource.loop = data.Value.IsLoop;
                        _seSources[data.Key].Add(audioSource);
                        _seFadeOutTweener[data.Key].Add(null);
                    }
                }
            }

            foreach (var data in _introData)
            {
                var player = Object.Instantiate(introSoundPlayer, _audioSource.transform);

                player.IntroSource.clip = data.Value.IntroClip;
                player.MainSource.clip = data.Value.MainClip;
                player.MainSource.loop = data.Value.IsLoop;

                _introSoundPlayers.Add(data.Key, player);
            }

            _rhythmVolumeSetting = rhythmVolumeSetting;

            BgmVolume = _rhythmVolumeSetting.Track;
            SeVolume = _rhythmVolumeSetting.Se;
            NoteSeVolume = _rhythmVolumeSetting.NoteSe;
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

        public void FadeOutMusic(float duration)
        {
            _audioSource.DOFade(0f, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                StopMusic();
                _audioSource.volume = _rhythmVolumeSetting.Track;
            });
        }

        public void PlaySE(string id, float delay = 0f)
        {
            if (_soundData.TryGetValue(id, out var data))
            {
                if (data.Clip != null)
                {
                    if (delay > 0)
                    {
                        DOVirtual.DelayedCall(delay, () =>
                        {
                            if (data.IsNoteSe)
                            {
                                _noteSeSource.PlayOneShot(data.Clip);
                            }
                            else
                            {
                                _seSource.PlayOneShot(data.Clip);
                            }
                        }, false);
                    }
                    else
                    {
                        if (data.IsNoteSe)
                        {
                            _noteSeSource.PlayOneShot(data.Clip);
                        }
                        else
                        {
                            _seSource.PlayOneShot(data.Clip);
                        }
                    }
                }
            }
        }

        public void PlaySE(string id, int index, float delay = 0f)
        {
            if (_soundData.TryGetValue(id, out var data))
            {
                if (data.SourceCount > 0 && index >= 0 && index < data.SourceCount)
                {
                    _seFadeOutTweener[id][index]?.Kill();
                    if (delay > 0)
                    {
                        _seSources[id][index].PlayDelayed(delay);
                    }
                    else
                    {
                        _seSources[id][index].Play();
                    }
                }
            }
        }

        public void StopSE(string id, int index)
        {
            if (_soundData.TryGetValue(id, out var data))
            {
                if (data.SourceCount > 0 && index >= 0 && index < data.SourceCount)
                {
                    _seFadeOutTweener[id][index]?.Kill();
                    _seSources[id][index].Stop();
                }
            }
        }

        public void FadeOutSE(string id, int index, float duration)
        {
            if (_soundData.TryGetValue(id, out var data))
            {
                if (data.SourceCount > 0 && index >= 0 && index < data.SourceCount)
                {
                    _seFadeOutTweener[id][index]?.Kill();
                    _seFadeOutTweener[id][index] = _seSources[id][index].DOFade(0f, duration).SetEase(Ease.Linear).OnKill(() =>
                    {
                        if (_seSources[id][index] != null)
                        {
                            _seSources[id][index].Stop();
                            _seSources[id][index].volume = data.IsNoteSe ? _rhythmVolumeSetting.NoteSe : _rhythmVolumeSetting.Se;
                        }
                    });
                }
            }
        }

        public void PlayIntroSE(string id, float delay = 0f)
        {
            if (_introSoundPlayers.TryGetValue(id, out var player))
            {
                if (delay > 0)
                {
                    DOVirtual.DelayedCall(delay, () =>
                    {
                        player.Play();
                    }, false);
                }
                else
                {
                    player.Play();
                }
            }
        }

        public void StopIntroSE(string id)
        {
            if (_introSoundPlayers.TryGetValue(id, out var player))
            {
                player.Stop();
            }
        }

        public void FadeOutIntroSE(string id, float duration)
        {
            if (_introSoundPlayers.TryGetValue(id, out var player))
            {
                player.FadeOut(duration);
            }
        }
    }
}