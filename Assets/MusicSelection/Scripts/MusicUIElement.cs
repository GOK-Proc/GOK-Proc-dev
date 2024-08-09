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
        private BeatmapInformation _beatmapInfo;
        private Thumbnail _thumbnail;

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
            GetComponent<TextMeshProUGUI>().text = info.Title;
        }

        public void OnSubmit()
        {
            // TODO:
            // issue#65 正しい難易度を渡す
            SceneTransitionManager.TransitionToRhythm(_rhythmId, Difficulty.Undefined, false);
        }

        public void OnCancel()
        {
            // モードセレクトSceneへ
            // SceneTransitionManager.TransitionTo();
            Debug.Log("OnCancel()が実行されました");
        }

        public void OnSelect()
        {
            _thumbnail.Set(null, _beatmapInfo);
        }
    }
}