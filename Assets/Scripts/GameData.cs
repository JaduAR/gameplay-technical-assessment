using UnityEngine;

public class GameData : MonoBehaviour      //attached to the GameData object in MainScene
{
    public enum GameModes { easy, medium, hard }

    [Header("Global Variables")]
    [Tooltip("Which Difficulity mode Selected")]
    public GameModes ModeSelected = GameModes.easy;

    [Header("Player Variables")]
    [Tooltip("Player Starting Health")]
    public int PlayerHealth = 100;

    [Tooltip("Player Max Health")]
    public int PlayerMaxHealth = 100;

    [Tooltip("Player Name")]
    public string PlayerName = "Player Name";

    [Header("Opponent Variables")]
    [Tooltip("Opponent Starting Health")]
    public int OpponentHealth = 100;

    [Tooltip("Opponent Max Health")]
    public int OpponentMaxHealth = 100;

    [Tooltip("Opponent Name")]
    public string OpponentName = "Opponent Name";

    //[Header("Flags for Player Punching Variables")]

    [HideInInspector] public bool PlayerPunch1;
    [HideInInspector] public bool PlayerPunch2;
    [HideInInspector] public bool PlayerCharge;

    [HideInInspector] public bool PlayerPunch1Hit;
    [HideInInspector] public bool PlayerPunch2Hit;

    [HideInInspector] public bool PlayerCombo;    
}

