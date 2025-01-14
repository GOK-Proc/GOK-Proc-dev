using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderValueText : MonoBehaviour
{
    [SerializeField] private Transform _sliderHandle;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void OnValueChanged(float value)
    {
        _text.text = value switch
        {
            > 0 => $"＋{value}",
            < 0 => $"－{-value}",
            _ => $"{value}"
        };

        var pos = transform.localPosition;
        pos.x = _sliderHandle.localPosition.x;
        transform.localPosition = pos;
    }
}
