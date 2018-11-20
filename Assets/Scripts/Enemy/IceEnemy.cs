using UnityEngine;

namespace Enemies
{
    public class IceEnemy: Enemy
    {
        protected override void Update()
        {
            base.Update();

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, PlayerDistance) && hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                navAgent.SetDestination(hit.collider.gameObject.transform.position);
            }

        }
    }
}
