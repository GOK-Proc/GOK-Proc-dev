using UnityEngine;
using UnityEngine.EventSystems;
using KanKikuchi.AudioManager;
using Transition;

namespace ModeSelection
{
    public class ModeSelectionManager : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private GameObject[] _selectableBanners;

        private void Awake()
        {
            _eventSystem.firstSelectedGameObject = SceneTransitionManager.RecentSceneName switch
            {
                SceneName.Title or SceneName.Map => _selectableBanners[0],
                SceneName.MusicSelection => _selectableBanners[1],
                SceneName.Gallery => _selectableBanners[2],
                _ => _selectableBanners[0]
            };
        }

        private void Start()
        {
            BGMManager.Instance.Play(BGMPath.MODE_INTRO, BGMPath.MODE_LOOP);
        }

        public void ToTitle()
        {
            SceneTransitionManager.TransitionToTitle();
        }

        public void ToMap()
        {
            SceneTransitionManager.TransitionToMap();
        }

        public void ToMusicSelection()
        {
            SceneTransitionManager.TransitionToMusicSelection();
        }

        public void ToGallery()
        {
            SceneTransitionManager.TransitionToGallery();
        }
    }
}