public class AttackP2FS : FighterState
{
    public FightStateID ID => FightStateID.ATTK_P2;

    public override void Execute(Fighter fighter)
    {
        fighter.SetAnim("Punch2");
    }
}
