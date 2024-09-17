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

        private int _pauseCursorIndex;
        private float[] _pauseMenuY;

        private readonly int _pauseMenuCount = 2;

        public void Initialize(bool isVs, IBattleMode battleMode, ISoundPlayable soundPlayable, IColorInputProvider colorInputProvider, IMoveInputProvider moveInputProvider, IPauseScreenDrawable pauseScreenDrawable)
        {
            _isVs = isVs;
            _battleMode = battleMode;
            _soundPlayable = soundPlayable;
            _colorInputProvider = colorInputProvider;
            _moveInputProvider = moveInputProvider;
            _pauseScreenDrawable = pauseScreenDrawable;

            _pauseCursorIndex = 0;
            _pauseMenuY = new float[] { _pauseResumeButton.transform.position.y, _pauseQuitButton.transform.position.y };
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

                _pauseCursorIndex = 0;
                _pauseScreenDrawable.SetPauseCursorPositionY(_pauseMenuY[_pauseCursorIndex]);

                _pauseResumeButton.interactable = true;
                _pauseQuitButton.interactable = true;

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

        public void OnUp(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _pauseCursorIndex--;

                if (_pauseCursorIndex < 0) _pauseCursorIndex = 0;
                _pauseScreenDrawable.SetPauseCursorPositionY(_pauseMenuY[_pauseCursorIndex]);
            }
        }

        public void OnDown(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _pauseCursorIndex++;

                if (_pauseCursorIndex >= _pauseMenuCount) _pauseCursorIndex = _pauseMenuCount - 1;
                _pauseScreenDrawable.SetPauseCursorPositionY(_pauseMenuY[_pauseCursorIndex]);
            }
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            switch (_pauseCursorIndex)
            {
                case 0:
                    switch (context.phase)
                    {
                        case InputActionPhase.Started:
                            _pauseResumeButton.PointerDown();
                            break;
                        case InputActionPhase.Performed:
                            _pauseResumeButton.Click();
                            break;
                        case InputActionPhase.Canceled:
                            _pauseResumeButton.PointerUp();
                            break;
                    }
                    break;
                case 1:
                    switch (context.phase)
                    {
                        case InputActionPhase.Started:
                            _pauseQuitButton.PointerDown();
                            break;
                        case InputActionPhase.Performed:
                            _pauseQuitButton.Click();
                            break;
                        case InputActionPhase.Canceled:
                            _pauseQuitButton.PointerUp();
                            break;
                    }
                    break;
            }
            
        }
    }
}