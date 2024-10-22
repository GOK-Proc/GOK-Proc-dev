using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class CustomButton : Selectable, ISubmitHandler
{
    [Space(20)]
    [SerializeField] private Image _image;

    [Header("Events")]
    [SerializeField] private UnityEvent _onSubmit;

    private float _defaultScale;
    private Color _defaultColor;

    private readonly float _onClickScale = 0.8f;
    private readonly float _onClickDuration = 0.1f;

    private readonly Color _onClickColor = new Color32(214, 77, 42, 255);

    protected override void Awake()
    {
        base.Awake();
        _defaultScale = transform.localScale.x;
        _defaultColor = _image.color;
    }

    private Sequence GetAnimationSequence(float endScale, Color endColor, float duration) => DOTween.Sequence().Append(transform.DOScale(endScale, duration)).Join(_image.DOColor(endColor, duration));
    private Sequence PressButtonSequence => GetAnimationSequence(_onClickScale, _onClickColor, _onClickDuration);
    private Sequence ReleaseButtonSequence => GetAnimationSequence(_defaultScale, _defaultColor, _onClickDuration);

    public void OnSubmit(BaseEventData eventData)
    {
        PressButtonSequence.OnComplete(() =>
        {
            _onSubmit?.Invoke();
            ReleaseButtonSequence.Play().SetUpdate(true);
        }).Play().SetUpdate(true);
    }
}
