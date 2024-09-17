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
                SceneTransitionManager.TransitionToMusicSelection();
            }
            catch
            {

            }
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if (_isVs)
            {
                switch (context.phase)
                {
                    case InputActionPhase.Started:
                        _battleNextButton.PointerDown();
                        break;
                    case InputActionPhase.Performed:
                        _battleNextButton.Click();
                        break;
                    case InputActionPhase.Canceled:
                        _battleNextButton.PointerUp();
                        break;
                }
            }
            else
            {
                switch (context.phase)
                {
                    case InputActionPhase.Started:
                        _rhythmNextButton.PointerDown();
                        break;
                    case InputActionPhase.Performed:
                        _rhythmNextButton.Click();
                        break;
                    case InputActionPhase.Canceled:
                        _rhythmNextButton.PointerUp();
                        break;
                }
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playerInput.SwitchCurrentActionMap("Pause");
                _soundPlayable.PauseMusic();
                Time.timeScale = 0;

                _pauseScreenDrawable.DrawPauseScreen();
            }
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
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
        }
    }
}