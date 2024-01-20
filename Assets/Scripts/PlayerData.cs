// this script can expand to have other attributes for the player like special abilities 
public class PlayerData
{
    CharacterDataScriptableObject _data;
    public CharacterDataScriptableObject Data
    {
        get
        {
            return _data;
        }
        private set
        {
            _data = value;
        }
    }

    private float _currentHealth;
    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }

    public PlayerData(CharacterDataScriptableObject data)
    {
        _currentHealth = data.Health;
        Data = data;
    }

    public void ResetData()
    {
        _currentHealth = Data.Health;

    }
}