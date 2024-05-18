using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class Note : LaneObject, IJudgeable
    {
        public double JustTime { get; private set; }
        public double[] JudgeRange { get; private set; }


        public void Create(Vector3 position, Vector3 velocity, int lane, NoteColor color, bool isLarge, double justTime, double[] judgeRange)
        {
            Create(position, velocity, lane, color, isLarge);
            JustTime = justTime;
            JudgeRange = judgeRange;
        }

        public virtual Judgement Judge(double time)
        {
            return default;
        }
    }
}