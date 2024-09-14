using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;

    [Header("On Click")]
    [SerializeField] private float _onClickScale;
    [SerializeField] private float _onClickDuration;
    [SerializeField] private Color _onClickColor;

    [Header("On Pointer Over")]
    [SerializeField] private float _onPointerOverScale;
    [SerializeField] private float _onPointerOverDuration;
    [SerializeField] private Color _onPointerOverColor;

    public Action OnClickEvent { get; set; }

    private float _defaultScale;
    private Color _defaultColor;

    private void Awake()
    {
        _defaultScale = transform.localScale.x;
        _defaultColor = _image.color;
    }

    private void AnimateButton(float endScale, Color endColor, float duration)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(endScale, duration));
        sequence.Join(_image.DOColor(endColor, duration));
        sequence.Play();
    }

    public void OnPointerClick(PointerEventData eventData) => OnClickEvent?.Invoke();

    public void OnPointerDown(PointerEventData eventData) => AnimateButton(_onClickScale, _onClickColor, _onClickDuration);

    public void OnPointerUp(PointerEventData eventData) => AnimateButton(_defaultScale, _defaultColor, _onClickDuration);

    public void OnPointerEnter(PointerEventData eventData) => AnimateButton(_onPointerOverScale, _onPointerOverColor, _onPointerOverDuration);

    public void OnPointerExit(PointerEventData eventData) => AnimateButton(_defaultScale, _defaultColor, _onPointerOverDuration);
}
