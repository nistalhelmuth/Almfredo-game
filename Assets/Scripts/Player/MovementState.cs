using UnityEngine;

namespace Player
{
    public class MovementState: PlayerState
    {
        
        private RaycastHit hit;
        private GameObject objectHit;
        public MovementState(PlayerBehaviour player): base(player)
        {

        }

        public override void TheListener()
        {
            
            
            if (Input.GetKeyDown("space")) //optimisar esto, se siente feo
            {
                this.Player.Anim.SetTrigger("Eating");
                Player.ActionHandler += ShootAction;
            }
            else if (Input.GetKeyDown("enter"))
            {
                this.Player.Anim.SetTrigger("Dashing");
            }
        }

        public void ShootAction(){
            if (Physics.Raycast(Player.transform.position, Player.transform.forward, out hit)){
                if (hit.distance < 1f && hit.transform.gameObject.tag == "Enemy") {
                    MonoBehaviour.Destroy(hit.transform.gameObject);
                    //objectHit = hit.transform.gameObject; 
                    //MonoBehaviour.print(objectHit.gameObject.tag);       
                }   
            }
            Player.ActionHandler -= ShootAction;
        }   
        
        public void takeHeal(){

        }

        public void HealAction(){

        }

        
    }
}
