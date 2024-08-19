using UnityEngine;
using KanKikuchi.AudioManager;
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
            // TODO: BGM停止
            // BGMManager.Instance.Stop();

            // TODO: モードセレクトへ遷移
            // SceneTransitionManager.TransitionTo();
        }
    }
}