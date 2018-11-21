using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Player
{
    public class MovementState: PlayerState
    {

        private float rayCounter;
        private float powerLength;
        private float powerCounter;
        private RaycastHit hitFront, hitLeft, hitRight;
        private GameManager gameManager;
        private GameObject proyectilePrefab;

        public MovementState(PlayerBehaviour player): base(player)
        {
            this.Player = player;
            powerLength = 5;    //cinco segundos de powerUp
            gameManager = MonoBehaviour.FindObjectOfType<GameManager>();
        }

        public override void TheListener()
        {
            // Cambio de direccion del jugador
            if (Input.GetAxis(Player.HorizontalViewAxis) != 0 || Input.GetAxis(Player.VerticalViewAxis) != 0)
            {
                Player.transform.forward = Vector3.Lerp(Player.transform.forward, Player.Mdirection, 0.5f);
            }

            if ((Input.GetButtonDown("Attack") || Input.GetAxis("Attack") > 0) && !Player.TriggerPressed)
            {
                Player.TriggerPressed = true;
                if (Player.playerState == -1) //no tiene poderes
                {
                    rayCounter = 0.5f;
                    Player.eating = true;
                    this.Player.Anim.SetTrigger("Eating");
                    Player.MusicSource.PlayOneShot(Player.biteSound, 1F);
                    Player.StartOtherCoroutine(WaitAndEat());
                }
                else if (Player.playerState == 0) //poder de fuego
                {
                    proyectilePrefab = Player.fireBallPrefab;
                    Player.Anim.SetTrigger("Shoot");
                    Player.StartOtherCoroutine(WaitAndShoot());
                }
                else if (Player.playerState == 1) //poder de fuego
                {
                    proyectilePrefab = Player.iceSpikePrefab;
                    Player.Anim.SetTrigger("Shoot");
                    Player.StartOtherCoroutine(WaitAndShoot());
                }
            }
            else if (Input.GetKeyDown("enter"))
            {
                this.Player.Anim.SetTrigger("Dashing");

            }
        }

        public void powerTimer()
        {
            powerCounter -= Player.playerDeltaTime;

            Player.powerBar.sizeDelta = new Vector2(powerCounter * 100.0f / 5.0f, Player.powerBar.sizeDelta.y);
            if (powerCounter < 0)
            {
                Player.playerState = -1;
                Player.ActionHandler -= powerTimer;
                Player.powerCanvas.SetActive(false);
                Player.BodyRenderer.material = Player.NeutralMaterial;
            }
        }

        public void AbsorbHandler(RaycastHit hit)
        {
            if (hit.transform.gameObject.name == "FireSoul(Clone)")
            {
                MonoBehaviour.Destroy(hit.transform.gameObject);
                Player.ActionHandler += powerTimer;
                Player.powerCanvas.SetActive(true);
                powerCounter = powerLength;
                Player.playerState = 0;
                Player.BodyRenderer.material = Player.FireMaterial;
            }
            else if (hit.transform.gameObject.name == "IceSoul(Clone)")
            {
                MonoBehaviour.Destroy(hit.transform.gameObject);
                Player.ActionHandler += powerTimer;
                Player.powerCanvas.SetActive(true);
                powerCounter = powerLength;
                Player.playerState = 1;
                Player.BodyRenderer.material = Player.IceMaterial;
            }
            else if (hit.transform.gameObject.name == "DeadSoul(Clone)")
            {
                MonoBehaviour.Destroy(hit.transform.gameObject);
                gameManager.AddSouls();
            }

        }

        IEnumerator WaitAndEat()
        {
            yield return new WaitForSeconds(0.1f);
            Player.ActionHandler += EatAction;
        }

        public void EatAction()
        {
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
            if (rayCounter < 0)
            {
                Player.eating = false;
                Player.ActionHandler -= EatAction;
            }
        }

        public void AbsorvEnemy(GameObject Enemy)
        {
            if (Enemy.transform.gameObject.name == "FlameSoul")
            {
                MonoBehaviour.print("soul");
                Player.playerState = 0;
            }
        }

        [Command]
        public void CmdShootAction()
        {
            Player.MusicSource.PlayOneShot(Player.shootSound, 1F);
            GameObject proyectile = MonoBehaviour.Instantiate(
                proyectilePrefab,
                Player.transform.position + Player.transform.forward * 0.6f,
                Player.transform.rotation);

            NetworkServer.Spawn(proyectile);

            Player.ActionHandler -= CmdShootAction;
        }

        IEnumerator WaitAndShoot()
        {
            yield return new WaitForSeconds(0.2f);
            Player.ActionHandler += CmdShootAction;
        }

    }
}
