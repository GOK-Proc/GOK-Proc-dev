using Gallery;

namespace MusicSelection
{
    public class MusicSelectionThumbnail : ThumbnailBase
    {
        // TODO: ここでベストスコアと最大コンボ数のフィールド宣言

        public override void Set(TrackInformation info)
        {
            // TODO: ここで_image変更

            _titleText.text = info.Title;
            _composerText.text = info.Composer;

            // TODO: ここでベストスコアと最大コンボ数変更
        }
    }
}