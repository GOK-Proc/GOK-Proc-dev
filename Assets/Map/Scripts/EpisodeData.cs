using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EpisodeData : ScriptableObject
{
	[SerializeField] private List<EpisodeInfomation> _dataList;
	public List<EpisodeInfomation> DataList {  get { return _dataList; } }

#if UNITY_EDITOR
	private HashSet<(int, int)> _episodeNumbers = new HashSet<(int, int)> ();

	private void OnValidate()
	{
		_episodeNumbers.Clear();
		foreach (var item in DataList)
		{
			if (_episodeNumbers.Contains((item.Chapter, item.Section)))
			{
				Debug.LogWarning("エピソード番号に重複があります");
			}
			else
			{
				_episodeNumbers.Add((item.Chapter, item.Section));
			}
		}
	}
#endif
}