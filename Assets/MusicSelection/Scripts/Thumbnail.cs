using Rhythm;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MusicSelection
{
    public class Thumbnail : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _composerText;

        public void Set(Sprite sprite, BeatmapInformation beatmapInfo)
        {
            _image.sprite = sprite;
            _titleText.text = beatmapInfo.Title;
            _composerText.text = beatmapInfo.Composer;
        }
    }
}