using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class InputCharacter : MonoBehaviour
    {
        [SerializeField] Character character;
        public Character Character => character;


        public Vector3 Movement { get; protected set; }
    }