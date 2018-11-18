using UnityEngine;
using System.Collections;

namespace Player
{
    public class MovementState: PlayerState
    {

        public float rayCounter;
        private RaycastHit hitFront, hitLeft, hitRight;
        public MovementState(PlayerBehaviour player): base(player)
        {
            this.Player = player;
        }

        public override void TheListener()
        {
            if (Input.GetKeyDown("space")) //optimisar esto, se siente feo
            {
                this.Player.Anim.SetTrigger("Eating");
                rayCounter = 0.5f;
                Player.ActionHandler += EatAction;
            }
            else if (Input.GetKeyDown("enter"))
            {
                this.Player.Anim.SetTrigger("Dashing");
            }
        }


        
        public void EatAction(){

            Vector3 front = Player.transform.forward;
            Vector3 left = (Player.transform.forward * 1f - Player.transform.right * 0.5f).normalized;
            Vector3 right = (Player.transform.forward * 1f + Player.transform.right * 0.5f).normalized;

            bool frontRay = Physics.Raycast(Player.transform.position, front, out hitFront);
            bool leftRay = Physics.Raycast(Player.transform.position, left, out hitLeft);
            bool rightRay = Physics.Raycast(Player.transform.position, right, out hitRight);

            if (frontRay && hitFront.distance < 1)
            {
                if (hitFront.transform.gameObject.tag == "Enemy") 
                {
                    MonoBehaviour.Destroy(hitFront.transform.gameObject);
                }  
            } 
            else if (leftRay && hitLeft.distance < 1)
            {
                if (hitLeft.transform.gameObject.tag == "Enemy") 
                {
                    MonoBehaviour.Destroy(hitLeft.transform.gameObject);
                }
            } 
            else if (rightRay && hitRight.distance < 1)
            {
                if (hitRight.transform.gameObject.tag == "Enemy")
                {
                    MonoBehaviour.Destroy(hitRight.transform.gameObject);
                }
            }

            rayCounter -= Player.playerDeltaTime;
            if(rayCounter < 0) {
                Player.ActionHandler -= EatAction;
            }
            
        }   
        
    }
}
