using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class SaveHealth : MonoBehaviour
{

    HealthSystem healthSystem;
    public float k = 350.0f;

    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        k = healthSystem.GetMaxHealthPoint();

    }

    public void SavehHealth()
    {

        PlayerPrefs.SetFloat("maxHealthPoint", healthSystem.GetMaxHealthPoint());

    }
    public void LoadHealth()
    {
        

        float k = PlayerPrefs.GetFloat("maxHealthPoint");
    }
}

