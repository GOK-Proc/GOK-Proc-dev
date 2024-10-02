using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

namespace Rhythm
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _instruction;
        [SerializeField] private CanvasGroup[] _pages;

        [SerializeField] float _instructionDuration;
        [SerializeField] float _instructionMinValue;

        [SerializeField] float _pageDuration;
        [SerializeField] float _pageFadeDuration;

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
        }

        private void OnDisable()
        {
            _blinkTweener?.Kill();
            _pageSequence?.Kill();
        }
    }
}