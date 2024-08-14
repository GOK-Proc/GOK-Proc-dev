using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Rhythm;
using UnityEngine.UI;

namespace MusicSelection
{
    public class MusicSelectionManager : SingletonMonoBehaviour<MusicSelectionManager>
    {
        private EventSystem _eventSystem;
        private DifficultySelection _difficultySelection;

        [SerializeField] private Difficulty _firstSelectedDifficulty;

        [Header("参照")] [SerializeField] private BeatmapData _beatmapData;
        [SerializeField] private GameObject _uiElementParent;
        [SerializeField] private GameObject _musicUIElementPrefab;
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private Thumbnail _thumbnail;
        [SerializeField] private DifficultyDisplay _difficultyDisplay;

        protected override void Awake()
        {
            base.Awake();

            _eventSystem = GetComponent<EventSystem>();
            _difficultySelection = new DifficultySelection(_firstSelectedDifficulty);
            GenerateUIElements();
            InitScrollbar();
            UpdateDifficultyUI();
        }

        public void OnNavigateHorizontal(InputValue inputValue)
        {
            var inputHorizontal = inputValue.Get<Vector2>().x;
            UpdateDifficultySelection(inputHorizontal);
            UpdateDifficultyUI();
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

        private void UpdateDifficultyUI()
        {
            _difficultyDisplay.Set();
        }
    }
}