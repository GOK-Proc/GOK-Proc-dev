using MusicSelection;
using UnityEngine;

namespace Gallery
{
    public class GalleryThumbnail : ThumbnailBase
    {
        [SerializeField] private Description _description;

        public override void Set(TrackInformation track)
        {
            _image.sprite = track.Thumbnail;

            _titleText.text = track.Title;
            _composerText.text = $"作曲：{track.Composer}";

            _description.Set(track.Description);
        }
    }
}