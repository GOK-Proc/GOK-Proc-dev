using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct JudgeCount
    {
        [SerializeField] private int _perfect;
        public readonly int Perfect => _perfect;

        [SerializeField] private int _good;
        public readonly int Good => _good;

        [SerializeField] private int _false;
        public readonly int False => _false;

        public JudgeCount(int p, int g, int f)
        {
            _perfect = p;
            _good = g;
            _false = f;
        }
    }
}