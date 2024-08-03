using UnityEngine;
using Rhythm;
using UnityEngine.EventSystems;

namespace MusicSelection
{
    public class MusicSelectionManager : SingletonMonoBehaviour<MusicSelectionManager>
    {
        private EventSystem _eventSystem;

        [SerializeField] private BeatmapData beatmapData;
        [SerializeField] private GameObject uiElementParent;
        [SerializeField] private GameObject musicUIElementPrefab;

        protected override void Awake()
        {
            base.Awake();

            _eventSystem = GetComponent<EventSystem>();
            GenerateUIElements();
        }

        private void GenerateUIElements()
        {
            var isFirst = true;
            foreach (var beatmapInfo in beatmapData.BeatmapDictionary.Values)
            {
                var element = Instantiate(musicUIElementPrefab, uiElementParent.transform)
                    .GetComponent<MusicUIElement>();
                element.Init(beatmapInfo);

                // TODO: UI要素の配置
                // 配置が決定していない + 曲一覧データがまだないため，後回し
                // Grid Layout Groupが使えるか？

                if (!isFirst) continue;
                _eventSystem.firstSelectedGameObject = element.gameObject;
                isFirst = false;
            }
        }
    }
}