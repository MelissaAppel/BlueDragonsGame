﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTowerController : TowerController
{
    [Header("Bomb Tower Stats")]
    public int pooledObjects = 4;
    public int bomberBaseAttack = 1;
    public int bomberMidAttack = 3;
    public int bomberHighAttack = 5;
    public float bomberBaseRange = 2f;
    public float bomberMidRange = 2.5f;
    public float bomberHighRange = 3f;


    public override void Start()
    {
        GetComponent<BoxCollider2D>().size = new Vector2((range*2), (range*2));
        count = 0f;
        currentAttack = bomberBaseAttack;
        currentRange = bomberBaseRange; 
    }

    //Upgrade Tower attack speed, range, rotation speed, and attack damage
    public void upgradeBombTower(){
        if (level == towerLevel.start){
            currentAttack = bomberMidAttack;
            currentRange = bomberMidRange;
            GetComponent<BoxCollider2D>().size = new Vector2((range*2), (range*2));
        }
        if (level == towerLevel.mid){
            currentAttack = bomberHighAttack;
            currentRange = bomberHighRange;
            GetComponent<BoxCollider2D>().size = new Vector2((range*2), (range*2));
        }
        if (level == towerLevel.high){
            return;
        }
        rotationSpeed += 0.5f;
        timeBeforeNextShot += .25f;
    }


    //Send bomb to closest target 
   public override void Shoot(){
       GameObject ProjectileGO = BombPool.SharedInstance.GetPooledObject(pooledObjects);
        ProjectileController Projectile = ProjectileGO.GetComponent<ProjectileController>();
        Projectile.ReceiveStats(currentAttack, currentRange);
        if (Projectile != null)
        {
            ProjectileGO.transform.position = transform.position;
            ProjectileGO.transform.rotation = transform.rotation;
            ProjectileGO.SetActive(true);
            Projectile.ReceiveTarget(target, currentAttack); //Pass target to ProjectileController and damage amount
        }  
   }
    
}
