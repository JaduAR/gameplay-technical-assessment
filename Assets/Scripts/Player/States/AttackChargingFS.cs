public class AttackChargingFS : FighterState
{
    public FightStateID ID => FightStateID.ATTK_CHARGING;

    public override void Execute(Fighter fighter)
    {
        fighter.SetAnim("HoldCharge");
    }

}
