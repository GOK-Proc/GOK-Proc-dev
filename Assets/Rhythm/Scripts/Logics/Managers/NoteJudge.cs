using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class NoteJudge
    {
        private readonly INoteProvider _noteProvider;
        private readonly IJudgeCountable _judgeCountable;
        private readonly IBattle _battle;

        public NoteJudge(INoteProvider noteProvider, IJudgeCountable judgeCountable, IBattle battle)
        {
            _noteProvider = noteProvider;
            _judgeCountable = judgeCountable;
            _battle = battle;
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
                        _battle.Hit(note.Color, note.IsLarge, judge);
                    }
                }
            }
        }
    }
}