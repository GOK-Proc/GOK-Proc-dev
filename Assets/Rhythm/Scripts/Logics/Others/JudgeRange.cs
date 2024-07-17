using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct JudgeRange
    {
        [SerializeField] private double _perfect;
        public readonly double Perfect { get => _perfect; }

        [SerializeField] private double _good;
        public readonly double Good { get => _good; }

    }
}