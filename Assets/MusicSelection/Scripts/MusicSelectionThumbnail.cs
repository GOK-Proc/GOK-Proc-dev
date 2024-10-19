using System;
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
        [SerializeField] private TextMeshProUGUI _levelText;

        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [SerializeField] private TextMeshProUGUI _maxComboText;

        [SerializeField] private TextMeshProUGUI _allPerfectText;
        [SerializeField] private TextMeshProUGUI _fullComboText;

        [SerializeField] private TextMeshProUGUI _clearText;

        [SerializeField] private RecordList _recordList;
        [SerializeField] private BeatmapData _beatmapData;

        public override void Set(TrackInformation track)
        {
            _track = track;
            _image.sprite = track.Thumbnail;
            _titleText.text = track.Title;
            _composerText.text = $"作曲：{track.Composer}";

            const float margin = 60f;
            _clearText.rectTransform.localPosition = _titleText.rectTransform.localPosition +
                                                     _titleText.preferredWidth * Vector3.right +
                                                     margin * Vector3.right ;

            if (track.HasBeatmap)
            {
                _notes = _beatmapData.BeatmapDictionary[track.Id].Notes;
                Set(DifficultySelection.Current);
            }
            else // チュートリアルの場合
            {
                _notes = Array.Empty<NotesInformation>();
                _bestScoreText.text = "- pt";
                _maxComboText.text = "- combo";
                _allPerfectText.gameObject.SetActive(false);
                _fullComboText.gameObject.SetActive(false);
                // チュートリアルの説明を表示するために作曲者欄を借りる
                _composerText.text = track.Description;
                _notesDesignerText.text = "";
            }
        }

        public void Set(Difficulty difficulty)
        {
            if (!_track.HasBeatmap) return;

            var record = _recordList[_track.Id][(int)difficulty];
            _bestScoreText.text = $"{record.Score:N0} pt";
            _maxComboText.text = $"{record.MaxCombo} combo";
            _allPerfectText.gameObject.SetActive(record.Achievement == Achievement.AllPerfect);
            _fullComboText.gameObject.SetActive(record.Achievement == Achievement.FullCombo);
            _clearText.gameObject.SetActive(record.IsCleared);

            var notes = _notes[(int)difficulty];
            _notesDesignerText.text = $"譜面：{notes.NotesDesigner}";
            var upperDifficulty = DifficultySelection.Current.ToString().ToUpper();
            _levelText.text = $"{upperDifficulty} Lv. {notes.Level}";
        }
    }
}