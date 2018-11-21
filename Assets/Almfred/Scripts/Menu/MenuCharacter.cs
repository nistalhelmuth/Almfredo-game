using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class MenuCharacter : MonoBehaviour
    {

        public Camera camera;

        private bool started = false;

        void Start()
        {
            camera.gameObject.GetComponent<MenuManager>().GoToMainMenu += LookUp;
        }

        void Update ()
        {
            if (started)
            {
                Vector3 viewVector = camera.transform.position - transform.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(viewVector), 1.5f * Time.deltaTime);
            }
        }

        public void LookUp()
        {
            started = true;
        }
    }
}
