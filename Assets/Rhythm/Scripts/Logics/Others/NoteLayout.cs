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
        public float CreateNoteY;
        public float DestroyNoteY;
        public float JudgeLineY;

        public NoteLayout(float firstLaneX, float laneDistanceX, float createNoteY, float destroyNoteY, float judgeLineY)
        {
            FirstLaneX = firstLaneX;
            LaneDistanceX = laneDistanceX;
            CreateNoteY = createNoteY;
            DestroyNoteY = destroyNoteY;
            JudgeLineY = judgeLineY;
        }
    }
}