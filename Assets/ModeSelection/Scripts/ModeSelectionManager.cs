using UnityEngine;
using KanKikuchi.AudioManager;
using Transition;

namespace ModeSelection
{
    public class ModeSelectionManager : MonoBehaviour
    {
        private void Start()
        {
            BGMManager.Instance.Play(BGMPath.MODE_INTRO, BGMPath.MODE_LOOP);
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