using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimSFX : StateMachineBehaviour {
    [SerializeField] private AudioClip clip;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameAudio.Instance.PlaySFX(clip);
    }
}
