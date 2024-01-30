using Game.Assets.Scripts.Character;
using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.Managers
{
    public class GameRoot : MonoSingleton<GameRoot>
    {
        [SerializeField] 
        private Character.Character _player;

        public ICharacter Player => _player;
    }
}
