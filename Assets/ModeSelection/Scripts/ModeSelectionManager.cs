using UnityEngine;
using Transition;

namespace ModeSelection
{
    public class ModeSelectionManager: MonoBehaviour
    {
        public void ToMap()
        {
            SceneTransitionManager.TransitionToMap();
        }
        
        public void ToMusicSelection()
        {
            Debug.Log("ここで楽曲選択シーンへ遷移します");
            // TODO: 
            // SceneTransitionManager.TransitionToSelection();
        }

        public void ToGallery()
        {
            Debug.Log("ギャラリーシーンへ遷移します");
            // TODO: 
            // SceneTransitionManager.TransitionToGallery();
        }
    }
}