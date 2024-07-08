using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

[CreateAssetMenu]
public class EpisodeFlags : ScriptableObject
{
	[SerializeField] private List<EpisodeFlagPair> _flagsList;
	public List<EpisodeFlagPair> FlagsList { get { return _flagsList; } private set { _flagsList = value; } }

	private string _path;

	private void OnEnable()
	{
#if UNITY_EDITOR
		_path = Path.Combine(Application.dataPath, "Map/EpisodeFlags.json");
#else
		_path = Path.Combine(Application.persistentDataPath, "EpisodeFlags.json");
#endif

		LoadJson();
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

	private void SaveJson()
	{
		string json = JsonUtility.ToJson(this);
		using (var writer = new StreamWriter(_path))
		{
			writer.Write(json);
		}
	}

	private void LoadJson()
	{
		if (!File.Exists(_path))
		{
			SaveJson();
			return;
		}

		using (var reader = new StreamReader(_path))
		{
			string json = reader.ReadToEnd();
			JsonUtility.FromJsonOverwrite(json, this);
		}
	}
}