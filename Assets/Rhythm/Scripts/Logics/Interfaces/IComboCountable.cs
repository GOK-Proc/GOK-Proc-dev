namespace Rhythm
{
    public interface IComboCountable
    {
        int Combo { get; }
        int MaxCombo { get; }
        bool IsAttackBonus { get; }
    }
}