using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Transition;
using DG.Tweening;

namespace Rhythm
{
    public class EventManager : MonoBehaviour
    {
        private bool _isVs;
        private IBattleMode _battleMode;
        private ISoundPlayable _soundPlayable;
        private IColorInputProvider _colorInputProvider;
        private IMoveInputProvider _moveInputProvider;
        private IPauseScreenDrawable _pauseScreenDrawable;

        [SerializeField] private PlayerInput _playerInput;

        [SerializeField] private CustomButton _battleNextButton;
        [SerializeField] private CustomButton _rhythmNextButton;

        [SerializeField] private CustomButton _pauseResumeButton;
        [SerializeField] private CustomButton _pauseQuitButton;

        private readonly float _victoryFadeOut = 0.3f;

        public void Initialize(bool isVs, IBattleMode battleMode, ISoundPlayable soundPlayable, IColorInputProvider colorInputProvider, IMoveInputProvider moveInputProvider, IPauseScreenDrawable pauseScreenDrawable)
        {
            _isVs = isVs;
            _battleMode = battleMode;
            _soundPlayable = soundPlayable;
            _colorInputProvider = colorInputProvider;
            _moveInputProvider = moveInputProvider;
            _pauseScreenDrawable = pauseScreenDrawable;
        }

        public void OnBattleNextButtonClick()
        {
            try
            {
                _battleNextButton.interactable = false;
                _soundPlayable.FadeOutIntroSE("Victory", _victoryFadeOut);
                SceneTransitionManager.TransitionToMap(_battleMode.IsWin);
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

            _pauseScreenDrawable.ErasePauseScreen().OnComplete(() =>
            {
                _colorInputProvider.IsColorInputValid = false;
                _moveInputProvider.IsMoveInputValid = false;
                _playerInput.SwitchCurrentActionMap("Rhythm");

                var sequence = _pauseScreenDrawable.DrawCountDownScreen();
                sequence.OnComplete(() =>
                {
                    _colorInputProvider.IsColorInputValid = true;
                    _moveInputProvider.IsMoveInputValid = true;

                    _soundPlayable.UnPauseMusic();
                    Time.timeScale = 1;
                });

                sequence.Play();
            });
        }

        public void OnPauseQuitButtonClick()
        {
            try
            {
                _pauseResumeButton.interactable = false;
                _pauseQuitButton.interactable = false;

                Time.timeScale = 1;

                if (_isVs)
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
                _pauseResumeButton.interactable = true;
            }
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

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playerInput.SwitchCurrentActionMap("Pause");
                _soundPlayable.PauseMusic();
                Time.timeScale = 0;

                _pauseResumeButton.interactable = true;
                _pauseQuitButton.interactable = true;

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
    }
}