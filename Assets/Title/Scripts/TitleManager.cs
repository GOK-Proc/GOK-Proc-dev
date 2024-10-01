using UnityEngine;
using Transition;

namespace Title
{
    public class TitleManager : MonoBehaviour
    {
        private void Start()
        {
            // TODO: BGM再生
            // BGMManager.Instance.Play("titleBGMPath");
        }

        public void ToModeSelection()
        {
            SceneTransitionManager.TransitionToModeSelection();
        }
    }
}