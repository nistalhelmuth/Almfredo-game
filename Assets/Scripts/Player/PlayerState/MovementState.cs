using UnityEngine;

namespace Player
{
    public class MovementState: PlayerState
    {
        public MovementState(PlayerBehaviour player): base(player)
        {

        }

        public override void TheListener()
        {
            if (Input.GetKeyDown("space"))
            {
                this.Player.Anim.SetTrigger("Eating");
            }
            else if (Input.GetKeyDown("enter"))
            {
                this.Player.Anim.SetTrigger("Dashing");
            }
        }
    }
}
