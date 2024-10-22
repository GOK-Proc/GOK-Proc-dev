using Rhythm;
using System.Collections;
using System.Collections.Generic;
using Transition;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

namespace Novel
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _pauseBox;
        [SerializeField] private CanvasGroup _pauseBoxCanvasGroup;
        [SerializeField] private float _pauseBoxDuration;
        [SerializeField] private CustomButton _pauseResumeButton;
        [SerializeField] private CustomButton _pauseQuitButton;

        [SerializeField] private PlayerInput _playerInput;

        public void OnPauseResumeButtonClick()
        {
            _pauseResumeButton.interactable = false;
            _pauseQuitButton.interactable = false;

            ErasePauseScreen().onComplete += () =>
            {
                _playerInput.SwitchCurrentActionMap("Novel");
                Time.timeScale = 1;
            };
        }

        public void OnPauseQuitButtonClick()
        {
            try
            {
                _pauseResumeButton.interactable = false;
                _pauseQuitButton.interactable = false;

                Time.timeScale = 1;

                SceneTransitionManager.TransitionToMap();
            }
            catch
            {
                Time.timeScale = 0;
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playerInput.SwitchCurrentActionMap("Pause");
                Time.timeScale = 0;

                _pauseResumeButton.interactable = true;
                _pauseQuitButton.interactable = true;

                _pauseResumeButton.Select();

                DrawPauseScreen();
            }
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnPauseResumeButtonClick();
            }
        }

        private Tweener DrawPauseScreen()
        {
            _pauseBoxCanvasGroup.alpha = 0f;
            _pauseBox.gameObject.SetActive(true);

            return _pauseBoxCanvasGroup.DOFade(1f, _pauseBoxDuration).SetUpdate(true);
        }

        private Tweener ErasePauseScreen() => _pauseBoxCanvasGroup.DOFade(0f, _pauseBoxDuration).SetUpdate(true).OnComplete(() =>
        {
            _pauseBox.gameObject.SetActive(false);
            _pauseBoxCanvasGroup.alpha = 1f;
        });
    }
}
