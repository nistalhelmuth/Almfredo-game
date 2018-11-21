using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enemies;
using UnityEngine.Networking;
using Player;

public class RoomGeneration : NetworkBehaviour
{
    public GameObject[] enemies;
    public GameObject chair;
    public GameObject shelf;
    public GameObject table;
    public GameObject mapRoom;
    public int type;
    [SyncVar]
    public bool roomClear;
    private List <Vector3> gridPositions = new List <Vector3> ();
    private NavMeshSurface navSurface;

    private int columns = 16;                                        //Number of columns in our game board.
    private int rows = 8;
    private Transform camera;

    void InitialiseList ()
    {
        //Clear our list gridPositions.
        gridPositions.Clear ();

        //Loop through x axis (columns).
        for (int x = 0; x < columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int z = 0; z < rows - 1; z++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add (new Vector3(x, 0f, z));
            }
        }
    }

    Vector3 RandomPosition ()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range (0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex] + transform.position  + new Vector3(-7f, 0f, -3f);;

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt (randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }

    void CmdLayoutObjectAtRandom (GameObject objectType, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range (minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            GameObject thing= Instantiate(objectType, randomPosition + objectType.transform.position, Quaternion.identity, this.transform);
            NetworkServer.Spawn(thing);
        }
    }
    [Command]
    void CmdLayoutEnemyAtRandom (int minimum, int maximum )
    {
        int enemycount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < enemycount; i++)
        {
            int randomEnemy = Random.Range(0, enemies.Length);
            Vector3 randomPosition = RandomPosition();
            GameObject instance = Instantiate(enemies[randomEnemy], randomPosition, Quaternion.identity, this.transform);
            NetworkServer.Spawn(instance);
            // Se le asigna varias posiciones de "escape" al enemigo instanciado
            instance.GetComponent<Enemy>().AddKeyPosition(randomPosition);
            for (int e = 0; e < 2; e++)
            {
                instance.GetComponent<Enemy>().AddKeyPosition(RandomPosition());
            }
        }
    }

    public void SetupScene (int _type, GameObject _mapRoom)
    {
        mapRoom = _mapRoom;
        type = _type;
        switch (type)
        {
        case 0: //cuando hay que poner algo en el cuarto

            //aqui podes tirar un random y evaluar segun la opcion
            //la unica opcion que esta ahorita es la de llenar el cuarto random
            roomClear = false;
            InitialiseList ();
            CmdLayoutObjectAtRandom (table, 3, 7);
            CmdLayoutObjectAtRandom (chair, 1, 4);
            CmdLayoutObjectAtRandom (shelf, 4, 7);
            break;
        case 1: //cuarto de start
            mapRoom.SetActive(true);
            roomClear=true;
            break;
        case 2: //ultimo cuarto
            roomClear=true;
            break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
            if (player.isLocal)
            {
                camera = player.playerCamera.transform;
                Vector3 dif = new Vector3(transform.position.x - camera.position.x, 0f, transform.position.z - camera.position.z);
                if (!roomClear)
                {
                    NavMeshSurface navSurface = transform.Find("NavMesh").GetComponent<NavMeshSurface>();
                    navSurface.BuildNavMesh();
                    CmdLayoutEnemyAtRandom (4, 9);
                    roomClear = true;
                }
                camera.Translate(dif);
                //mapRoom.SetActive(true);
            }
        }
    }

}
