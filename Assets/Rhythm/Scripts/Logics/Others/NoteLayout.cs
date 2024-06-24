using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct NoteLayout
    {
        public float FirstLaneX;
        public float LaneDistanceX;
        public float JudgeLineY;
        public float BeginLineY;
        public float DestroyLineY;

        public NoteLayout(float firstLaneX, float laneDistanceX, float judgeLineY, float beginLineY, float destroyLineY)
        {
            FirstLaneX = firstLaneX;
            LaneDistanceX = laneDistanceX;
            JudgeLineY = judgeLineY;
            BeginLineY = beginLineY;
            DestroyLineY = destroyLineY;
        }
    }
}