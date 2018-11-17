using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player
{
    public class BaseState: PlayerState
    {
        private RaycastHit hit;
        private GameObject objectHit;
        public BaseState(PlayerBehaviour player): base(player)
        {
            //return new IdleState(player);
        }

        public override void TheListener()
        {
            MonoBehaviour.print(Player.transform.position);
            MonoBehaviour.print(Player.Mdirection);
        }


        public void BASE()
        {
            
        }
    }
}
