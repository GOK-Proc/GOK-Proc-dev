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

        public override void Set(TrackInformation info)
        {
            _trackCache = info;
            
            _image.sprite = info.Thumbnail;

            _titleText.text = info.Title;
            _composerText.text = info.Composer;

            Set(DifficultySelection.Current);
        }

        public void Set(Difficulty difficulty)
        {
            var record = _recordList[_trackCache.Id][(int)difficulty];
            _bestScoreText.text = record.Score.ToString();
            _maxComboText.text = record.MaxCombo.ToString();
        }
    }
}