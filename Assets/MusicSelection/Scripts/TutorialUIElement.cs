using UnityEngine.EventSystems;
using UnityEngine.UI;
using Gallery;
using Transition;

namespace MusicSelection
{
    public class TutorialUIElement : MusicUIElement
    {
        public override void Init(TrackInformation info, Scrollbar scrollbar,
            ThumbnailBase thumbnailBase)
        {
            base.Init(info, scrollbar, thumbnailBase);

            _rhythmId = RhythmId.Chapter1_2;
        }

        public override void OnSubmit(BaseEventData _)
        {
            if (_rhythmId == RhythmId.None) return;

            StopCoroutine(_bgmSwitchCor);

            SceneTransitionManager.TransitionToRhythmTutorial();
        }

        public override void OnSelect(BaseEventData _)
        {
            base.OnSelect(_);

            DifficultySelection.SetActive(false);
        }

        public override void OnDeselect(BaseEventData _)
        {
            base.OnDeselect(_);

            DifficultySelection.SetActive(true);
        }
    }
}