using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct NoteLayout
    {
        [SerializeField] private float _firstLaneX;
        public readonly float FirstLaneX => _firstLaneX;

        [SerializeField] private float _laneDistanceX;
        public readonly float LaneDistanceX => _laneDistanceX;

        [SerializeField] private float _judgeLineY;
        public readonly float JudgeLineY => _judgeLineY;

        [SerializeField] private float _beginLineY;
        public readonly float BeginLineY => _beginLineY;

        [SerializeField] private float _destroyLineY;
        public readonly float DestroyLineY => _destroyLineY;

    }
}