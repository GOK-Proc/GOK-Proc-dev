using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;

namespace Rhythm
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CanvasGroup _instruction;
        [SerializeField] private CanvasGroup[] _pages;

        [SerializeField] float _instructionDuration;
        [SerializeField] float _instructionMinValue;

        [SerializeField] float _pageDuration;
        [SerializeField] float _pageFadeDuration;

        [SerializeField] float _inputDelay;

        private Tweener _blinkTweener;
        private Sequence _pageSequence;

        void OnEnable()
        {
            _blinkTweener = _instruction.DOFade(_instructionMinValue, _instructionDuration)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetUpdate(true);

            _pageSequence = DOTween.Sequence();

            foreach (var page in _pages)
            {
                _pageSequence
                    .AppendCallback(() =>
                    {
                        page.alpha = 0f;
                        page.gameObject.SetActive(true);
                    })
                    .Join(page.DOFade(1f, _pageFadeDuration))
                    .AppendInterval(_pageDuration)
                    .Append(page.DOFade(0f, _pageFadeDuration))
                    .AppendCallback(() =>
                    {
                        page.gameObject.SetActive(false);
                    });
            }

            _pageSequence.SetLoops(-1).SetUpdate(true).Play();

            _instruction.gameObject.SetActive(false);

            DOVirtual.DelayedCall(_inputDelay, () =>
            {
                _instruction.gameObject.SetActive(true);
                _playerInput.SwitchCurrentActionMap("Tutorial");
            });
        }

        private void OnDisable()
        {
            _blinkTweener?.Kill();
            _pageSequence?.Kill();
        }
    }
}