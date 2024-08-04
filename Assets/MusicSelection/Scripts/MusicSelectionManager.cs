using UnityEngine;
using Rhythm;
using UnityEngine.EventSystems;

namespace MusicSelection
{
    public class MusicSelectionManager : SingletonMonoBehaviour<MusicSelectionManager>
    {
        private EventSystem _eventSystem;

        [SerializeField] private BeatmapData _beatmapData;
        [SerializeField] private GameObject _uiElementParent;
        [SerializeField] private GameObject _musicUIElementPrefab;
        [SerializeField] private Thumbnail _thumbnail;

        protected override void Awake()
        {
            base.Awake();

            _eventSystem = GetComponent<EventSystem>();
            GenerateUIElements();
        }

        private void GenerateUIElements()
        {
            var isFirst = true;
            var posY = 0f;
            foreach (var beatmapInfo in _beatmapData.BeatmapDictionary.Values)
            {
                var element = Instantiate(_musicUIElementPrefab, _uiElementParent.transform)
                    .GetComponent<MusicUIElement>();
                element.Init(beatmapInfo, _thumbnail);

                var rectTransform = element.gameObject.GetComponent<RectTransform>();
                var pos = rectTransform.localPosition;
                pos.y = posY;
                rectTransform.localPosition = pos;
                posY += 100f;

                if (!isFirst) continue;
                _eventSystem.firstSelectedGameObject = element.gameObject;
                isFirst = false;
            }
        }
    }
}