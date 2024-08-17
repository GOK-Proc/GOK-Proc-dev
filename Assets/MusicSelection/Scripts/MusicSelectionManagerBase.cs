using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Rhythm;
using UnityEngine.UI;

namespace MusicSelection
{
    public class MusicSelectionManagerBase : SingletonMonoBehaviour<MusicSelectionManagerBase>
    {
        private EventSystem _eventSystem;

        [Header("参照")] [SerializeField] private BeatmapData _beatmapData;
        [SerializeField] private GameObject _uiElementParent;
        [SerializeField] private GameObject _musicUIElementPrefab;
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private Thumbnail _thumbnail;

        protected override void Awake()
        {
            base.Awake();

            _eventSystem = GetComponent<EventSystem>();
            GenerateUIElements();
            InitScrollbar();
        }

        private void GenerateUIElements()
        {
            var isFirst = true;
            var posY = 0f;
            var height = _musicUIElementPrefab.GetComponent<RectTransform>().rect.height;

            foreach (var beatmapInfo in _beatmapData.BeatmapDictionary.Values)
            {
                var element = Instantiate(_musicUIElementPrefab, _uiElementParent.transform)
                    .GetComponent<MusicUIElement>();
                element.Init(beatmapInfo, _scrollbar, _thumbnail);

                var rectTransform = element.gameObject.GetComponent<RectTransform>();
                var pos = rectTransform.anchoredPosition;
                pos.y = posY;
                rectTransform.anchoredPosition = pos;

                posY -= height;

                if (!isFirst) continue;
                _eventSystem.firstSelectedGameObject = element.gameObject;
                isFirst = false;
            }
        }

        // REVIEW: sizeを0.xにしないといけない条件（全楽曲>画面内楽曲）で試していないため意図した挙動にならない可能性あり．
        // 体験版時点ではリズムゲームが6曲予定のため余裕で画面内に収まるため，スクロールバーがそもそも必要ない．
        private void InitScrollbar()
        {
            // すべての楽曲数
            var allMusicNum = (float)_beatmapData.BeatmapDictionary.Count;
            // 画面内に収まる楽曲の数（小数考慮）
            var musicInScreenNum = Screen.height /
                                   _musicUIElementPrefab.GetComponent<RectTransform>().rect.height;

            _scrollbar.size = musicInScreenNum / allMusicNum;
        }
    }
}