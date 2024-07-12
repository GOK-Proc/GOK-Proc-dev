using System;
using UnityEngine;

namespace Map
{
	[Serializable]
	public class EpisodeFlagPair
	{
		[SerializeField] private int _chapter;
		[SerializeField] private int _section;

		public (int, int) Key
		{
			get { return (_chapter, _section); }
			private set
			{
				_chapter = value.Item1;
				_section = value.Item2;
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