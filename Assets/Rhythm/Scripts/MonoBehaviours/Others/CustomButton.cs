using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class CustomButton : Selectable, IPointerClickHandler
{
    [Space(20)]
    [SerializeField] private Image _image;

    [Header("Events")]
    [SerializeField] private UnityEvent _onClick;

    [Header("On Click")]
    [SerializeField] private float _onClickScale;
    [SerializeField] private float _onClickDuration;
    [SerializeField] private Color _onClickColor;

    [Header("On Pointer Over")]
    [SerializeField] private float _onPointerOverScale;
    [SerializeField] private float _onPointerOverDuration;
    [SerializeField] private Color _onPointerOverColor;

    private float _defaultScale;
    private Color _defaultColor;

    protected override void Awake()
    {
        base.Awake();
        _defaultScale = transform.localScale.x;
        _defaultColor = _image.color;
    }

    private void AnimateButton(float endScale, Color endColor, float duration)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(endScale, duration));
        sequence.Join(_image.DOColor(endColor, duration));
        sequence.Play().SetUpdate(true);
    }

    public void Click()
    {
        if (interactable) _onClick?.Invoke();
    }

    public void PointerDown()
    {
        if (interactable) AnimateButton(_onClickScale, _onClickColor, _onClickDuration);
    }

    public void PointerUp()
    {
        AnimateButton(_defaultScale, _defaultColor, _onClickDuration);
    }

    public void PointerEnter()
    {
        if (interactable) AnimateButton(_onPointerOverScale, _onPointerOverColor, _onPointerOverDuration);
    }

    public void PointerExit()
    {
        AnimateButton(_defaultScale, _defaultColor, _onPointerOverDuration);
    }


    public void OnPointerClick(PointerEventData eventData) => Click();

    public override void OnPointerDown(PointerEventData eventData) => PointerDown();

    public override void OnPointerUp(PointerEventData eventData) => PointerUp();

    public override void OnPointerEnter(PointerEventData eventData) => PointerEnter();

    public override void OnPointerExit(PointerEventData eventData) => PointerExit();
}
