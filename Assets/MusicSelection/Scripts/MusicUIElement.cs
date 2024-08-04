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

        private static GameObject _prefab;
        private static GameObject _parent;

        public void Init(BeatmapInformation info)
        {
            _beatmapInfo = info;
            _rhythmId = (RhythmId)Enum.Parse(typeof(RhythmId), _beatmapInfo.Id);
            GetComponent<TextMeshPro>().text = info.Title;
        }

        public void OnSubmit()
        {
            SceneTransitionManager.TransitionToRhythm(_rhythmId, false);
        }

        public void OnCancel()
        {
            // モードセレクトSceneへ
            // SceneTransitionManager.TransitionTo();
            Debug.Log("OnCancel()が実行されました");
        }
    }
}