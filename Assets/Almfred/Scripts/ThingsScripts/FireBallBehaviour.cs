using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
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
        GameObject deadSoul = Instantiate(DeadSoul, position, Quaternion.identity);
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
                gameManager.AddDamage (obj);
            }

            break;
        case Shooter.Player:
            if (obj.tag == "Enemy")
            {
                CmdOnDestroy(obj.transform.position);
                Destroy(obj);
                Destroy(gameObject);
                gameManager.AddSouls();
            }
            break;
        }
    }
}
