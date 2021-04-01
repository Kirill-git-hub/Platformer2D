﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region Singleton creation

    public static GameController instance = null; // Экземпляр объекта

    // Метод, выполняемый при старте игры
    void Start()
    {
        // Теперь, проверяем существование экземпляра
        if (instance == null)
        { // Экземпляр менеджера был найден
            instance = this; // Задаем ссылку на экземпляр объекта
        }
        else if (instance == this)
        { // Экземпляр объекта уже существует на сцене
            Destroy(gameObject); // Удаляем объект
        }
        // Теперь нам нужно указать, чтобы объект не уничтожался
        // при переходе на другую сцену игры
        DontDestroyOnLoad(gameObject);
    }

    #endregion
    
    public GameObject playerPrefab = null;
    public Transform playerContainer = null;

    [HideInInspector]
    public GameObject playerObject = null;
    private bool playerIsAlive = false;
    [SerializeField] private List<EnemySpawn> spawnPosition = new List<EnemySpawn>();
    private PlayerData playerData;
    public PlayerData PlayerData => playerData;
    public void RespawnPlayer()
    {
        if (!playerIsAlive && playerContainer)
        {
            playerObject = Instantiate(playerPrefab, playerContainer);
            playerIsAlive = true;
            playerData = playerObject.GetComponent<PlayerData>();
            UserInterfaceController.instance.UpdateHealth(playerData.PlayerHealth);
        }
    }

    public void SpawnManager()
    {
        foreach (var pos in spawnPosition)
        {
            if (pos.zombieObject)
            {
                Destroy(pos.zombieObject);
            }
            pos.Spawn();
        }
    }
    public void KillPlayer(float killAfter = 0f)
    {
        if (playerIsAlive && playerObject)
        {
            Destroy(playerObject, killAfter);
            playerObject = null;
            playerIsAlive = false;
            UserInterfaceController.instance.ShowDeathMessage();            
            UserInterfaceController.instance.UpdateHealth(0);
        }
    }
    public void DealDamageToPlayer(int damage)
    {
        PlayerData.PlayerHealth -= damage;
        UserInterfaceController.instance.UpdateHealth(playerData.PlayerHealth);
    }
}
