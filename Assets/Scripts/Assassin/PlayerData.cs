﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int damage;
    [SerializeField] private int playerHealth = 3;
    [SerializeField] private Image[] heartImage;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    public int MaxHealth
    {
        get => maxHealth;
    }
    public int Damage
    {
        get => damage; 
    }
    public int PlayerHealth
    {
        get => playerHealth;
        set => playerHealth = value;
    }

}
