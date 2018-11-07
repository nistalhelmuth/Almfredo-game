using UnityEngine;
using System.Collections;

namespace Player
{
    public class BaseState: PlayerState
    {
        public BaseState(PlayerBehaviour player): base(player)
        {
            //return new IdleState(player);
        }

        public override void TheListener()
        {

        }


        public void BASE()
        {
            MonoBehaviour.print("BASE");
            if (Input.GetKeyUp("space"))
            {
                MonoBehaviour.print("EAT");
            }
        }
    }
}
