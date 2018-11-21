using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Enemies
{
    public class FireEnemy: Enemy
    {
        public GameObject Fireball;

        private int nextShotTime;

        protected override void Start()
        {
            base.Start();
            nextShotTime = Random.Range(2,5);
            StartCoroutine("Shoot");
        }

        protected override void Update()
        {
            base.Update();

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, PlayerDistance) && hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if (navAgent.velocity == Vector3.zero)
                {
                    Vector3? keyPosition = GetRandomKeyPosition();
                    if (keyPosition != null)
                    {
                        navAgent.SetDestination(keyPosition.Value);
                    }
                }
            }
        }

        IEnumerator Shoot()
        {
            yield return new WaitForSeconds(nextShotTime);
            anim.SetTrigger("Shoot");
            yield return new WaitForSeconds(0.4f);
            CmdShootEnemy();
            nextShotTime = Random.Range(2,5);
            StartCoroutine("Shoot");
        }

        [Command]
        public void CmdShootEnemy()
        {
            GameObject fireBall = Instantiate(Fireball, transform.position + transform.forward * 0.4f, transform.rotation);
            NetworkServer.Spawn(fireBall);
        }
    }
}
