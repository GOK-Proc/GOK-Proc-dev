using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class NoteJudge
    {
        private readonly INoteProvider _noteProvider;
        private readonly IJudgeCountable _judgeCountable;

        public NoteJudge(INoteProvider noteProvider, IJudgeCountable judgeCountable)
        {
            _noteProvider = noteProvider;
            _judgeCountable = judgeCountable;
        }

        public void Judge()
        {
            foreach (var note in _noteProvider.Notes)
            {
                if (!note.IsJudged)
                {
                    var judge = note.Judge();

                    if (judge != Judgement.Undefined)
                    {
                        _judgeCountable.CountUpJudgeCounter(judge);
                        Debug.Log(judge);
                    }
                }
            }
        }
    }
}