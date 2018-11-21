using UnityEngine;
using UnityEngine.Networking;

public class LevelStarter : MonoBehaviour
{


    public NetworkManagerHUD NetworkHUD;
    // Use this for initialization
    void Start ()
    {
        NetworkHUD.showGUI = true;
    }

}
