using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public sealed class ParamsCharacter
    {
        [SerializeField]
        private Vector3 _MovementDirection;
        public Vector3 MovementDirection
            {
                get => _MovementDirection;
                set => _MovementDirection = Vector3.ClampMagnitude(value, 1);
            }
        public float ForwardSpeed { get; set; }
        public float DesiredForwardSpeed { get; set; }
        public float VerticalSpeed { get; set; }

    }