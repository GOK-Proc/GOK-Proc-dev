using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Gallery;
using Transition;

namespace MusicSelection
{
    public abstract class
        MusicSelectionManagerBase : SingletonMonoBehaviour<MusicSelectionManagerBase>
    {
        private EventSystem _eventSystem;
        protected Dictionary<string, TrackInformation> TrackDict;

        [Header("参照")] [SerializeField] protected TrackData _trackData;
        [SerializeField] private GameObject _uiElementParent;
        [SerializeField] private GameObject _musicUIElementPrefab;
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] protected ThumbnailBase _thumbnailBase;

        protected override void Awake()
        {
            base.Awake();

            TrackDict = _trackData.TrackDictionary;
            _eventSystem = GetComponent<EventSystem>();
        }

        protected virtual void Start()
        {
            GenerateUIElements();
            InitScrollbar();
        }

        private void GenerateUIElements()
        {
            var posY = 0f;
            var height = _musicUIElementPrefab.GetComponent<RectTransform>().rect.height;

            foreach (var trackInformation in TrackDict.Values)
            {
                var element = Instantiate(_musicUIElementPrefab, _uiElementParent.transform)
                    .GetComponent<MusicUIElement>();
                element.Init(trackInformation, _scrollbar, _thumbnailBase);

                var rectTransform = element.gameObject.GetComponent<RectTransform>();
                var pos = rectTransform.anchoredPosition;
                pos.y = posY;
                rectTransform.anchoredPosition = pos;

                posY -= height;

                // OPTIMIZE:
                // Enum.ToString()の実装速度が比較的遅いためボトルネックになっている可能性あり．
                // 現状気になるほどの遅延は見られないのでこの実装でいく．
                if (_eventSystem.firstSelectedGameObject == null || 
                    trackInformation.Id == SceneTransitionManager.CurrentRhythmId.ToString())
                {
                    _eventSystem.firstSelectedGameObject = element.gameObject;
                }
            }
        }

        // REVIEW: sizeを0.xにしないといけない条件（全楽曲>画面内楽曲）で試していないため意図した挙動にならない可能性あり．
        // 体験版時点ではリズムゲームが6曲予定のため余裕で画面内に収まるため，スクロールバーがそもそも必要ない．
        private void InitScrollbar()
        {
            // すべての楽曲数
            var allMusicNum = (float)TrackDict.Count;
            // 画面内に収まる楽曲の数（小数考慮）
            var musicInScreenNum = Screen.height /
                                   _musicUIElementPrefab.GetComponent<RectTransform>().rect.height;

            _scrollbar.size = musicInScreenNum / allMusicNum;
        }
    }
}