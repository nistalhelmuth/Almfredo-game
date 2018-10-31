using UnityEngine;
using System.Collections;

namespace Player
{
    public class BaseState: PlayerState
    {
        public BaseState(PlayerBehaviour player)
        {
            this.Player = player;
						Player.actionHandler += BASE;
						//return new IdleState(player);
        }

        public override PlayerState TheListener()
        {
            return this;
        }


				public void BASE(){
            MonoBehaviour.print("BASE");
						if (Input.GetKeyUp("space"))
            {
                MonoBehaviour.print("EAT");   
            }
        }
    }
}