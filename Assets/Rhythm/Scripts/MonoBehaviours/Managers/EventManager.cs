using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Transition;
using DG.Tweening;
using UnityEngine.UI;

namespace Rhythm
{
    public class EventManager : MonoBehaviour
    {
        private bool _isVs;
        private TutorialId _tutorialId;
        private bool _isPause;
        private KeyConfig _keyConfig;
        private IBattleMode _battleMode;
        private ISoundPlayable _soundPlayable;
        private ISoundVolumeAdjustable _soundVolumeAdjustable;
        private IColorInputProvider _colorInputProvider;
        private IMoveInputProvider _moveInputProvider;
        private IPauseScreenDrawable _pauseScreenDrawable;
        private ITutorialDrawable _tutorialDrawable;
        private ISkipScreenDrawable _skipScreenDrawable;

        [SerializeField] private PlayerInput _playerInput;

        [SerializeField] private CustomButton _battleNextButton;
        [SerializeField] private CustomButton _rhythmNextButton;

        [SerializeField] private CustomButton _pauseResumeButton;
        [SerializeField] private CustomButton _pauseQuitButton;
        [SerializeField] private Slider _bgmVolumeSlider;
        [SerializeField] private Slider _seVolumeSlider;
        [SerializeField] private Slider _noteSeVolumeSlider;

        [SerializeField] private CustomButton _skipYesButton;
        [SerializeField] private CustomButton _skipNoButton;

        private readonly float _victoryFadeOut = 0.3f;

        public void Initialize(bool isVs, TutorialId tutorialId, KeyConfig keyConfig, IBattleMode battleMode, ISoundPlayable soundPlayable, ISoundVolumeAdjustable soundVolumeAdjustable, IColorInputProvider colorInputProvider, IMoveInputProvider moveInputProvider, IPauseScreenDrawable pauseScreenDrawable, ITutorialDrawable tutorialDrawable, ISkipScreenDrawable skipScreenDrawable)
        {
            _isVs = isVs;
            _tutorialId = tutorialId;
            _keyConfig = keyConfig;
            _battleMode = battleMode;
            _soundPlayable = soundPlayable;
            _soundVolumeAdjustable = soundVolumeAdjustable;
            _colorInputProvider = colorInputProvider;
            _moveInputProvider = moveInputProvider;
            _pauseScreenDrawable = pauseScreenDrawable;
            _tutorialDrawable = tutorialDrawable;
            _skipScreenDrawable = skipScreenDrawable;

            _isPause = false;
        }

        public void OnBattleNextButtonClick()
        {
            try
            {
                _battleNextButton.interactable = false;
                _soundPlayable.FadeOutIntroSE("Victory", _victoryFadeOut);

                switch (_tutorialId)
                {
                    case TutorialId.None:
                        SceneTransitionManager.TransitionToMap(_battleMode.IsWin);
                        break;
                    case TutorialId.Battle:
                        SceneTransitionManager.TransitionToRhythm(SceneTransitionManager.CurrentRhythmId, SceneTransitionManager.CurrentDifficulty, true);
                        break;
                    case TutorialId.Rhythm:
                        SceneTransitionManager.TransitionToMusicSelection();
                        break;
                }
            }
            catch
            {

            }
            
        }

        public void OnRhythmNextButtonClick()
        {
            try
            {
                _rhythmNextButton.interactable = false;
                _soundPlayable.FadeOutIntroSE("Victory", _victoryFadeOut);
                SceneTransitionManager.TransitionToMusicSelection();
            }
            catch
            {

            }
        }

        public void OnPauseResumeButtonClick()
        {
            _pauseResumeButton.interactable = false;
            _pauseQuitButton.interactable = false;
            _bgmVolumeSlider.interactable = false;
            _seVolumeSlider.interactable = false;
            _noteSeVolumeSlider.interactable = false;

            _pauseScreenDrawable.ErasePauseScreen().onComplete += () =>
            {
                _colorInputProvider.IsColorInputValid = false;
                _moveInputProvider.IsMoveInputValid = false;
                _playerInput.SwitchCurrentActionMap(_keyConfig.ToStringQuickly());

                var sequence = _pauseScreenDrawable.DrawCountDownScreen();
                sequence.OnComplete(() =>
                {
                    _colorInputProvider.IsColorInputValid = true;
                    _moveInputProvider.IsMoveInputValid = true;

                    _soundPlayable.UnPauseMusic();
                    Time.timeScale = 1;
                    _isPause = false;
                });

                sequence.Play();
            };
        }

        public void OnPauseQuitButtonClick()
        {
            try
            {
                _pauseResumeButton.interactable = false;
                _pauseQuitButton.interactable = false;
                _bgmVolumeSlider.interactable = false;
                _seVolumeSlider.interactable = false;
                _noteSeVolumeSlider.interactable = false;

                Time.timeScale = 1;

                if (_isVs && _tutorialId != TutorialId.Rhythm)
                {
                    SceneTransitionManager.TransitionToMap();
                }
                else
                {
                    SceneTransitionManager.TransitionToMusicSelection();
                }
            }
            catch
            {
                Time.timeScale = 0;
            }
        }

        public void OnSkipYesButtonClick()
        {
            try
            {
                _skipYesButton.interactable = false;
                _skipNoButton.interactable = false;

                Time.timeScale = 1;
                SceneTransitionManager.TransitionToRhythm(SceneTransitionManager.CurrentRhythmId, SceneTransitionManager.CurrentDifficulty, true);
            }
            catch
            {
                Time.timeScale = 0;
            }
        }

        public void OnSkipNoButtonClick()
        {
            _skipYesButton.interactable = false;
            _skipNoButton.interactable = false;

            _skipScreenDrawable.EraseSkipScreen().onComplete += () =>
            {
                _playerInput.SwitchCurrentActionMap(_keyConfig.ToStringQuickly());
                Time.timeScale = 1;
            };
        }

        public void SelectNextButton()
        {
            if (_isVs)
            {
                _battleNextButton.Select();
            }
            else
            {
                _rhythmNextButton.Select();
            }
        }

        public void SelectSkipNoButton()
        {
            _skipNoButton.Select();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed && !_isPause)
            {
                _playerInput.SwitchCurrentActionMap("Pause");
                _soundPlayable.PauseMusic();
                Time.timeScale = 0;
                _isPause = true;

                _pauseResumeButton.interactable = true;
                _pauseQuitButton.interactable = true;
                _bgmVolumeSlider.interactable = true;
                _seVolumeSlider.interactable = true;
                _noteSeVolumeSlider.interactable = true;

                _pauseResumeButton.Select();

                _pauseScreenDrawable.DrawPauseScreen();
            }
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnPauseResumeButtonClick();
            }
        }

        public void OnTutorialResume(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _tutorialDrawable.EraseTutorial().onComplete += () =>
                {
                    _playerInput.SwitchCurrentActionMap(_keyConfig.ToStringQuickly());
                    _soundPlayable.UnPauseMusic();
                    Time.timeScale = 1;
                };
            }
        }

        public void OnBgmVolumeChanged(float value)
        {
            _soundVolumeAdjustable.BgmVolume = value;
        }

        public void OnSeVolumeChanged(float value)
        {
            _soundVolumeAdjustable.SeVolume = value;
        }

        public void OnNoteSeVolumeChanged(float value)
        {
            _soundVolumeAdjustable.NoteSeVolume = value;
        }

        public void SetSoundVolume(RhythmVolumeSetting volumeSetting)
        {
            _bgmVolumeSlider.value = volumeSetting.Track;
            _seVolumeSlider.value = volumeSetting.Se;
            _noteSeVolumeSlider.value = volumeSetting.NoteSe;

            _soundVolumeAdjustable.BgmVolume = volumeSetting.Track;
            _soundVolumeAdjustable.SeVolume = volumeSetting.Se;
            _soundVolumeAdjustable.NoteSeVolume = volumeSetting.NoteSe;
        }
    }
}