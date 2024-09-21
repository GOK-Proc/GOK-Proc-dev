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

        private void Start()
        {
            BGMManager.Instance.Play(BGMPath.MODE_INTRO, BGMPath.MODE_LOOP);

            _eventSystem.firstSelectedGameObject = SceneTransitionManager.RecentSceneName switch
            {
                SceneName.Title or SceneName.Map => _selectableBanners[0],
                SceneName.MusicSelection => _selectableBanners[1],
                SceneName.Gallery => _selectableBanners[2],
                _ => _selectableBanners[0]
            };
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