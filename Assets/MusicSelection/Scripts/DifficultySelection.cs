using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Rhythm;

namespace MusicSelection
{
    public class DifficultySelection
    {
        private static readonly Dictionary<Difficulty, Color> Colors = new()
        {
            // Ryotan案
            { Difficulty.Easy, new Color(0.73f, 1.00f, 0.78f) },
            { Difficulty.Hard, new Color(1.00f, 0.73f, 0.73f) },
            { Difficulty.Expert, new Color(0.87f, 0.73f, 1.00f) },
            
            // 社会のGOMI案
            // { Difficulty.Easy, new Color(0.00f, 0.69f, 0.41f) },
            // { Difficulty.Hard, new Color(1.00f, 0.29f, 0.00f) },
            // { Difficulty.Expert, new Color(0.60f, 0.00f, 0.60f) }
        };

        public static Difficulty Current { get; private set; }
        public static Color CurrentColor => Colors[Current];
        public static bool IsEasiest => Current == Difficulty.Easy;
        public static bool IsHardest => Current == Difficulty.Expert;

        public DifficultySelection(Difficulty difficulty)
        {
            Current = difficulty;
        }

        public void SelectNextHarder()
        {
            Current = Current switch
            {
                Difficulty.Easy => Difficulty.Hard,
                Difficulty.Hard => Difficulty.Expert,
                Difficulty.Expert => Difficulty.Expert,
                _ => throw new InvalidEnumArgumentException()
            };
        }

        public void SelectNextEasier()
        {
            Current = Current switch
            {
                Difficulty.Easy => Difficulty.Easy,
                Difficulty.Hard => Difficulty.Easy,
                Difficulty.Expert => Difficulty.Hard,
                _ => throw new InvalidEnumArgumentException()
            };
        }
    }
}