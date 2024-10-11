using TMPro;
using UnityEngine;
using Gallery;
using Rhythm;

namespace MusicSelection
{
    public class MusicSelectionThumbnail : ThumbnailBase
    {
        private TrackInformation _track;
        private NotesInformation[] _notes;

        [SerializeField] private TextMeshProUGUI _notesDesignerText;

        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [SerializeField] private TextMeshProUGUI _maxComboText;

        [SerializeField] private TextMeshProUGUI _allPerfectText;
        [SerializeField] private TextMeshProUGUI _fullComboText;

        [SerializeField] private RecordList _recordList;
        [SerializeField] private BeatmapData _beatmapData;

        public override void Set(TrackInformation track)
        {
            _track = track;
            _image.sprite = track.Thumbnail;
            _titleText.text = track.Title;
            _composerText.text = track.Composer;

            _notes = _beatmapData.BeatmapDictionary[track.Id].Notes;

            Set(DifficultySelection.Current);
        }

        public void Set(Difficulty difficulty)
        {
            var record = _recordList[_track.Id][(int)difficulty];
            _bestScoreText.text = $"{record.Score.ToString()} pt";
            _maxComboText.text = $"{record.MaxCombo.ToString()} combo";

            _allPerfectText.gameObject.SetActive(record.Achievement switch
            {
                Achievement.AllPerfect => true, _ => false
            });
            _fullComboText.gameObject.SetActive(record.Achievement switch
            {
                Achievement.FullCombo => true, _ => false
            });

            _notesDesignerText.text = _notes[(int)difficulty].NotesDesigner;
        }
    }
}