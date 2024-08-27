using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct GaugeRate
    {
        [SerializeField] private float _border;
        public readonly float Border => _border;

        [SerializeField] private float _perfectRate;
        public readonly float PerfectRate => _perfectRate;

        [SerializeField] private float _goodCoefficient;
        public readonly float GoodCoefficient => _goodCoefficient;

        [SerializeField] private float _falseCoefficient;
        public readonly float FalseCoefficient => _falseCoefficient;
    }
}