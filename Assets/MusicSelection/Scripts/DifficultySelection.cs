using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Rhythm;

namespace MusicSelection
{
    public class DifficultySelection
    {
        public static Difficulty Current { get; private set; }

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