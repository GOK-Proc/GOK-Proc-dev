using MusicSelection;
using TMPro;
using UnityEngine;

namespace Gallery
{
    public class GalleryThumbnail : ThumbnailBase
    {
        [SerializeField] private TextMeshProUGUI _descriptionText;

        public override void Set(TrackInformation info)
        {
            //TODO: ここで_image変更

            _titleText.text = info.Title;
            _composerText.text = info.Composer;

            _descriptionText.text = info.Description;
        }
    }
}