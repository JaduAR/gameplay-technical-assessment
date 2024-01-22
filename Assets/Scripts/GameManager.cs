using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;
    public UIManager uiManager;
    public Opponent opponent;
    public PlayerController player;

    private void Awake()
    {
        if (i != null && i != this)
            Destroy(this);
        else
            i = this;

        player = FindObjectOfType<PlayerController>();
        opponent = FindObjectOfType<Opponent>();
    }
}
