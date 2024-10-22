using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NextMarkMove : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    private Tween _tween;

    private void Start()
    {
        float startPosY = _rectTransform.anchoredPosition.y;
        _tween = _rectTransform.DOAnchorPosY(startPosY + 20, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        _tween.Kill();
    }
}
