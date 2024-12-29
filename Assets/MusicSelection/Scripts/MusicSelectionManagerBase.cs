using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Gallery;
using Transition;

namespace MusicSelection
{
    public abstract class
        MusicSelectionManagerBase : SingletonMonoBehaviour<MusicSelectionManagerBase>
    {
        protected Dictionary<string, TrackInformation> _trackDict;

        private List<TrackInformation> TrackList => _trackDict.Values.ToList();

        [Header("参照")] [SerializeField] protected TrackData _trackData;
        [SerializeField] private GameObject _uiElementParent;
        [SerializeField] private GameObject _musicUIElementPrefab;
        [SerializeField] private GameObject _tutorialUIElementPrefab;
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] protected ThumbnailBase _thumbnailBase;
        [SerializeField] private TrackScrollRect _trackScrollRect;

        protected override void Awake()
        {
            base.Awake();

            _trackDict = _trackData.TrackDictionary;
        }

        protected virtual void Start()
        {
            InitializeScrollRect();
        }

        private void InitializeScrollRect()
        {
            _trackScrollRect.OnSelectionChanged(index => _thumbnailBase.Set(TrackList[index]));

            var tracks = _trackDict.Values.ToArray();
            _trackScrollRect.UpdateData(tracks);

            var currentTrackIndex = 0;
            if (SceneTransitionManager.CurrentRhythmId != RhythmId.None)
            {
                // 直前に遊んだ曲番号を取得
                currentTrackIndex = _trackDict.Keys.ToList()
                    .IndexOf(SceneTransitionManager.CurrentRhythmId.ToString());
            }
            else if (SceneTransitionManager.CurrentTutorialId == TutorialId.Rhythm)
            {
                currentTrackIndex = _trackDict.Keys.ToList().IndexOf("Tutorial");
            }

            _trackScrollRect.JumpTo(currentTrackIndex);
        }
    }
}