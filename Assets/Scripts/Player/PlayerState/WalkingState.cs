using UnityEngine;
using System.Collections;

namespace Player
{
    public class WalkingState: MovementState
    {

        public WalkingState(PlayerBehaviour player): base(player)
        {
            this.Player = player;
            Player.PhysicsHandler += Walking;
            Player.ActionHandler += TheListener;
        }

        public override void TheListener()
        {
            base.TheListener();

            if (Input.GetAxis(Player.HorizontalAxis) == 0 || Input.GetAxis(Player.VerticalAxis) == 0)
            {
                Player.ActionHandler -= TheListener;
                Player.PhysicsHandler -= Walking;
                new IdleState(Player);
            }
        }

        public void Walking()
        {
            Player.transform.forward = Vector3.Lerp(Player.transform.forward, Player.Mdirection, 0.5f);
            //Player.transform.rotation = Quaternion.LookRotation(Player.Mdirection, Vector3.up);
            Player.body.velocity = Player.Mdirection * Time.deltaTime * Player.speed;
        }

    }
}
