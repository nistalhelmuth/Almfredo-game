using UnityEngine;
using System.Collections;

namespace Player
{
    public class IdleState: PlayerState
    {

        public IdleState(PlayerBehaviour player)
        {
            this.Player = player;
            Player.actionHandler += IDLE;
        }

        public override PlayerState TheListener()
        {

            if (Input.GetAxis(Player.HorizontalAxis) != 0 || Input.GetAxis(Player.VerticalAxis) != 0)
            {   
                Player.actionHandler -= IDLE;
                return new WalkingState(Player);
            }

            if (Input.GetKeyDown("space")) 
            {
                Player.actionHandler -= IDLE;
                return new EatingState(Player);
            }

            return this;
        }

        public void IDLE(){
            MonoBehaviour.print("IDLE");
        }

        
    }
}