using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct LostRate
    {
        [SerializeField] private float _victory;
        public readonly float Victory => _victory;

        [SerializeField] private float _overkill;
        public readonly float Overkill => _overkill;

        [SerializeField] private float _knockout;
        public readonly float Knockout => _knockout;

    }
}