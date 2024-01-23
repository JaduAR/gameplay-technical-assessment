public class AttackP1FS : FighterState
{
    public FightStateID ID => FightStateID.ATTK_P1;

    public override void Execute(Fighter fighter)
    {
        fighter.SetAnim("Punch1");
    }
}
