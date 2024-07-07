using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct LostRate
    {
        [SerializeField] private float _victory;
        public readonly float Victory { get => _victory; }

        [SerializeField] private float _overkill;
        public readonly float Overkill { get => _overkill; }

        [SerializeField] private float _knockout;
        public readonly float Knockout { get => _knockout; }

    }
}