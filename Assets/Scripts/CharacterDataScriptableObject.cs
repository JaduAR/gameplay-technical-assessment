using UnityEngine;

// this it s template to handle multiple charachters
[CreateAssetMenu(fileName = "CharacterDataScriptableObject", menuName = "ScriptableObjects/CharacterDataScriptableObject")]
public class CharacterDataScriptableObject : ScriptableObject
{
    public int Health=100;
    public int Punch1Power = 10;
    public int Punch2Power = 20;
    public int PunchChargedPower = 100;
}