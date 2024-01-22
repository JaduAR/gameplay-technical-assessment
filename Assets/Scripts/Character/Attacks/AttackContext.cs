namespace Game.Assets.Scripts.Character.Attacks
{
    public class AttackContext
    {
        public AttackContext(ICharacter player, ICharacter target)
        {
            Player = player;
            Target = target;
        }

        public ICharacter Player { get; }
        public ICharacter Target { get; }
    }
}