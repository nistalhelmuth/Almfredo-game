using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class LevelGeneration : NetworkBehaviour {

	public class Room {
		public Vector2 gridPos;
		public int type;
		public bool doorTop, doorBot, doorLeft, doorRight;

		public GameObject mapRoom;

		public Room(Vector2 _gridPos, int _type){
			gridPos = _gridPos;
			type = _type;
		}
	}

	List<Vector2> takenPositions = new List<Vector2>();
	public int width, height, numberOfRooms;
	public GameObject roomObject;
	public GameObject[] doorObject;
	public GameObject[] roomWhiteObj;
	public Transform mapRoot;
	private Transform camera;
	private Room[,] rooms;
	// Use this for initialization
	void Start () {
		
	}

	public override void OnStartServer()
	{
		CreateRooms();
		SetRoomDoors();
		SetRoomTypes();
		CmdDrawMap();
		CmdInstantiateRooms();
	}
	
void CreateRooms(){
		//setup
		rooms = new Room[width*2,height*2];
		rooms[width,height] = new Room(Vector2.zero, 1);
		takenPositions.Insert(0,Vector2.zero);
		Vector2 checkPos;
		//magic numbers
		float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
		//add rooms
		for (int i =0; i < numberOfRooms -1; i++){
			float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
			//grab new position
			checkPos = NewPosition();
			//test new position
			if (NumberOfNeighbors(checkPos) > 1 && Random.value > randomCompare){
				int iterations = 0;
				do{
					checkPos = SelectiveNewPosition();
					iterations++;
				}while(NumberOfNeighbors(checkPos) > 1 && iterations < 100);
			}
			//finalize position
			rooms[(int) checkPos.x + width, (int) checkPos.y + height] = new Room(checkPos, 0);
			takenPositions.Insert(0,checkPos);
		}	
	}
	Vector2 NewPosition(){
		Vector2 checkingPos = Vector2.zero;
		Vector2 position = new Vector2();
		do{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
			position.x = (int) takenPositions[index].x;
			position.y = (int) takenPositions[index].y;
			int direction = Random.Range(0,3);
			switch (direction)
			{
					case 0:
						position.y += 1;
						break;
					case 1:
						position.x += 1;
						break;
					case 2:
						position.y -= 1;
						break;
					case 3:
						position.x -= 1;	
						break;				
			}
			checkingPos = position;
		}while (takenPositions.Contains(checkingPos) || position.x < -width || position.y < -height || position.x >= width || position.y >= height);
		
		return checkingPos;
	}
	Vector2 SelectiveNewPosition(){ // method differs from the above in the two commented ways
		int index = 0, inc;
		Vector2 checkingPos = Vector2.zero;
		Vector2 position = new Vector2();
		do{
			inc = 0;
			do{ 
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc ++;
			}while (NumberOfNeighbors(takenPositions[index]) > 1 && inc < 100);
			position.x = (int) takenPositions[index].x;
			position.y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
			bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
			if (UpDown){ //find the position bnased on the above bools
				if (positive){
					position.y += 1;
				}else{
					position.y -= 1;
				}
			}else{
				if (positive){
					position.x += 1;
				}else{
					position.x -= 1;
				}
			}
			checkingPos = position;
		}while (takenPositions.Contains(checkingPos) || position.x < -width || position.y < -height || position.x >= width || position.y >= height);

		return checkingPos;
	} 

	int NumberOfNeighbors(Vector2 checkingPos){
		int ret = 0; 
		if (takenPositions.Contains(checkingPos + Vector2.right)){ 
			ret++;
		}
		if (takenPositions.Contains(checkingPos + Vector2.left)){
			ret++;
		}
		if (takenPositions.Contains(checkingPos + Vector2.up)){
			ret++;
		}
		if (takenPositions.Contains(checkingPos + Vector2.down)){
			ret++;
		}
		return ret;
	}

	void SetRoomDoors(){
		for (int i = 0; i<numberOfRooms;i++){
			if(takenPositions.Contains(takenPositions[i] + Vector2.up)){
				rooms[(int)takenPositions[i].x+width,(int)takenPositions[i].y+height].doorTop = true;
			}
			if(takenPositions.Contains(takenPositions[i] + Vector2.right)){
				rooms[(int)takenPositions[i].x+width,(int)takenPositions[i].y+height].doorRight = true;
			}
			if(takenPositions.Contains(takenPositions[i] + Vector2.down)){	
				rooms[(int)takenPositions[i].x+width,(int)takenPositions[i].y+height].doorBot = true;
			}
			if(takenPositions.Contains(takenPositions[i] + Vector2.left)){
				rooms[(int)takenPositions[i].x+width,(int)takenPositions[i].y+height].doorLeft = true;
			}
		}
	}

	void SetRoomTypes(){
		int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); 
		rooms[(int)takenPositions[index].x+width,(int)takenPositions[index].y+height].type = 0; 
		int index2;
		int inc = 0;
		do {
			index2 = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); 
			inc++;
		} while ( index2 == index && NumberOfNeighbors(takenPositions[index2]) > 1 && inc < 100);
		rooms[(int)takenPositions[index2].x+width,(int)takenPositions[index2].y+height].type = 2; 
	}

	void CmdDrawMap() {
		foreach(Room room in rooms){
			if(room != null){
				Vector2 drawPos = room.gridPos;
				drawPos.x *= 16;//aspect ratio of map sprite
				drawPos.y *= 12;
				switch (room.type)
				{
					case 0:
						room.mapRoom = Instantiate(roomWhiteObj[0], drawPos, Quaternion.identity, mapRoot);
						NetworkServer.Spawn(room.mapRoom);
						//room.mapRoom.SetActive(false);
						break;
					case 1:
						room.mapRoom = Instantiate(roomWhiteObj[1], drawPos, Quaternion.identity, mapRoot);
						NetworkServer.Spawn(room.mapRoom);
						//room.mapRoom.SetActive(false);
						break;
					case 2:
						room.mapRoom = Instantiate(roomWhiteObj[2], drawPos, Quaternion.identity, mapRoot);
						NetworkServer.Spawn(room.mapRoom);
						//room.mapRoom.SetActive(false);
						break;		
				}	
			}
		}
	}

	void CmdInstantiateRooms(){
		Vector2 position;
		Vector3 drawPos;
		GameObject roomCreated;
		for (int i = 0; i<numberOfRooms;i++){	
			Room roomToCreate = rooms[(int)takenPositions[i].x+width,(int)takenPositions[i].y+height];
			position = roomToCreate.gridPos;
			drawPos = new Vector3(position.x*21.25f,0f,position.y*12.75f);
			roomCreated = Instantiate(roomObject, drawPos, Quaternion.identity, transform);
			NetworkServer.Spawn(roomCreated);
			roomCreated.gameObject.GetComponent<RoomGeneration>().SetupScene(roomToCreate.type, roomToCreate.mapRoom);
			if(!roomToCreate.doorTop)
			{
				GameObject topDoor = Instantiate(doorObject[0], drawPos+Vector3.forward * 5.3f, Quaternion.identity, roomCreated.transform);
				NetworkServer.Spawn(topDoor);
			}
			if(!roomToCreate.doorRight)
			{
				GameObject rightDoor = Instantiate(doorObject[1], drawPos+Vector3.right * 9.37f, Quaternion.identity, roomCreated.transform); 
				NetworkServer.Spawn(rightDoor);
			}
			if(!roomToCreate.doorBot)
			{
				GameObject botDoor = Instantiate(doorObject[2], drawPos+Vector3.back * 5.3f, Quaternion.identity, roomCreated.transform);
				NetworkServer.Spawn(botDoor);
			}
			if(!roomToCreate.doorLeft)
			{
				GameObject leftDoor = Instantiate(doorObject[3], drawPos+Vector3.left * 9.37f, Quaternion.identity, roomCreated.transform);
				NetworkServer.Spawn(leftDoor);
			}
			 
		}
	}

}
