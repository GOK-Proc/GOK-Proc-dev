using UnityEngine;
using KanKikuchi.AudioManager;

namespace Map
{
	class BGMPlayer : MonoBehaviour
	{
		private void Start()
		{
			BGMManager.Instance.Play(BGMPath.MAP_INTRO, BGMPath.MAP_LOOP);
		}
	}
}