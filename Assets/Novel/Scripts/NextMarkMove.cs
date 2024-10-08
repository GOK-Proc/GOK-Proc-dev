using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NextMarkMove : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    private void Start()
    {
        float startPosY = _rectTransform.anchoredPosition.y;
        _rectTransform.DOAnchorPosY(startPosY + 20, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
