using System.Collections;
using Transition;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Settings
{
	public class FirstObjectSelector : MonoBehaviour
	{
		[SerializeField] private TabLabel _left;
		[SerializeField] private TabLabel _center;
		[SerializeField] private AdjustOffset _adjust;

		private void Start()
		{
			StartCoroutine(Initialize());
		}

		IEnumerator Initialize()
		{
			yield return null;

			if (SceneTransitionManager.RecentSceneName == SceneName.Adjustment)
			{
				_center.OnSelect(new BaseEventData(EventSystem.current));
				EventSystem.current.SetSelectedGameObject(_adjust.gameObject);
			}
			else EventSystem.current.SetSelectedGameObject(_left.gameObject);
		}
	}
}