using System.Text.RegularExpressions;
using UnityEngine;
using MusicSelection;

namespace Gallery
{
    public class GalleryThumbnail : ThumbnailBase
    {
        [SerializeField] private Description _description;

        public override void Set(TrackInformation track)
        {
            _image.sprite = track.Thumbnail;

            _titleText.text = track.Title;
            _composerText.text = track.Composer;

            // Unicodeエスケープシーケンスに挟まった\nを改行文字に変換
            var unescapedDescription = Regex.Unescape(track.Description);
            _description.Set(unescapedDescription);
        }
    }
}