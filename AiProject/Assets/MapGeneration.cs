using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using UnityEngine;

public class MapGeneration : MonoBehaviour {

	public string [,] map;
	public int mapWidth = 160;
	public int mapHeight = 120;


	 void Start(){
		
		string strMap = "";

		map = new string[mapHeight,mapWidth];

		//Initialize Entire Map
		for (int i = 0; i < mapHeight; i++) {
			for (int j = 0; j < mapWidth; j++) {
				map [i, j] = "1";
			}
			//Debug.Log ("\n");
		}


		//Generate Hard To Traverse Tiles
		HardToTraverse();


		//Create Highways
		for (int i = 0; i < 4; i++) {
			List<Node> fullPath = new List<Node> ();
			fullPath = InitRandomPath (fullPath);
			List<Node> temp = new List<Node> ();


			//bool valid = true;

//			int count = 5;
//			while (count > 0) {
			//temp = PathForHighway (fullPath [fullPath.Count - 1].x, fullPath [fullPath.Count - 1].y);
			/*
				for (int h = temp.Count-1; h < temp.Count - 20; h--) {
					if (temp [h].x < 0 || temp [h].x > 160 || temp [h].y > 120 || temp [h].y < 0) {
						print ("Found a boundary");
						break;
			
					} else {
						
					
						print ("Created another path");
					}


					count--;
				}

				*/

			

			//}
		


			foreach (Node tmp in temp) {
				print ("X: " + tmp.x + " " + "Y " + tmp.y);
			}

			print ("FULLPATH LENTH " + fullPath.Count);




		}


		//Create String and Write to File
		for (int i = 0; i < mapHeight; i++) {
			for (int j = 0; j < mapWidth; j++) {
				strMap = strMap + map [i, j];
			}
			strMap = strMap + "\n";
			//Debug.Log ("\n");
		}
		File.WriteAllText ("test.txt", strMap);

	}

	void HardToTraverse(){
		for (int i = 0; i < 8; i++) {
			//int randCenterX = Random.Range (31/2, mapHeight-31/2);
			//int randCenterY = Random.Range (31/2, mapWidth-31/2);
			int randCenterX = Random.Range (31/2, mapHeight-1);
			int randCenterY = Random.Range (31/2, mapWidth-1);
			//int randCenterX = Random.Range (0, mapHeight-1);
			//int randCenterY = Random.Range (0, mapWidth-1)65-=-8

			for (int h = randCenterX-31/2; ( h <= (randCenterX + 31/2)) && h < mapHeight-1; h++) {
				for (int k = randCenterY-31/2; (k <= (randCenterY + 31/2)) && k < mapWidth-1; k++) {
					map [h,k] = (Random.Range(1,3)) + "";
				}
				//Debug.Log ("\n");
			}
		}

	}

	List<Node> PathForHighway(int x,int y){
		List<Node> firstPath = new List<Node> ();








		return firstPath;
	}



	/*
	List<Node> PathForHighway(int x, int y){
		List<Node> fullPath = new List<Node> ();
		Debug.Log ("BE|ER");
		int prob = ProbOfPath ();


			Debug.Log ("YOLO");
			if (map [x - 1, y].Equals ("a")) {
				//up
				switch (prob) {
				case 0:
					//right
					for (int t = 0; t < 20; t++) {
						y++;
						map [x, y] = "a";

						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh-3");
					}

					break;
				case 1:
					//left
					for (int t = 0; t < 20; t++) {
						y--;
						map [x, y] = "a";
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh-2");
					}
					break;
				default:
					//down
				for (int t = 0; t < 20; t++) {
						x++;
						map [x, y] = "a";
						
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh-1");
					}

					break;
				}

		} else if (map [x + 1, y].Equals ("a")) {
				//down
				switch (prob) {
				case 0:
					//right
					for (int t = 0; t < 20; t++) {
						y++;
						map [x, y] = "a";
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh0");
					}

					break;
				case 1:
					//left
					for (int t = 0; t < 20; t++) {
						y--;
						map [x, y] = "a";
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh1");
					}
					break;
				default:
					//up
					for (int t = 0; t < 20; t++) {
						x--; 
						map [x, y] = "a";
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh2");
					}


					break;

				}
			} else if (map [x, y - 1].Equals ("a")) {
				//left
				switch (prob) {
				case 0:
					//up
					for (int t = 0; t < 20; t++) {
						x--; 
						map [x, y] = "a";
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh3");
					}

					break;
				case 1:
					//down
					for (int t = 0; t < 20; t++) {
						x++;
						map [x, y] = "a";
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh4");
					}
					break;
				default:
					//right
					for (int t = 0; t < 20; t++) {
						y++;
						map [x, y] = "a";
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh5");
					}

					break;
				}
			} else if(map [x, y + 1].Equals ("a")) {
				//right
				switch (prob) {
				case 0:
					//down
					for (int t = 0; t < 20; t++) {
						x++;
						map [x, y] = "a";
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh6");
					}

					break;
				case 1:
					//up
					for (int t = 0; t < 20; t++) {
						x--; 
						map [x, y] = "a";
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh7");
					}
					break;
				default:
					//left
					for (int t = 0; t < 20; t++) {
						y--;
						map [x, y] = "a";
						Node tmp = new Node ();
						tmp.x = x;
						tmp.y = y;
						fullPath.Add (tmp);
						Debug.Log ("Bleh8");
					}

					break;
				}
			}else {
			print ("PATH FAILED TO GENERATE");
		}

		return fullPath;
	}

*/
//
//	bool validCheck(int x, int highVal){
//		
//		bool isValid = true;
//		for(int i = 0; i < x; i++){
//			if (x > 0) {
//				x--;
//			} else {
//				isValid = false;
//				break;
//			}
//		}
//
//		for(int i = 0; i < 20; i++){
//			if (x < highVal-1) {
//				x++;
//			} else {
//				isValid = false;
//				break;
//			}
//		}
//
//		return isValid;
//	}

//	bool CheckForValidPath(int x, int y){
//		//Check upwards
//		return validCheck(x, 120) && validCheck(y,160);
//
//	}

	int ProbOfPath(){
		int num = 0;
		num = Random.Range (0, 5);

		return num;
	}

	List<Node> InitRandomPath(List<Node> fullPath){

		int randHighwayX = Random.Range (0, 120);
		int randHighwayY = Random.Range (0, 160);

		int randEdgeChoice = Random.Range (0, 4);
		print ("randEdgeChoice is " + randEdgeChoice);
		int[] startHighWay = new int[2];
		//Dictionary<char, int> startHighWay = new Dictionary<char, int>();

		//int startHighWayX, startHighWayY;

		switch (randEdgeChoice){
		case 0:
			//Make Highway Near Y - Axis
			startHighWay [0] = 0;
			startHighWay [1] = randHighwayY;

			Node tmp = new Node ();
			tmp.x = startHighWay [0];
			tmp.y = startHighWay [1];
			fullPath.Add (tmp);

			map [startHighWay [0], startHighWay [1]] = "a";


			print (startHighWay[0] + " , " + startHighWay[1]);
			for (int t = 0; t < 20; t++) {


				startHighWay [0]++;
				startHighWay [1] = randHighwayY;
		//		print (startHighWay['x'] + " , " + startHighWay['y']);

				Node tmp1 = new Node ();
				tmp1.x = startHighWay[0];
				tmp1.y = startHighWay [1];
				fullPath.Add (tmp1);

				map [startHighWay [0], startHighWay [1]] = "a";

			}

			break;
		case 1:
			//Make Highway Along Far Y - Axis
			startHighWay [0] = mapHeight - 1; 
			startHighWay [1] = randHighwayY;
			print (startHighWay[0] + " , " + startHighWay[1]);
			map [startHighWay [0], startHighWay [1]] = "a";






			Node tmp2 = new Node ();
			tmp2.x = startHighWay [0];
			tmp2.y = startHighWay [1];
			fullPath.Add (tmp2);

			//print (startHighWay['x'] + " , " + startHighWay['y']);
			for (int t = 0; t < 20; t++) {
				startHighWay [0]--; 
				startHighWay [1] = randHighwayY;
		//		print (startHighWay['x'] + " , " + startHighWay['y']);

				Node tmp3 = new Node ();
				tmp3.x = startHighWay[0];
				tmp3.y = startHighWay [1];
				fullPath.Add (tmp3);

				map [startHighWay [0], startHighWay [1]] = "a";
			}

			break;
		case 2:
			//Make Highway Along Near X - Axis
			startHighWay [0] = randHighwayX; 
			startHighWay [1] = 0;

			print (startHighWay[0] + " , " + startHighWay[1]);
			map [startHighWay [0], startHighWay [1]] = "a";

			Node tmp4 = new Node ();
			tmp4.x = startHighWay [0];
			tmp4.y = startHighWay [1];
			fullPath.Add (tmp4);

			for (int t = 0; t < 20; t++) {
				startHighWay [0] = randHighwayX; 
				startHighWay [1]++;
				map [startHighWay [0], startHighWay [1]] = "a";

				Node tmp5 = new Node ();
				tmp5.x = startHighWay[0];
				tmp5.y = startHighWay [1];
				fullPath.Add (tmp5);
			}

			break;
		case 3:
			//Make Highway Along Far X- Axis
			startHighWay [0] = randHighwayX; 
			startHighWay [1] = mapWidth - 1;

			print (startHighWay[0] + " , " + startHighWay[1]);
			map[startHighWay[0], startHighWay[1]] = "a";
			Node tmp6 = new Node ();
			tmp6.x = startHighWay [0];
			tmp6.y = startHighWay [1];
			fullPath.Add (tmp6);

			for (int t = 0; t < 20; t++) {
				startHighWay [0] = randHighwayX; 
				startHighWay [1]--;
				map[startHighWay[0], startHighWay[1]] = "a";

				Node tmp7 = new Node ();
				tmp7.x = startHighWay[0];
				tmp7.y = startHighWay [1];
				fullPath.Add (tmp7);
			}
			break;
		}
		//print (startHighWay['x'] + " , " + startHighWay['y']);

		return fullPath;
	}



}
