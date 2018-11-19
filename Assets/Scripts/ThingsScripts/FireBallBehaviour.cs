using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;


public class FireBallBehaviour : MonoBehaviour
{
    public enum Shooter {Player, Enemy};

    public Shooter WhoShoots;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 5.0f;
        Destroy(gameObject, 3.0f);
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
                Destroy(obj);
            }
            break;
        }
    }
}
