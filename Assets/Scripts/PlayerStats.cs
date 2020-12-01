using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    bool isAlive = true;
    bool nearAPlanet;
    public bool NearAPlanet
    {
        get { return nearAPlanet; }
        set { nearAPlanet = value; }
    }
    GravityController nearPlanet;
    public GravityController NearPlanet
    {
        get { return nearPlanet; }
        set { nearPlanet = value; }
    }
    
    [SerializeField]
    int maxHealth = 100;
    [SerializeField]
    Canvas statCanvas;
    [SerializeField]
    Slider healthBar;
    [SerializeField]
    PlayerPlanetDetector planetDetector;

    int health;
    public int Health
    {
        get { return health; }
        set { 
            health = value;
            healthBar.value = health;
        }
    }

    private void Awake()
    {
        ResetHealth();
    }
    private void ResetHealth()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }
    public void decreaseHealth(int h)
    {
        health -= h;
        if (health <= 0)
        {
            health = 0;
            isAlive = false;
        }
        healthBar.value = health;
    }
    public void increaseHealth(int h)
    {
        health += h;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        healthBar.value = health;
    }
}
