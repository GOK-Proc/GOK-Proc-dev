using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Rhythm;

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
        [SerializeField] private Thumbnail _thumbnail;
        [SerializeField] private GameObject _difficultyCursor;

        protected override void Awake()
        {
            base.Awake();

            _eventSystem = GetComponent<EventSystem>();
            _difficultySelection = new DifficultySelection(_firstSelectedDifficulty);
            GenerateUIElements();
            UpdateDifficultyCursor();
        }

        public void OnNavigateHorizontal(InputValue inputValue)
        {
            var inputHorizontal = inputValue.Get<Vector2>().x;
            UpdateDifficultySelection(inputHorizontal);
        }

        private void GenerateUIElements()
        {
            var isFirst = true;
            var posY = 400f;
            var height = _musicUIElementPrefab.GetComponent<RectTransform>().sizeDelta.y;

            foreach (var beatmapInfo in _beatmapData.BeatmapDictionary.Values)
            {
                var element = Instantiate(_musicUIElementPrefab, _uiElementParent.transform)
                    .GetComponent<MusicUIElement>();
                element.Init(beatmapInfo, _thumbnail);

                var rectTransform = element.gameObject.GetComponent<RectTransform>();
                var pos = rectTransform.localPosition;
                pos.y = posY;
                rectTransform.localPosition = pos;

                posY -= height;

                if (!isFirst) continue;
                _eventSystem.firstSelectedGameObject = element.gameObject;
                isFirst = false;
            }
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

            UpdateDifficultyCursor();
        }

        private void UpdateDifficultyCursor()
        {
            var x = 300f * ((float)DifficultySelection.Current - 1f);
            _difficultyCursor.transform.localPosition = new Vector2(x, 0f);
        }
    }
}