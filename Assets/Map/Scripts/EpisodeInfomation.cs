using System;
using UnityEngine;

namespace Map
{
	[Serializable]
	public struct EpisodeInfomation
	{
		[SerializeField] private int _chapter;
		public int Chapter { get { return _chapter; } }

		[SerializeField] private int _section;
		public int Section { get { return _section; } }

		[SerializeField] private string _title;
		public string Title { get { return _title; } }

		[SerializeField] private EpisodeType _episodeType;
		public EpisodeType EpisodeType { get { return _episodeType; } }

		[SerializeField] private NovelId _novelId;
		public NovelId NovelId { get { return _novelId; } }

		[SerializeField] private RhythmId _rhythmId;
		public RhythmId RhythmId { get { return _rhythmId; } }
	}
}