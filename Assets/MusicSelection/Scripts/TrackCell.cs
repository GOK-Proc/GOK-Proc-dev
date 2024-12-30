using System;
using System.Collections;
using FancyScrollView;
using Gallery;
using KanKikuchi.AudioManager;
using TMPro;
using Transition;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MusicSelection
{
    public class TrackCell : FancyScrollRectCell<TrackInformation, TrackContext>,
        ISubmitHandler, ICancelHandler, ISelectHandler, IDeselectHandler
    {
        private TrackInformation _trackInfo;
        private Coroutine _bgmSwitchCor;

        private RhythmId RhythmId => _trackInfo.HasBeatmap switch
        {
            true => (RhythmId)Enum.Parse(typeof(RhythmId), _trackInfo.Id),
            false => RhythmId.None
        };

        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Selectable _selectable;

        public override void UpdateContent(TrackInformation trackInfo)
        {
            _trackInfo = trackInfo;
            _title.text = trackInfo.Title;

            var selected = Context.SelectedIndex == Index;
            _title.fontSize = selected ? 60f : 48f;

            if (!selected) return;
            _selectable.Select();
        }

        public void OnSubmit(BaseEventData _)
        {
            if (RhythmId == RhythmId.None) return;

            StopCoroutine(_bgmSwitchCor);
            SceneTransitionManager.TransitionToRhythm(RhythmId, DifficultySelection.Current);
        }

        public void OnCancel(BaseEventData _)
        {
            SceneTransitionManager.TransitionToModeSelection();
        }

        public void OnSelect(BaseEventData _)
        {
            Context.OnCellSelected?.Invoke(Index);
            UpdateContent(_trackInfo);
            _bgmSwitchCor = StartCoroutine(SwitchBGMIfNeeded());

            if (_trackInfo.Id == "Tutorial")
            {
                DifficultySelection.SetActive(false);
            }
        }

        public void OnDeselect(BaseEventData _)
        {
            UpdateContent(_trackInfo);

            if (_bgmSwitchCor == null) return;
            StopCoroutine(_bgmSwitchCor);

            if (_trackInfo.Id == "Tutorial")
            {
                DifficultySelection.SetActive(true);
            }
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