using UnityEngine;
using System.Collections;

namespace Enemies
{
    public class FireEnemy: Enemy
    {
        public float PlayerDistance;

        protected override void Start()
        {
            base.Start();
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
            yield return new WaitForSeconds(3f);
            anim.SetTrigger("Shoot");
            StartCoroutine("Shoot");
        }
    }
}
