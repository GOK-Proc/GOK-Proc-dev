using System;
using UnityEngine;

namespace Map
{
	[Serializable]
	public class EpisodeFlagPair
	{
		[SerializeField] private EpisodeNumber _episodeNumber;

		public (int, int) Key
		{
			get { return (_episodeNumber.Chapter, _episodeNumber.Section); }
			private set
			{
				_episodeNumber.Chapter = value.Item1;
				_episodeNumber.Section = value.Item2;
			}
		}

		[SerializeField] private bool _value;
		public bool Value { get { return _value; } set { _value = value; } }

		public EpisodeFlagPair((int, int) key, bool value)
		{
			Key = key;
			Value = value;
		}
	}
}