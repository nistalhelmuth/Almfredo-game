using UnityEngine;
using System.Collections;

namespace Player
{
    public class WalkingState: PlayerState
    {

        public WalkingState(PlayerBehaviour player)
        {
            this.Player = player;
            Player.actionHandler += Walking;
        }

        public override PlayerState TheListener()
        { 
            if (Input.GetAxis(Player.HorizontalAxis) == 0 || Input.GetAxis(Player.VerticalAxis) == 0)
            {
                Player.actionHandler -= Walking;
                return new IdleState(Player);
            }

            if (Input.GetKeyDown("space")) 
            {
                Player.actionHandler -= Walking;
                return new EatingState(Player);
            }

            return this;
        }

        public void Walking(){
            MonoBehaviour.print("WALKING");
            Player.transform.forward = Vector3.Lerp(Player.transform.forward, Player.Mdirection, 0.5f);
            
            //Player.transform.rotation = Quaternion.LookRotation(Player.Mdirection, Vector3.up);
            Player.body.velocity = Player.Mdirection * Time.deltaTime * Player.speed;
        }
        
    }
}