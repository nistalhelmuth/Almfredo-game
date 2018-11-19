using UnityEngine;
using System.Collections;

namespace Player
{
    public class MovementState: PlayerState
    {

        public float rayCounter;

        private float powerLength;
        private float powerCounter;
        private RaycastHit hitFront, hitLeft, hitRight;

        public MovementState(PlayerBehaviour player): base(player)
        {
            this.Player = player;
            powerLength = 5;    //cinco segundos de powerUp
        }

        public override void TheListener()
        {


            if (Input.GetKeyDown("space")) 
            {
                if (Player.playerState == -1) //no tiene poderes
                {
                    this.Player.Anim.SetTrigger("Eating");
                    rayCounter = 0.5f;
                    Player.ActionHandler += EatAction;
                } 
                else if(Player.playerState == 0)  //poder de fuego
                {  
                    Player.ActionHandler += ShootAction;
                }
            }
            else if (Input.GetKeyDown("enter"))
            {
                this.Player.Anim.SetTrigger("Dashing");
                
            }
        }

        public void powerTimer(){
            powerCounter -= Player.playerDeltaTime;
            if (powerCounter < 0) {
                Player.playerState = -1;
                Player.ActionHandler -= powerTimer;
            }
        }

        public void AbsorbHandler(RaycastHit hit) {
            if (hit.transform.gameObject.name == "FireSoul(Clone)") {
                MonoBehaviour.Destroy(hit.transform.gameObject);
                Player.ActionHandler += powerTimer;
                powerCounter = powerLength;
                Player.playerState = 0;
            } else if (hit.transform.gameObject.name == "otherEnemy(Clone)") {
                MonoBehaviour.Destroy(hit.transform.gameObject);
                Player.ActionHandler += powerTimer;
                powerCounter = powerLength;
                Player.playerState = 1;
            } 

        }
        
        public void EatAction()
        {
            Player.eating = true;
            Vector3 front = Player.transform.forward;
            Vector3 left = (Player.transform.forward * 1f - Player.transform.right * 0.5f).normalized;
            Vector3 right = (Player.transform.forward * 1f + Player.transform.right * 0.5f).normalized;

            bool frontRay = Physics.Raycast(Player.transform.position, front, out hitFront);
            bool leftRay = Physics.Raycast(Player.transform.position, left, out hitLeft);
            bool rightRay = Physics.Raycast(Player.transform.position, right, out hitRight);

            Debug.DrawRay(Player.transform.position, front, Color.yellow);
            Debug.DrawRay(Player.transform.position, left, Color.yellow);
            Debug.DrawRay(Player.transform.position, right, Color.yellow);


            if (frontRay && hitFront.distance < 1)
            {
                AbsorbHandler(hitFront);
            } 
            else if (leftRay && hitLeft.distance < 1)
            {
                AbsorbHandler(hitFront);
            } 
            else if (rightRay && hitRight.distance < 1)
            {
                AbsorbHandler(hitFront);
            }

            rayCounter -= Player.playerDeltaTime;
            if(rayCounter < 0) {
                Player.eating = false;
                Player.ActionHandler -= EatAction;
            }
        }   

        public void AbsorvEnemy(GameObject Enemy){
            if (Enemy.transform.gameObject.name == "FlameSoul") 
            {
                MonoBehaviour.print("soul");
                Player.playerState = 0;
            }
        }

        public void ShootAction()
        {
            GameObject fireBall =MonoBehaviour.Instantiate(
				Player.fireBallPrefab,
				Player.transform.position+ Player.transform.forward * 0.6f,
				Player.transform.rotation);
		
            fireBall.GetComponent<Rigidbody>().velocity = fireBall.transform.forward * 10.0f;

            //NetworkServer.Spawn(bullet);

            MonoBehaviour.Destroy(fireBall, 2.0f); 
            Player.ActionHandler -= ShootAction;
        }
        
    }
}
