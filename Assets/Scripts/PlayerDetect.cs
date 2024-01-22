using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class PlayerDetect : MonoBehaviour      //attached to the PlayerDet objects
{  
    [Tooltip("Base Opponent Prefab GameObject")]
    public GameObject OpponentGO;

    private OpponentControler OppCon;

    // Start is called before the first frame update
    void Start()
    {
        OppCon = OpponentGO.GetComponent<OpponentControler>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerAvatar")
        {
            if (this.gameObject.name == "PlayerDetNorth")
            {
                OppCon.PlayerCurLoc = OpponentControler.PlayerLoc.North;
            }
            else if (this.gameObject.name == "PlayerDetEast")
            {
                OppCon.PlayerCurLoc = OpponentControler.PlayerLoc.East;
            }
            else if (this.gameObject.name == "PlayerDetSouth")
            {
                OppCon.PlayerCurLoc = OpponentControler.PlayerLoc.South;
            }
            else if (this.gameObject.name == "PlayerDetWest")
            {
                OppCon.PlayerCurLoc = OpponentControler.PlayerLoc.West;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "PlayerAvatar")
        {
            OppCon.PlayerCurLoc = OpponentControler.PlayerLoc.None;
        }
    }
}

