using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderData;

public class Controller_ParticleHit : MonoBehaviour
{
    [SerializeField] ParticleSystem _ps;
    [SerializeField] Transform _spawnPoint;

    
    public void Spawn(float _size)
        {
            ParticleSystem pSys = Instantiate(_ps);
            pSys.transform.position = _spawnPoint.position;
            pSys.transform.rotation = _spawnPoint.rotation;
            pSys.transform.localScale = pSys.transform.localScale * _size;
            
            Color col = _size <= 1 ? Color.yellow : Color.red;
            pSys.GetComponent<Renderer>().material.SetColor("_EmissionColor", col);
        }
}
