using TMPro;
using UnityEngine;

namespace Game.Assets.Scripts.UI.Score
{
    public class ScorePresenter : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _scoreText;

        // TODO This component shouldn't store any data
        private int _score = 0;

        // TODO Refactor to use storage and reactive properties
        public void OnDeathEvent(Object sender, object data)
        {
            _scoreText.text = (++_score).ToString();
        }
    }
}
