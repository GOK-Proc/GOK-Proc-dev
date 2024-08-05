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

		[SerializeField] private List<EpisodeFlagPair> _flagsList;
		public List<EpisodeFlagPair> FlagsList { get { return _flagsList; } private set { _flagsList = value; } }

		private Dictionary<(int, int), bool> _flagsDict;
		public Dictionary<(int, int), bool> FlagsDict
		{
			get
			{
				if(FlagsDict == null) InitializeDictionary();
				
				return _flagsDict;
			}

			private set
			{
				_flagsDict = value;
			}
		}

		private void OnEnable()
		{
			LoadJson();
		}

		private void InitializeDictionary()
		{
			FlagsDict = new Dictionary<(int, int), bool>();

			foreach(var fp in FlagsList)
			{
				if(!FlagsDict.ContainsKey(fp.Key))
				{
					FlagsDict.Add(fp.Key, fp.Value);
				}
			}
		}

		public void SetFlag((int, int) episodeId, bool value)
		{
			if (!value) return;

			_flagsList.Select(flag => flag.Key == episodeId ? new EpisodeFlagPair(episodeId, true) : flag).ToList();

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
			FlagsList.Clear();
			foreach (var episode in episodeData.DataList)
			{
				FlagsList.Add(new EpisodeFlagPair((episode.Chapter, episode.Section), false));
			}

			FlagsList = FlagsList.OrderBy(kvp => kvp.Key.Item1).ThenBy(kvp => kvp.Key.Item2).ToList();

			SaveJson();
		}
#endif
	}
}