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
        public Vector2 NoteRectUpperLeft;
        public Vector2 NoteRectLowerRight;

        public NoteLayout(float firstLaneX, float laneDistanceX, float judgeLineY, Vector2 noteRectUpperLeft, Vector2 noteRectLowerRight)
        {
            FirstLaneX = firstLaneX;
            LaneDistanceX = laneDistanceX;
            JudgeLineY = judgeLineY;
            NoteRectUpperLeft = noteRectUpperLeft;
            NoteRectLowerRight = noteRectLowerRight;
        }
    }
}