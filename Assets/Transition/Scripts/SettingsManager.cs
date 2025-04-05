using KanKikuchi.AudioManager;
using UnityEngine;
using Settings;

namespace Transition
{
	public class SettingsManager : MonoBehaviour
	{
		[SerializeField] private UserSettings _userSettings;

		public void SetDefaultSeVolume()
		{
			SEManager.Instance.ChangeBaseVolume(_userSettings.SoundEffectVolume / 10f);
		}
		
		public void SetNovelSeVolume()
		{
			SEManager.Instance.ChangeBaseVolume(_userSettings.NovelEffectVolume / 10f);
		}
	}
}