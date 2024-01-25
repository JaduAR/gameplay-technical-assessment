
using UnityEngine;

public class SimpleFightStageManager : MonoBehaviour
{

    public static SimpleFightStageManager Instance;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
