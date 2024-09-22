using TMPro;
using UnityEngine;
using Gallery;
using Rhythm;

namespace MusicSelection
{
    public class MusicSelectionThumbnail : ThumbnailBase
    {
        private TrackInformation _trackCache;
        
        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [SerializeField] private TextMeshProUGUI _maxComboText;

        [SerializeField] private RecordList _recordList;

        public override void Set(TrackInformation track)
        {
            _trackCache = track;
            
            _image.sprite = track.Thumbnail;

            _titleText.text = track.Title;
            _composerText.text = track.Composer;

            Set(DifficultySelection.Current);
        }

        public void Set(Difficulty difficulty)
        {
            var record = _recordList[_trackCache.Id][(int)difficulty];
            _bestScoreText.text = $"{record.Score.ToString()} pt";
            _maxComboText.text = $"{record.MaxCombo.ToString()} combo";
        }
    }
}