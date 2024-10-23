namespace Data.Runtime
{
    public interface IAmSpellGiver
    {
        public enum Spell
        {
            fire,
            water,
            grass,
            arcane
        }

        Spell spell { get; }
    }
}
