using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Gallery;

namespace MusicSelection
{
    public class Thumbnail : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _composerText;

        public void Set(Sprite sprite, TrackInformation trackInfo)
        {
            _image.sprite = sprite ?? _image.sprite;
            _titleText.text = trackInfo.Title;
            _composerText.text = trackInfo.Composer;
        }
    }
}