using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class TileMap : MonoBehaviour {
    public GameObject selectedUnit;
    
    public TileType[] tileTypes;

    int[,] tiles;
    Node[,] graph;

    //List<Node> currentPath = null;

    int mapSizeX = 10;
    int mapSizeY = 10;

    // Use this for initialization
    void Start() {
        //set up the selectedUnit's variable
        selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().tileY = (int) selectedUnit.transform.position.y;
        selectedUnit.GetComponent<Unit>().map = this;

        GenerateMapData();
        GeneratePathfindingGraph();
        GenerateMapVisual();
    }

    void GenerateMapData() {
        //allocate our map tiles
        tiles = new int[mapSizeX, mapSizeY];
        int x, y;
        //initialise our map tiles
        for(x = 0; x < mapSizeX; x++) {
            for(y = 0; y < mapSizeY; y++) {
                tiles[x, y] = 0;
            }
        }

        //swamp area
        for(x = 3; x <= 5; x++) {
            for(y = 0; y < 4; y++) {
                tiles[x, y] = 1;
            }
        }

        //u shaped mountain range
        tiles[4, 4] = 2;
        tiles[5, 4] = 2;
        tiles[6, 4] = 2;
        tiles[7, 4] = 2;
        tiles[8, 4] = 2;
        tiles[8, 5] = 2;
        tiles[4, 5] = 2;
        tiles[4, 6] = 2;
        tiles[8, 6] = 2;

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
            cost += 0.001f;

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

    public void GeneratePathTo(int x, int y) {
        //clear out our unit's old path
        selectedUnit.GetComponent<Unit>().currentPath = null;

		if (UnitCanEnterTile(x, y) == false) {
			//we probably click on a mountain or something, so just quit out
			return;

		}

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>(); 
        //Setup the Q - unvisited nodes
        List<Node> unvisited = new List<Node>();

        Node source = graph[
                            selectedUnit.GetComponent<Unit>().tileX, 
                            selectedUnit.GetComponent<Unit>().tileY
                            ];

        Node goal = graph[
                            x,
                            y
                            ];

        dist[source] = 0;
        prev[source] = null;

        //Initialize everything Infinity distance
        //Possible some nodes can't be reached from source
        foreach(Node v in graph) {
            if(v != source) {
                dist[v] = Mathf.Infinity;
                prev[v] = null;

            }
            unvisited.Add(v);
        }


        while(unvisited.Count > 0) {
            //not fast, optimisation: use priority queue

            //u is going to be the unvisited node with the smallest distance
            Node u = null;

            foreach(Node possibleU in unvisited) {
                if (u == null || dist[possibleU] < dist[u]) {
                    u = possibleU;
                }
            }
                
            if (u == goal) {
                break; //exit the while loop
            }

            unvisited.Remove(u);

            foreach(Node v in u.neighbors) {
                //float temp = dist[u] + u.DistanceTo(v);         //DistanceTo gives Euclidean distance
                float temp = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
                if (temp < dist[v]) {
                    dist[v] = temp;
                    prev[v] = u;
                }
            }
        }

        //if we get there, either we found the shortest route to our target, or there is no route at all to our target

        if (prev[goal] == null) {
            //no route between our target and the source
            return;
        }

        List<Node> currentPath = new List<Node>();

        Node curr = goal;

        //step through the "prev" chain and add it to our path
        while (curr != null) {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        //right now, currentPath describes a route from our target to our source
        // so we need to invert it

        currentPath.Reverse();

        selectedUnit.GetComponent<Unit>().currentPath = currentPath;
    
    }

	// Update is called once per frame
	void Update () {
		
	}
}
