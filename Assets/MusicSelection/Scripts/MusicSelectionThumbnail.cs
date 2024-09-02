using TMPro;
using UnityEngine;
using Gallery;

namespace MusicSelection
{
    public class MusicSelectionThumbnail : ThumbnailBase
    {
        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [SerializeField] private TextMeshProUGUI _maxComboText;

        public override void Set(TrackInformation info)
        {
            _image.sprite = info.Thumbnail;

            _titleText.text = info.Title;
            _composerText.text = info.Composer;

            // TODO: ここでベストスコアと最大コンボ数変更
        }
    }
}