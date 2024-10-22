using UnityEngine;
using Transition;
using KanKikuchi.AudioManager;

namespace Title
{
    public class TitleManager : MonoBehaviour
    {
        private void Start()
        {
            BGMManager.Instance.Play(BGMPath.MAIN_THEME);
        }

        public void ToModeSelection()
        {
            SceneTransitionManager.TransitionToModeSelection();
        }
    }
}