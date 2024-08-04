using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class NoteJudge
    {
        private readonly NoteLayout _layout;
        private readonly INoteProvider _noteProvider;
        private readonly IJudgeCountable _judgeCountable;
        private readonly IBattle _battle;
        private readonly IEffectDrawable _effectDrawable;

        public NoteJudge(NoteLayout layout, INoteProvider noteProvider, IJudgeCountable judgeCountable, IBattle battle, IEffectDrawable effectDrawable)
        {
            _layout = layout;
            _noteProvider = noteProvider;
            _judgeCountable = judgeCountable;
            _battle = battle;
            _effectDrawable = effectDrawable;
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
                        _effectDrawable.DrawJudgeEffect(new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * note.Lane, _layout.JudgeLineY, 0f), judge);
                        _effectDrawable.DrawBattleEffect(new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * note.Lane, _layout.JudgeLineY, 0f), note.Color, note.IsLarge, judge);
                    }
                }
            }
        }
    }
}