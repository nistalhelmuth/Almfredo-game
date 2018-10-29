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

            if (Input.GetAxis(Player.HorizontalAxis) != 0)
            {   
                Player.actionHandler -= IDLE;
                return new WalkingState(Player);
            }

            return this;
        }

        public void IDLE(){
            MonoBehaviour.print("IDLE");
        }
    }
}