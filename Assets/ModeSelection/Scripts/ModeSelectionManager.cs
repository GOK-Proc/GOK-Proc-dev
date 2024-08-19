using UnityEngine;
using Transition;

namespace ModeSelection
{
    public class ModeSelectionManager : MonoBehaviour
    {
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