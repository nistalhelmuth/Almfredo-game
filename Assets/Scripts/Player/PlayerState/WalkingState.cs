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

            if (Input.GetAxis(Player.HorizontalAxis) == 0 && Input.GetAxis(Player.VerticalAxis) == 0)
            {
                Player.ActionHandler -= TheListener;
                Player.PhysicsHandler -= Walking;
                new IdleState(Player);
            }
        }

        public void Walking()
        {
            //Player.transform.rotation = Quaternion.LookRotation(Player.Mdirection, Vector3.up);

            Vector3 velocityVector = new Vector3(Input.GetAxis(Player.HorizontalAxis), 0f, Input.GetAxis(Player.VerticalAxis));

            //Cambia la direccion del player solo cuando no hay input
            //del eje que la maneja
            if (Player.Mdirection.magnitude == 0)
            {
                Player.transform.forward = Vector3.Lerp(Player.transform.forward, velocityVector, 0.5f);
            }
            Player.body.velocity = velocityVector * Time.deltaTime * Player.speed;
        }

    }
}
