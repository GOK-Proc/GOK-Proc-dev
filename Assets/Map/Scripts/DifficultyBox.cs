using Rhythm;
using Settings;
using Transition;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Map
{
	class DifficultyBox : Selectable, ISubmitHandler, ICancelHandler
	{
		[SerializeField] private DifficultySelector _selector;
		[SerializeField] private ScenarioDifficulty _difficulty;

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