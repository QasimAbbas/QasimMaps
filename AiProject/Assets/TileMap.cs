using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System;
using Priority_Queue;


using UnityEngine;

public class TileMap : MonoBehaviour {
    public GameObject selectedUnit;
    public TileType[] tileTypes;

	//public MapGeneration createMap;

    int[,] tiles;
    Node[,] graph;
	int[,] tilesMap;
	string[,] loadMap;

    //List<Node> currentPath = null;

    int mapSizeX = 160;
    int mapSizeY = 120;

    //Use this for initialization
    void Start() {

		ParseMap ();

//		for (int i = 2; i < lines.Length; i++) {
//			//print(lines[i]);
//		}

//		for (int i = 0; i < lines.Length; i++) {
//			for (int j = 0; j < lines.Length; j++) {
//				tilesMap [i] [j] = lines [i].IndexOf(j);
//			}
//			
//		}
//
//		for (int i = 0; i < lines.Length; i++) {
//			for (int j = 0; j < lines.Length; j++) {
//				print (tilesMap [i] [j]);
//			}
//
//		}



        //set up the selectedUnit's variable
        selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().tileY = (int) selectedUnit.transform.position.y;
        selectedUnit.GetComponent<Unit>().map = this;

        GenerateMapData();
        GeneratePathfindingGraph();
        GenerateMapVisual();
    }



	void ParseMap(){
		loadMap = new string[mapSizeX,mapSizeY];
		string[] lines = File.ReadAllLines ("test.txt");

		print ("FILE TEST.TXT HAS : " + lines.Length + " LINES");

		for (int i = 0; i < mapSizeY; i++) {
			string line = lines [i];
			for (int j = 0; j < mapSizeX; j++){
				print ("LINE HAS LENGTH : " + line.Length);
				print ("ELEMENT: " + line[j] + " AT LINE " + i + " AT POSITION " + j);
				loadMap [j, i] = line [j] + "";

				
			}
		}
	
	}



    void GenerateMapData() {
        //allocate our map tiles
        tiles = new int[mapSizeX, mapSizeY];
        int x, y;

	

//        //initialise our map tiles
//        for(x = 0; x < mapSizeX; x++) {
//            for(y = 0; y < mapSizeY; y++) {
//                tiles[x, y] = 0;
//            }
//        }

		//initialise special tiles
		for(x = 0; x < mapSizeX; x++) {
			for(y = 0; y < mapSizeY; y++) {
				if (loadMap [x, y].Equals ("2")) {
					print ("CREATE HTT");
					tiles [x, y] = 1;
				} else if (loadMap [x, y].Equals ("a")) {
					print ("HighWay");
					tiles [x, y] = 2;
				} else {
					tiles[x, y] = 0;
				}

			}
		}

//        //swamp area
//        for(x = 3; x <= 5; x++) {
//            for(y = 0; y < 4; y++) {
//                tiles[x, y] = 1;
//            }
//        }



//        //u shaped mountain range
//
//        tiles[4, 4] = 2;
//        tiles[5, 4] = 2;
//        tiles[6, 4] = 2;
//        tiles[7, 4] = 2;
//        tiles[8, 4] = 2;
//        tiles[8, 5] = 2;
//        tiles[4, 5] = 2;
//        tiles[4, 6] = 2;
//        tiles[8, 6] = 2;

    }

   public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY) {
        TileType tt = tileTypes[tiles[targetX, targetY]];

		if(UnitCanEnterTile(targetX, targetY) == false) {
			return Mathf.Infinity;
		}

        float cost = tt.movementCost;

        if (sourceX != targetX && sourceY != targetY) {
            // we are moving diagonally. fudge the cost for tie breaking
            //purely a cosmetic thing
            cost += 0.41f;

        }

        return cost;
    }



    void GeneratePathfindingGraph() {
        //initialize the array
        graph = new Node[mapSizeX, mapSizeY];

        //initialize a node for each spot in the array
        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }

        //now that all the nodes exist, calculate their neighbors
        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {
                
                //8 way connections
                /*
                if (x > 0) 
                    graph[x, y].neighbors.Add(graph[x - 1, y]);
                if(x < mapSizeX - 1)
                    graph[x, y].neighbors.Add(graph[x + 1, y]);
                if(y > 0)
                    graph[x, y].neighbors.Add(graph[x, y - 1]);
                if (y < mapSizeY - 1)
                    graph[x, y].neighbors.Add(graph[x, y + 1]);
                
                */
                
                //diagonals
                //Try Left
                if (x > 0) {
                    graph[x, y].neighbors.Add(graph[x - 1, y]);

                    if (y > 0)
                        graph[x, y].neighbors.Add(graph[x - 1, y - 1]);
                    if (y < mapSizeY-1)
                        graph[x, y].neighbors.Add(graph[x - 1, y + 1]);
                }

                //Try Right
                if (x < mapSizeX-1) {
                    graph[x, y].neighbors.Add(graph[x + 1, y]);

                    if (y > 0)
                        graph[x, y].neighbors.Add(graph[x + 1, y - 1]);
                    if (y < mapSizeY  - 1)
                        graph[x, y].neighbors.Add(graph[x + 1, y + 1]);
                }
                //Straight up and down
                if (y > 0)
                    graph[x, y].neighbors.Add(graph[x, y - 1]);
                if (y < mapSizeY - 1)
                    graph[x, y].neighbors.Add(graph[x, y + 1]);



            }
        }
    }



    void GenerateMapVisual() {
        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {
                TileType tt = tileTypes[tiles[x, y]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);
                ClickableTile ct = go.GetComponent<ClickableTile>();
				//go.GetComponent<Renderer> ().material.color = Color.blue;
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
            }

        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int y){
        return new Vector3(x, y, 0);
    }

	public bool UnitCanEnterTile(int x, int y) {
		//we could test the unit's walk/hover/fly type against various 
		//terrain flags here to see if they are allowed to entre the tile


		return tileTypes[tiles[x, y]].isWalkable;
	}

	public void GeneratePathToD(int x, int y) {
		//clear out our unit's old path
		selectedUnit.GetComponent<Unit> ().currentPath = null;

		if (UnitCanEnterTile (x, y) == false) {
			//we probably click on a mountain or something, so just quit out
			return;

		}

		Dictionary<Node, float> dist = new Dictionary<Node, float> ();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node> (); 
		//Setup the Q - unvisited nodes
		List<Node> unvisited = new List<Node> ();

		Node source = graph [
			              selectedUnit.GetComponent<Unit> ().tileX, 
			              selectedUnit.GetComponent<Unit> ().tileY
		              ];

		Node goal = graph [
			            x,
			            y
		            ];

		dist [source] = 0;
		prev [source] = null;

		//Initialize everything Infinity distance
		//Possible some nodes can't be reached from source
		foreach (Node v in graph) {
			if (v != source) {
				dist [v] = Mathf.Infinity;
				prev [v] = null;

			}
			unvisited.Add (v);
		}


		while (unvisited.Count > 0) {
			//not fast, optimisation: use priority queue

			//u is going to be the unvisited node with the smallest distance
			Node u = null;

			foreach (Node possibleU in unvisited) {
				if (u == null || dist [possibleU] < dist [u]) {
					u = possibleU;
				}
			}

			if (u == goal) {
				break; //exit the while loop
			}

			unvisited.Remove (u);

			foreach (Node v in u.neighbors) {
				//float temp = dist[u] + u.DistanceTo(v);         //DistanceTo gives Euclidean distance
				float temp = dist [u] + CostToEnterTile (u.x, u.y, v.x, v.y);
				if (temp < dist [v]) {
					dist [v] = temp;
					prev [v] = u;
				}
			}
		}

		//if we get there, either we found the shortest route to our target, or there is no route at all to our target

		if (prev [goal] == null) {
			//no route between our target and the source
			return;
		}

		List<Node> currentPath = new List<Node> ();

		Node curr = goal;

		//step through the "prev" chain and add it to our path
		while (curr != null) {
			currentPath.Add (curr);
			curr = prev [curr];
		}

		//right now, currentPath describes a route from our target to our source
		// so we need to invert it

		currentPath.Reverse ();

		selectedUnit.GetComponent<Unit> ().currentPath = currentPath;

	}

	public void GeneratePathTo(int x, int y) {
		//clear out our unit's old path
		//selectedUnit.GetComponent<Unit>().currentPath = null;

		if (UnitCanEnterTile(x, y) == false) {
			//we probably clicked on a mountain or something, so just quit out
			print("error: cannot travel there");
			return;
		}

		//what is a dictionary
		Dictionary<Node, float> dist = new Dictionary<Node, float>();


		//AStar moved to separate file
		//Setup the Q - unvisited nodes
		SimplePriorityQueue<Node, float> fringe = new SimplePriorityQueue<Node, float>();

		List<Node> visited = new List<Node>();

		List<Node> currentPath = new List<Node>();

		Node start = graph[
			selectedUnit.GetComponent<Unit>().tileX,
			selectedUnit.GetComponent<Unit>().tileY
		];

		Node s = graph[
			selectedUnit.GetComponent<Unit>().tileX,
			selectedUnit.GetComponent<Unit>().tileY
		];

		Node goal = graph[
			x,
			y
		];


		dist[start] = 0;
		Node parent = start;
		float h = s.DistanceTo (goal);
		float f = dist[start] + h;
		fringe.Enqueue(start, f);

		//visited = null;

		while (fringe.Count > 0) {
			s = fringe.Dequeue();

			if (s == goal) {
				print("path found");
				return;
			}
				
 

			UnityEngine.Object[] objs = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			GameObject myObject = null;
			foreach (GameObject go in objs) {
				if (go.transform.position == TileCoordToWorldCoord(s.x,s.y)) {
					myObject = go;
					myObject.GetComponent<Renderer> ().material.color = Color.green;

					break;
				}
			}

			visited.Add(s);

		for (int i = 0; i < s.neighbors.Count; i++) { 
			Node sprime = s.neighbors [i];
				if (!visited.Contains(sprime)) {
					if (!fringe.Contains(sprime)) {
						dist[sprime] = Mathf.Infinity;
						parent = null;
					}

					//update vertex
					if(dist[s] + CostToEnterTile(s.x, s.y, sprime.x, sprime.y) < dist[sprime]) {

						dist[sprime] = dist[s] + CostToEnterTile(s.x, s.y, sprime.x, sprime.y);
						parent = s;
						currentPath.Add(parent);
//						selectedUnit.GetComponent<Unit>().currentPath = currentPath;

						if (fringe.Contains(sprime)) {
							
							fringe.Remove(sprime);
						}

						fringe.Enqueue(sprime, dist[sprime] + sprime.DistanceTo(goal));
					}
				}
			}
		}

		print("no path found");
		return;
	}


	// Update is called once per frame
	void Update () {
		
	}
}
