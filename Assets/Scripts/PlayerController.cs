using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PlayerData _playerData;

    [SerializeField]
    private PunchDetector _leftHnadPunchDetector;

    [SerializeField]
    private PunchDetector _rightHnadPunchDetector;

    public delegate void PunchLanded(int value);
    public PunchLanded punchLanded;

    public delegate void PlayerPunching();
    public PlayerPunching playerPunching;

    [SerializeField]
    PlayerAttack _playerAttak;

    // we will assume thate there is only one character data for this task
    [SerializeField]
    private CharacterDataScriptableObject _myCharacterData;


    public void Init()
    {
        _playerData = new PlayerData(_myCharacterData);
    }

    private void Start()
    {
        _leftHnadPunchDetector.punchLanded += PunchEnemy;
        _rightHnadPunchDetector.punchLanded += PunchEnemy;
    }

    private void OnDestroy()
    {
        _leftHnadPunchDetector.punchLanded -= PunchEnemy;
        _rightHnadPunchDetector.punchLanded -= PunchEnemy;
    }


    private void PunchEnemy()
    {
        Debug.Log("Punched the enemy with " + _playerAttak.PunchType);

        if (_playerAttak.PunchType == EPunchType.none)
        {
            return;
        }

        int damageValue = 0;
        switch (_playerAttak.PunchType)
        {
            case EPunchType.punch1:
                damageValue = _myCharacterData.Punch1Power;
                break;
            case EPunchType.punch2:
                damageValue = _myCharacterData.Punch2Power;
                break;
            case EPunchType.charged:
                damageValue = _myCharacterData.PunchChargedPower;
                break;
            default:
                break;
        }

        punchLanded(damageValue);
    }
    public void PlayDeadAnimation()
    {
        Debug.Log("I Am Dead!");
    }

    public void ResetData()
    {
        _playerData.ResetData();
    }
}