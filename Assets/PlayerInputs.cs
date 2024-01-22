using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputs : MonoBehaviour
    {
    // speed at which we seek our max movement speed
    [Header("Input Settings")]
    [SerializeField]
    private float        inputSpeed;

    [Header("Current Values")]
    [HideInInspector]
    public Vector2      movement;
    [HideInInspector]
    public bool         punch;

    public Vector3      localMovement;

    void Update()
        {
        // FYI, I was planning on using the new input system, but wasn't sure if you wanted me to add packages,
        // so I'm using the old for this demo but can easily swap this to the new input system

        int y = 0;
        int x = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            y += 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            y -= 1;
        
        movement.y = Mathf.MoveTowards(movement.y, y, inputSpeed * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            x += 1;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            x -= 1;

        movement.x = Mathf.MoveTowards(movement.x, x, inputSpeed * Time.deltaTime);


        Vector3 camFwd = Camera.main.transform.forward;
        Vector3 camRgt = -Camera.main.transform.right;

        camFwd *= movement.y;
        camRgt *= movement.x;

        Vector3 worldSpaceDir = new Vector3(camFwd.x + camRgt.x, 0, camFwd.z + camRgt.z);
        localMovement = transform.InverseTransformDirection(worldSpaceDir);

        //Debug.Log(localMovement);

        if (Input.GetKeyDown(KeyCode.Space))
            OnPunch();
        }

    public void OnPunch()
        {
        punch = true;
        }

    private void LateUpdate()
        {
        // clear our punch input
        punch = false;
        }
    }
