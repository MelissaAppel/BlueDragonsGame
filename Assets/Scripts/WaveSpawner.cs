﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public float initialWaitTime;
    public float timeBetweenWaves;
    public float timeBetweenSubWaves;
    public int numOfEnemiesPerWave;
    public float timeBetweenEnimies;
    public int numOfSubWaves;
    public int numOfWaves;
    public TextMeshProUGUI waveText;

    float timer = 0;
    WaveSpawnerStates state = WaveSpawnerStates.Starting;
    int enemiesSpawned = 0;
    int wavesSpawned = 0;
    int subWavesSpawned = 0;
    public static string[] enemyTypes = {"Enemy", "SlowResistEnemy", "BurnResistEnemy", "PoisonResistEnemy", "FreezeResistEnemy", "BossEnemy"};
    string currentSpawnType;
    System.Random rand = new System.Random();
    static int currentLevel = 0;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(state);
        timer += Time.deltaTime;
        switch(state){
            case WaveSpawnerStates.Starting:
                currentLevel = PlayerPrefs.GetInt("Level", 0);
                if(timer > initialWaitTime){
                    currentSpawnType = WaveSpawner.enemyTypes[rand.Next(enemyTypes.Length - 1)];
                    //Debug.Log(currentSpawnType);
                    //currentSpawnType = "SlowResistEnemy";
                    //Debug.Log(rand.Next(enemyTypes.Length));
                    state = WaveSpawnerStates.Spawning;
                    timer = 0;
                }
                break;
            case WaveSpawnerStates.Waiting:
                if(timer >= timeBetweenSubWaves){
                    currentSpawnType = enemyTypes[rand.Next(enemyTypes.Length - 1)];
                    state = WaveSpawnerStates.Spawning;
                    timer = 0;
                }
                break;
            case WaveSpawnerStates.Spawning:
                waveText.text = "Wave: " + (wavesSpawned + 1);
                if(wavesSpawned == numOfWaves - 1){
                    GameObject EnemyGO = ObjectPool.SharedInstance.GetPooledObject("BossEnemy");
                    EnemyController Enemy = EnemyGO.GetComponent<EnemyController>();
                    EnemyGO.SetActive(true);
                    wavesSpawned++;
                    state = WaveSpawnerStates.Finished;
                } else {
                    if(timer > timeBetweenEnimies){
                        //Debug.Log(currentSpawnType);
                        GameObject EnemyGO = ObjectPool.SharedInstance.GetPooledObject(currentSpawnType);
                        EnemyController Enemy = EnemyGO.GetComponent<EnemyController>();
                        EnemyGO.SetActive(true);
                        Enemy.normalSpeed += .05f * currentLevel;
                        enemiesSpawned++;
                        timer = 0;
                        if(enemiesSpawned >= numOfEnemiesPerWave + currentLevel + wavesSpawned){
                            subWavesSpawned++;
                            enemiesSpawned = 0;
                            if(subWavesSpawned >= numOfSubWaves){
                                state = WaveSpawnerStates.Finished;
                                wavesSpawned++;
                            }
                            else{
                                state = WaveSpawnerStates.Waiting;
                                timer = 0;
                            }
                        }
                    }
                }
                break;
            case WaveSpawnerStates.Finished:
                if(wavesSpawned >= numOfWaves){
                    //level beat
                }else if(timer > timeBetweenWaves){
                    state = WaveSpawnerStates.Spawning;
                    enemiesSpawned = 0;
                    subWavesSpawned = 0;
                    currentSpawnType = enemyTypes[rand.Next(enemyTypes.Length - 1)];
                }
                break;
            default: 
                break;
        }

    }

    public static void NextLevel(){
        PlayerPrefs.SetInt("Level", currentLevel + 1);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}

public enum WaveSpawnerStates {
    Spawning,
    Waiting,
    Starting,
    Finished
}
