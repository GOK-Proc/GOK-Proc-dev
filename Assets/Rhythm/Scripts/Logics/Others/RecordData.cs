using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct RecordData
    {
        [SerializeField] private int _score;
        public readonly int Score => _score;

        [SerializeField] private bool _isCleared;
        public readonly bool IsCleared => _isCleared;

        [SerializeField] private int _maxCombo;
        public readonly int MaxCombo => _maxCombo;

        [SerializeField] private Achievement _achievement;
        public readonly Achievement Achievement => _achievement;

        [SerializeField] private JudgeCount _judgeCount;
        public readonly JudgeCount JudgeCount => _judgeCount;

        public RecordData(int score, bool isClear, int maxCombo, Achievement achievement, JudgeCount judgeCount)
        {
            _score = score;
            _isCleared = isClear;
            _maxCombo = maxCombo;
            _achievement = achievement;
            _judgeCount = judgeCount;
        }
    }
}