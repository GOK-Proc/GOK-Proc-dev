using System;
using System.Collections;
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
        private bool _isInitialized = false;

        protected RhythmId _rhythmId;
        private TextMeshProUGUI _text;
        private TrackInformation _trackInfo;
        private Scrollbar _scrollbar;
        private ThumbnailBase _thumbnailBase;

        protected Coroutine _bgmSwitchCor;

        private const int NormalFontSize = 48;
        private const int FontSizeWhenSelected = 60;

        public virtual void Init(TrackInformation info, Scrollbar scrollbar,
            ThumbnailBase thumbnailBase)
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException("This instance has already been initialized.");
            }

            _isInitialized = true;

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
        public virtual void OnSubmit(BaseEventData _)
        {
            if (_rhythmId == RhythmId.None) return;

            StopCoroutine(_bgmSwitchCor);
            SceneTransitionManager.TransitionToRhythm(_rhythmId, DifficultySelection.Current);
        }

        public void OnCancel(BaseEventData _)
        {
            SceneTransitionManager.TransitionToModeSelection();
        }

        public virtual void OnSelect(BaseEventData _)
        {
            _text.fontSize = FontSizeWhenSelected;
            // TODO: ここでスクロールバー制御
            _thumbnailBase.Set(_trackInfo);

            _bgmSwitchCor = StartCoroutine(SwitchBGMIfNeeded());
        }

        public virtual void OnDeselect(BaseEventData _)
        {
            _text.fontSize = NormalFontSize;

            StopCoroutine(_bgmSwitchCor);
        }

        private IEnumerator SwitchBGMIfNeeded()
        {
            // この曲が選択されてから，一定時間以上選択されたままになっているか？
            // Yes -> 曲を切り替える
            // No  -> 「通り道」で選択されただけとみなして曲はそのまま

            // InputSystemUIInputModule.MoveRepeatDelayに合わせて0.5秒待つ
            yield return new WaitForSeconds(0.5f);

            BGMManager.Instance.FadeOut(0.3f);
            BGMManager.Instance.Play(_trackInfo.Intro, _trackInfo.Sound,
                delay: 0.5f, allowsDuplicate: true);
        }
    }
}