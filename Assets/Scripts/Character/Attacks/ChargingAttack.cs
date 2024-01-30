using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Assets.Scripts.Character.Attacks
{
    [CreateAssetMenu(menuName = "Scriptable Objects/ChargingAttack")]
    public class ChargingAttack: Attack
    {
        [SerializeField]
        private string _finalAttackTriggerName;

        [SerializeField]
        private float _finalAttackAnimationTime;

        protected override async Task WaitForAnimation(Task inputTrigger, AttackContext context)
        {
            // Wait for what take longer - minimal animation time or player release the button
            await Task.WhenAll(base.WaitForAnimation(inputTrigger, context), inputTrigger);

            context.Player.TriggerAnimation(_finalAttackTriggerName);

            await Task.Delay(TimeSpan.FromSeconds(_finalAttackAnimationTime));
        }
    }
}