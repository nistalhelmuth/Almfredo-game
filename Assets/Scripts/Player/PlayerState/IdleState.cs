using UnityEngine;
using System.Collections;

namespace Player
{
    public class IdleState: MovementState
    {

        public IdleState(PlayerBehaviour player): base(player)
        {
            Player.ActionHandler += TheListener;
        }

        public override void TheListener()
        {
            base.TheListener();

            if (Input.GetAxis(Player.HorizontalAxis) != 0 || Input.GetAxis(Player.VerticalAxis) != 0)
            {
                Player.ActionHandler -= TheListener;
                new WalkingState(Player);
            }
        }
    }
}
