using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class NoteJudge
    {
        private readonly bool _isVs;
        private readonly NoteLayout _layout;
        private readonly INoteProvider _noteProvider;
        private readonly IJudgeCountable _judgeCountable;
        private readonly IComboCountable _comboCountable;
        private readonly IBattleMode _battle;
        private readonly IRhythmMode _rhythm;
        private readonly ISoundPlayable _soundPlayable;
        private readonly IEffectDrawable _effectDrawable;

        public NoteJudge(bool isVs, in NoteLayout layout, INoteProvider noteProvider, IJudgeCountable judgeCountable, IComboCountable comboCountable, IBattleMode battle, IRhythmMode rhythm, ISoundPlayable soundPlayable, IEffectDrawable effectDrawable)
        {
            _isVs = isVs;
            _layout = layout;
            _noteProvider = noteProvider;
            _judgeCountable = judgeCountable;
            _comboCountable = comboCountable;
            _battle = battle;
            _rhythm = rhythm;
            _soundPlayable = soundPlayable;
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
                        if (judge != Judgement.False) _soundPlayable.PlaySE($"Hit{note.Color.ToStringQuickly()}{(note.IsLarge ? "L" : "")}");
                        _effectDrawable.DrawJudgeEffect(new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * note.Lane, _layout.JudgeLineY, 0f), judge);
                        _effectDrawable.DrawJudgeFontEffect(new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * note.Lane, _layout.JudgeLineY, 0f), judge);
                        if (_isVs)
                        {
                            var isLarge = note is TapNote && note.IsLarge;
                            _battle.Hit(note.Color, isLarge, judge);
                            _effectDrawable.DrawBattleEffect(new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * note.Lane, _layout.JudgeLineY, 0f), note.Color, isLarge, judge, note.Id);
                            if (_comboCountable.IsAttackBonus) _effectDrawable.DrawComboAttackEffect();
                        }
                        else
                        {
                            _rhythm.Hit(judge);
                        }
                    }
                }
            }
        }
    }
}