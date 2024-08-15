using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Map
{
	[Serializable]
	public struct EpisodeNumber
	{
		[SerializeField] private int _chapter;
		public int Chapter { get { return _chapter; } set { _chapter = value; } }

		[SerializeField] private int _section;
		public int Section { get { return _section; } set { _section = value; } }
	}
}