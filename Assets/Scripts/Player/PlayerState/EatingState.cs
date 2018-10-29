using UnityEngine;
using System.Collections;

namespace Player
{
    public class EatingState: PlayerState
    {

        public EatingState(PlayerBehaviour player)
        {
            this.Player = player;
            Player.actionHandler += Eating;
        }

        public override PlayerState TheListener()
        { 
            
            if (Input.GetKeyUp("space"))
            {
                MonoBehaviour.print("Done");
                Player.actionHandler -= Eating;
                return new IdleState(Player);
            }
            return this;
        }

        public void Eating(){
            MonoBehaviour.print("EATING");
        }
    }
}