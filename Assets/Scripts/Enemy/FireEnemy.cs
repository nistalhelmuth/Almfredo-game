using UnityEngine;

namespace Enemies
{
    public class FireEnemy: Enemy
    {
        public float PlayerDistance;

        public override void Update()
        {
            base.Update();

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, PlayerDistance) && hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if (navAgent.velocity == Vector3.zero)
                {
                    navAgent.SetDestination(GetRandomKeyPosition());
                }
            }
        }
    }
}
