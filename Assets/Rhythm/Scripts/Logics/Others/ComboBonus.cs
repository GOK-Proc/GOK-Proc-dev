using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct ComboBonus
    {
        [SerializeField] private ComboBonusElement[] _bonuses;
        public readonly ComboBonusElement[] Bonuses => _bonuses;
    }

    [System.Serializable]
    public struct ComboBonusElement
    {
        [SerializeField] private int _combo;
        public readonly int Combo => _combo;

        [SerializeField] private BonusType _type;
        public readonly BonusType Type => _type;

        [SerializeField] private int _value;
        public readonly int Value => _value;
    }

    public enum BonusType
    {
        None,
        Attack,
        Heal,
    }
}