using UnityEngine;
using KanKikuchi.AudioManager;
using Transition;

namespace ModeSelection
{
    public class ModeSelectionManager : MonoBehaviour
    {
        private void Start()
        {
            // TODO: BGM再生
            // BGMManager.Instance.Play("path");
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