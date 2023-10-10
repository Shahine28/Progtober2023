using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    bool isDead;
    public static event Action OnPlayerDamage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        OnPlayerDamage?.Invoke();
        if (health < 0 && !isDead)
        {

        }
    }

    private void OnValidate()
    {
        if (health > maxHealth)
        {
            maxHealth = health;
        }

        if (health < 0)
        {
            health = Mathf.Abs(health);
        }

        if (maxHealth < 0)
        {
            maxHealth = Mathf.Abs(maxHealth);
        }
        /*OnPlayerDamage?.Invoke();*/
    }
}
