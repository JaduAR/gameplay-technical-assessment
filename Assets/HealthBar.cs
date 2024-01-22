using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
    {
    public int       maxHealth = 100;
    public Slider    slider;
    public Transform refPoint;

    public UnityEvent<int> onHealthChanged;

    int curHealth;
    Camera cam;

    void Awake()
        {
        cam = Camera.main;
        curHealth = maxHealth;

        slider.value = 1.0f;
        }

    // Update is called once per frame
    void Update()
        {
        Vector3 screenPt = cam.WorldToScreenPoint(refPoint.transform.position);
        slider.transform.position = screenPt;
        }

    public void OnDamage(int damage)
        {
        curHealth -= damage;
        if (curHealth < 0) 
            curHealth = 0;

        slider.value = curHealth / (float)maxHealth;

        onHealthChanged?.Invoke(curHealth);
        }
    }
