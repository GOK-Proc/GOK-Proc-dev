using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Rhythm;

namespace MusicSelection
{
	public class DifficultySelection
	{
		private static bool _isActive = true;

		private static readonly Dictionary<Difficulty, Color> Colors = new()
		{
			{ Difficulty.Easy, new Color(0.73f, 1.00f, 0.78f) },
			{ Difficulty.Hard, new Color(1.00f, 0.73f, 0.73f) },
			{ Difficulty.Expert, new Color(0.87f, 0.73f, 1.00f) }
		};

		public static Difficulty Current { get; private set; }
		public static bool CurrentIsVs { get; private set; }
		public static Color CurrentColor => Colors[Current];
		public static bool IsEasiest => State == 0;
		public static bool IsHardest => State == 5;

		private static int _state = 0;
		private static int State
		{
			get { return _state; }
			set
			{
				if (value < 0) _state = 0;
				else if (value > 5) _state = 5;
				else _state = value;
			}
		}
		private List<(Difficulty, bool)> States = new()
		{
			(Difficulty.Easy, false),
			(Difficulty.Hard, false),
			(Difficulty.Expert, false),
			(Difficulty.Easy, true),
			(Difficulty.Hard, true),
			(Difficulty.Expert, true),
		};

		public DifficultySelection(Difficulty difficulty)
		{
			Current = difficulty;
		}

		public static void SetActive(bool active)
		{
			_isActive = active;
			if (active)
			{
				DifficultyDisplay.Show();
			}
			else
			{
				DifficultyDisplay.Hide();
			}
		}

		public void SelectNextHarder()
		{
			if (!_isActive) return;
			if (IsHardest) return;

			SystemSoundEffect.PlaySelect();
			State++;
			Current = States[State].Item1;
			CurrentIsVs = States[State].Item2;
		}

		public void SelectNextEasier()
		{
			if (!_isActive) return;
			if (IsEasiest) return;

			SystemSoundEffect.PlaySelect();
			State--;
			Current = States[State].Item1;
			CurrentIsVs = States[State].Item2;
		}
	}
}