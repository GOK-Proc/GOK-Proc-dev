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
            _image.sprite = info.Thumbnail;

            _titleText.text = info.Title;
            _composerText.text = info.Composer;

            _descriptionText.text = info.Description;
            _descriptionText.pageToDisplay = 1;
        }
    }
}