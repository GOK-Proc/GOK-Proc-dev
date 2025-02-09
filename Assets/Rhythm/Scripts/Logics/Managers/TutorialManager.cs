using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Settings;

namespace Rhythm
{
    public class TutorialManager
    {
        private readonly bool _isTutorial;
        private readonly KeyConfigId _keyConfig;
        private readonly TutorialData _data;
        private readonly PlayerInput _playerInput;
        private readonly AudioSource _subAudioSource;
        private readonly ISoundPlayable _soundPlayable;
        private readonly ITimeProvider _timeProvider;
        private readonly ITutorialDrawable _tutorialDrawable;
        private readonly InputAction _resume;

        private int _next;

        private enum Mode
        {
            Play,
            Displayed,
            Wait
        }

        private Mode _currentMode;
        private bool _loopFlag;
        private bool _resumeFlag;

        public bool CanPause => _currentMode == Mode.Play;

        public TutorialManager(bool isTutorial, KeyConfigId keyConfig, TutorialData data, PlayerInput playerInput, AudioSource subAudioSource, ISoundPlayable soundPlayable, ITimeProvider timeProvider, ITutorialDrawable tutorialDrawable)
        {
            _isTutorial = isTutorial;
            _keyConfig = keyConfig;
            _data = data;
            _playerInput = playerInput;
            _subAudioSource = subAudioSource;
            _soundPlayable = soundPlayable;
            _timeProvider = timeProvider;

            _next = 0;
            _currentMode = Mode.Play;
            _loopFlag = true;
            _resumeFlag = false;
            _tutorialDrawable = tutorialDrawable;

            _resume = _playerInput.actions.FindActionMap("Tutorial")?["Resume"];
        }

        public void Update()
        {
            if (_isTutorial)
            {
                switch (_currentMode)
                {
                    case Mode.Play:

                        if (_next < _data.Times.Length)
                        {
                            if (_timeProvider.Time >= _data.Times[_next] + _data.Beatmap.Offset)
                            {
                                _playerInput.SwitchCurrentActionMap("None");
                                _soundPlayable.PauseMusic();
                                _subAudioSource.Play();
                                _loopFlag = true;
                                _resumeFlag = false;
                                Time.timeScale = 0;

                                _tutorialDrawable.DrawTutorial(_next, _keyConfig);
                                _currentMode = Mode.Displayed;
                            }
                        }

                        break;

                    case Mode.Displayed:

                        if (!_subAudioSource.isPlaying)
                        {
                            _subAudioSource.Play();
                        }

                        if (_resume is not null && _resume.triggered)
                        {
                            _playerInput.SwitchCurrentActionMap("None");
                            _tutorialDrawable.EraseTutorial().onComplete += () =>
                            {
                                _playerInput.SwitchCurrentActionMap(_keyConfig.ToString());
                                
                                _currentMode = Mode.Wait;
                            };
                        }

                        break;
                    case Mode.Wait:

                        if (!_subAudioSource.isPlaying)
                        {
                            if (_loopFlag)
                            {
                                _subAudioSource.Play();
                                _loopFlag = false;
                            }
                            else
                            {
                                if (!_resumeFlag)
                                {
                                    _soundPlayable.UnPauseMusic();
                                    Time.timeScale = 1;

                                    _resumeFlag = true;
                                }

                                _next++;
                                _currentMode = Mode.Play;
                            }
                        }
                        else
                        {
                            if (_loopFlag && _subAudioSource.time < _subAudioSource.clip.length - _data.Delay)
                            {
                                _loopFlag = false;
                            }

                            if (!_resumeFlag && !_loopFlag && _subAudioSource.time >= _subAudioSource.clip.length - _data.Delay - Time.deltaTime / 2)
                            {
                                _soundPlayable.UnPauseMusic();
                                Time.timeScale = 1;

                                _resumeFlag = true;
                            }
                        }

                        break;
                }
            }
        }

    }
}