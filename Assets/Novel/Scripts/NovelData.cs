using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Novel
{
	[CreateAssetMenu]
	public class NovelData : ScriptableObject
	{
		public Dictionary<string, TextAsset> NovelDict => _novelScenarios.ToDictionary(x => x.name, x => x);

		[SerializeField] private TextAsset[] _novelScenarios;

#if UNITY_EDITOR
		private void OnValidate()
		{
			IdGenerator.GenerateNovelId(this);
		}
#endif
	}
}
