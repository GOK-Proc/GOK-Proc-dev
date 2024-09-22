using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Gallery;
using Transition;
using KanKikuchi.AudioManager;

namespace MusicSelection
{
    public class MusicUIElement : MonoBehaviour, ISubmitHandler, ICancelHandler, ISelectHandler,
        IDeselectHandler
    {
        private bool _isInited = false;

        private RhythmId _rhythmId;
        private TextMeshProUGUI _text;
        private TrackInformation _trackInfo;
        private Scrollbar _scrollbar;
        private ThumbnailBase _thumbnailBase;

        private const int NormalFontSize = 48;
        private const int FontSizeWhenSelected = 60;

        public void Init(TrackInformation info, Scrollbar scrollbar, ThumbnailBase thumbnailBase)
        {
            if (_isInited)
                throw new InvalidOperationException("This instance has already been initialized.");
            _isInited = true;

            _trackInfo = info;
            _rhythmId = info.HasBeatmap switch
            {
                true => (RhythmId)Enum.Parse(typeof(RhythmId), _trackInfo.Id),
                false => RhythmId.None
            };

            _scrollbar = scrollbar;

            _thumbnailBase = thumbnailBase;
            _text = GetComponent<TextMeshProUGUI>();
            _text.text = info.Title;
            _text.fontSize = NormalFontSize;
        }

        // ギャラリーでは必要ない．
        // InputSystemUIInputModuleのインスペクターでSubmitをNoneにすることで解決
        public void OnSubmit(BaseEventData _)
        {
            if (_rhythmId == RhythmId.None) return;

            SceneTransitionManager.TransitionToRhythm(_rhythmId, DifficultySelection.Current,
                false);
        }

        public void OnCancel(BaseEventData _)
        {
            SceneTransitionManager.TransitionToModeSelection();
        }

        public void OnSelect(BaseEventData _)
        {
            _text.fontSize = FontSizeWhenSelected;
            // TODO: ここでスクロールバー制御
            _thumbnailBase.Set(_trackInfo);

            BGMManager.Instance.Play(_trackInfo.Intro, _trackInfo.Sound);
        }

        public void OnDeselect(BaseEventData _)
        {
            _text.fontSize = NormalFontSize;
        }
    }
}