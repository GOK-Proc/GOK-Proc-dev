﻿using System.Linq;
using Gallery;
using UnityEngine;
using UnityEngine.InputSystem;
using Rhythm;
using Transition;

namespace MusicSelection
{
    /// <summary>
    /// リズムゲームで遊ぶ楽曲の選択画面マネージャー
    /// </summary>
    public class MusicSelectionManager : MusicSelectionManagerBase
    {
        private DifficultySelection _difficultySelection;

        [SerializeField] private TrackInformation _tutorialTrackInformation;

        [Header("難易度選択関連")] [SerializeField] private Difficulty _firstSelectedDifficulty;
        [SerializeField] private DifficultyDisplay _difficultyDisplay;

        protected override void Awake()
        {
            base.Awake();

            _trackDict = _trackData.TrackDictionary.Where(x => x.Value.HasBeatmap)
                .ToDictionary(x => x.Key, x => x.Value);
            _trackDict.Add(_tutorialTrackInformation.Id, _tutorialTrackInformation);

            _difficultySelection =
                new DifficultySelection(SceneTransitionManager.CurrentDifficulty);
        }

        protected override void Start()
        {
            base.Start();

            _difficultyDisplay.Set();
        }

        public void OnNavigateHorizontal(InputValue inputValue)
        {
            var inputHorizontal = inputValue.Get<Vector2>().x;
            UpdateDifficultySelection(inputHorizontal);
            UpdateDifficultyRelatedUI();
        }

        private void UpdateDifficultySelection(float input)
        {
            switch (input)
            {
                case > 0f:
                    _difficultySelection.SelectNextHarder();
                    break;
                case < 0f:
                    _difficultySelection.SelectNextEasier();
                    break;
            }
        }

        private void UpdateDifficultyRelatedUI()
        {
            var msThumbnail = (MusicSelectionThumbnail)_thumbnailBase;
            msThumbnail.Set(DifficultySelection.Current);
            _difficultyDisplay.Set();
        }
    }
}