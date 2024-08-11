using System;
using Rhythm;
using TMPro;
using UnityEngine;
using Transition;

namespace MusicSelection
{
    public class MusicUIElement : MonoBehaviour
    {
        private RhythmId _rhythmId;
        private TextMeshProUGUI _text;
        private BeatmapInformation _beatmapInfo;
        private Thumbnail _thumbnail;

        private const int NormalFontSize = 48;
        private const int FontSizeWhenSelected = 60;

        public void Init(BeatmapInformation info, Thumbnail thumbnail)
        {
            _beatmapInfo = info;
            try
            {
                // TODO:
                // issue#59 RhythmIdの自動生成ができていないとArgumentException
                _rhythmId = (RhythmId)Enum.Parse(typeof(RhythmId), _beatmapInfo.Id);
            }
            catch
            {
                // Do nothing
            }

            _thumbnail = thumbnail;
            _text = GetComponent<TextMeshProUGUI>();
            _text.text = info.Title;
            _text.fontSize = NormalFontSize;
        }

        public void OnSubmit()
        {
            SceneTransitionManager.TransitionToRhythm(_rhythmId, DifficultySelection.Current,
                false);
        }

        public void OnCancel()
        {
            // モードセレクトSceneへ
            // SceneTransitionManager.TransitionTo();
            Debug.Log("OnCancel()が実行されました");
        }

        public void OnSelect()
        {
            _text.fontSize = FontSizeWhenSelected;
            _thumbnail.Set(null, _beatmapInfo);
            // TODO: ここで対応する楽曲を再生
        }

        public void OnDeselect()
        {
            _text.fontSize = NormalFontSize;
            // TODO: ここで対応する楽曲を停止
        }
    }
}