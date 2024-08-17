using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Gallery;
using Transition;

namespace MusicSelection
{
    public class MusicUIElement : MonoBehaviour
    {
        private bool _isInited = false;

        private RhythmId _rhythmId;
        private TextMeshProUGUI _text;
        private TrackInformation _trackInfo;
        private Scrollbar _scrollbar;
        private Thumbnail _thumbnail;

        private const int NormalFontSize = 48;
        private const int FontSizeWhenSelected = 60;

        public void Init(TrackInformation info, Scrollbar scrollbar, Thumbnail thumbnail)
        {
            if (_isInited)
                throw new InvalidOperationException("This instance has already been initialized.");
            _isInited = true;

            _trackInfo = info;
            _rhythmId = (RhythmId)Enum.Parse(typeof(RhythmId), _trackInfo.Id);

            _scrollbar = scrollbar;

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
            // TODO: ここでスクロールバー制御
            _thumbnail.Set(null, _trackInfo);
            // TODO: ここで対応する楽曲を再生
        }

        public void OnDeselect()
        {
            _text.fontSize = NormalFontSize;
            // TODO: ここで対応する楽曲を停止
        }
    }
}