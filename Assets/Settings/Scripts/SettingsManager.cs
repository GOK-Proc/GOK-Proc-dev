using KanKikuchi.AudioManager;
using UnityEngine;

namespace Settings
{
	public class SettingsManager : MonoBehaviour
	{
		private void Start()
		{
			BGMManager.Instance.Play(BGMPath.SETTING);
		}
	}
}