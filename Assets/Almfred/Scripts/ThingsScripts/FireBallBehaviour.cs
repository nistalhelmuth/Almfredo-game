﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Enemies;
using UnityEngine.Networking;


public class FireBallBehaviour : NetworkBehaviour
{
    public enum Shooter {Player, Enemy};

    public Shooter WhoShoots;

    private GameManager gameManager;
    public GameObject DeadSoul;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 8.0f;
        Destroy(gameObject, 3.0f);
        gameManager = FindObjectOfType<GameManager> ();
    }

    [Command]
    void CmdOnDestroy(Vector3 position)
    {
        GameObject deadSoul = Instantiate(DeadSoul, position, Quaternion.Euler(0f, 180f, 0f));
        NetworkServer.Spawn(deadSoul);
    }
    void OnTriggerEnter(Collider collider)
    {
        GameObject obj = collider.gameObject;
        switch (WhoShoots)
        {
        case Shooter.Enemy:
            if (!obj.CompareTag("Obstacle") && !obj.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }

            if (obj.CompareTag("Player"))
            {
                Vector3 hitDirection = collider.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                obj.GetComponent<PlayerBehaviour>().takeDmg(hitDirection);
            }

            break;
        case Shooter.Player:
            if (obj.tag == "Enemy")
            {
                int life = obj.GetComponent<Enemy>().TakeDamage();
                if (life == 0)
                {
                    CmdOnDestroy(obj.transform.position);
                    Destroy(obj);
                    gameManager.AddSouls();
                }
                Destroy(gameObject);
            }
            break;
        }
    }
}
