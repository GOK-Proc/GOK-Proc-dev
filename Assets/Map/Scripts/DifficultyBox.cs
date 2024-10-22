using Rhythm;
using Transition;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Map
{
	class DifficultyBox : Selectable, ISubmitHandler, ICancelHandler
	{
		[SerializeField] private DifficultySelector _selector;
		[SerializeField] private Difficulty _difficulty;

		public void OnSubmit(BaseEventData eventData)
		{
			_selector.SetMapDifficulty(_difficulty);
		}

		public void OnCancel(BaseEventData eventData)
		{
			SceneTransitionManager.TransitionToModeSelection();
		}
	}
}