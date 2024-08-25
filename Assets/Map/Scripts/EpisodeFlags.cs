using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Map
{
	[CreateAssetMenu]
	public class EpisodeFlags : ScriptableObject
	{
#if UNITY_EDITOR
		private readonly string PATH = Path.Combine(Application.dataPath, "Map/EpisodeFlags.json");
#else
		private readonly string PATH = Path.Combine(Application.persistentDataPath, "EpisodeFlags.json");
#endif

		[SerializeField] private List<EpisodeFlagPair> _flagList;
		public List<EpisodeFlagPair> FlagList { get { return _flagList; } private set { _flagList = value; } }

		public Dictionary<(int, int), bool> FlagDict => FlagList.ToDictionary(x => x.Key, x => x.Value);

		private void OnEnable()
		{
			LoadJson();
		}

		public void SetFlag((int, int) episodeId, bool value)
		{
			if (!value) return;

			_flagList.Select(flag => flag.Key == episodeId ? new EpisodeFlagPair(episodeId, true) : flag).ToList();

			SaveJson();
		}

		private void SaveJson()
		{
			string json = JsonUtility.ToJson(this);
			using (var writer = new StreamWriter(PATH))
			{
				writer.Write(json);
			}
		}

		private void LoadJson()
		{
			if (!File.Exists(PATH))
			{
				SaveJson();
				return;
			}

			using (var reader = new StreamReader(PATH))
			{
				string json = reader.ReadToEnd();
				JsonUtility.FromJsonOverwrite(json, this);
			}
		}

#if UNITY_EDITOR
		public void ResetFlags(EpisodeData episodeData)
		{
			FlagList.Clear();
			foreach (var episode in episodeData.DataList)
			{
				FlagList.Add(new EpisodeFlagPair((episode.Chapter, episode.Section), false));
			}

			FlagList = FlagList.OrderBy(kvp => kvp.Key.Item1).ThenBy(kvp => kvp.Key.Item2).ToList();

			if (FlagList.Count > 0)
			{
				FlagList[0].Value = true;
			}

			SaveJson();
		}
#endif
	}
}