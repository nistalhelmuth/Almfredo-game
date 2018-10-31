using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

	public class Room {
		public Vector2 gridPos;
		public int type;
		public bool doorTop, doorBot, doorLeft, doorRight;

		public Room(Vector2 _gridPos, int _type){
			gridPos = _gridPos;
			type = _type;
		}
	}

	private Room[,] rooms;
	List<Vector2> takenPositions = new List<Vector2>();
	public int width, height, numberOfRooms;
	public GameObject roomObject;
	public GameObject doorObject;
	// Use this for initialization
	void Start () {
		CreateRooms();
		SetRoomDoors();
		SetRoomTypes();
		InstantiateRooms();
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

	void InstantiateRooms(){
		Vector2 position;
		Vector3 drawPos;
		GameObject roomCreated;
		for (int i = 0; i<numberOfRooms;i++){	
			Room roomToCreate = rooms[(int)takenPositions[i].x+width,(int)takenPositions[i].y+height];
			position = roomToCreate.gridPos;
			drawPos = new Vector3(position.x*21,0f,position.y*13);
			roomCreated = Instantiate(roomObject, drawPos, Quaternion.identity);
			roomCreated.transform.parent = transform;
			roomCreated.gameObject.GetComponent<RoomGeneration>().SetupScene(roomToCreate.type);
			if(roomToCreate.doorTop){
				Instantiate(doorObject, drawPos+Vector3.forward * 6, Quaternion.identity).transform.parent = roomCreated.transform;
			}
			if(roomToCreate.doorRight){
				Instantiate(doorObject, drawPos+Vector3.right * 10, Quaternion.identity).transform.parent = roomCreated.transform;
			}
			if(roomToCreate.doorBot){
				Instantiate(doorObject, drawPos+Vector3.back * 6, Quaternion.identity).transform.parent = roomCreated.transform;
			}
			if(roomToCreate.doorLeft){
				Instantiate(doorObject, drawPos+Vector3.left * 10, Quaternion.identity).transform.parent = roomCreated.transform;
			}
		}
	}

}
