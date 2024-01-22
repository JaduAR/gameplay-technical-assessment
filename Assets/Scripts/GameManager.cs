using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    public bool finishedGame { get; private set; }

    public void Start() {
        GameAudio.Instance.PlayMusic(GameAudio.Instance.battle, true);
    }

    public void Complete() {
        finishedGame = true;
        GameGUI.Instance.FinishScreen();
        PlayerController.Instance.Halt();
    }
}
