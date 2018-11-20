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
            Player.Anim.SetBool("Walking", true);
        }

        public override void TheListener()
        {
            base.TheListener();

            if (Input.GetAxis(Player.HorizontalAxis) == 0 && Input.GetAxis(Player.VerticalAxis) == 0)
            {
                Player.ActionHandler -= TheListener;
                Player.PhysicsHandler -= Walking;
                Player.Anim.SetBool("Walking", false);
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
                Player.Anim.SetFloat("YMovement", 1f);
            }
            else
            {
                float diffAngle = Mathf.Acos(Vector3.Dot(Vector3.forward, Player.Mdirection));
                if (Player.Mdirection.x != 0)
                {
                    diffAngle *=  (Player.Mdirection.x / Mathf.Abs(Player.Mdirection.x));
                }
                float xCoord = velocityVector.x * Mathf.Cos(diffAngle) - velocityVector.z * Mathf.Sin(diffAngle);
                float yCoord = velocityVector.x * Mathf.Sin(diffAngle) + velocityVector.z * Mathf.Cos(diffAngle);
                Player.Anim.SetFloat("YMovement", yCoord);
                Player.Anim.SetFloat("XMovement", xCoord);
            }
            Player.body.velocity = velocityVector * Time.deltaTime * Player.speed;
        }

    }
}
