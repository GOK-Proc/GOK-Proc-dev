using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct JudgeRate
    {
        [SerializeField] private float _attack;
        public readonly float Attack => _attack;

        [SerializeField] private float _defense;
        public readonly float Defense => _defense;

        [SerializeField] private float _score;
        public readonly float Score => _score;
    }
}