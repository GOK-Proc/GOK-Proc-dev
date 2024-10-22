using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct TutorialData
    {
        [SerializeField] private BeatmapInformation _beatmap;
        public readonly BeatmapInformation Beatmap => _beatmap;

        [SerializeField] private float[] _times;
        public readonly float[] Times => _times;
    }
}