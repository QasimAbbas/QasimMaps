using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public int tileX;
    public int tileY;
    public TileMap map;

    public List<Node> currentPath = null;
    int moveSpeed = 2;
    void Update() {
        if (currentPath != null) {
            int currNode = 0;
            while (currNode < currentPath.Count-1) {
                Vector3 start = map.TileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].y) 
                    + new Vector3(0, 0, -1f);
                Vector3 end = map.TileCoordToWorldCoord(currentPath[currNode+1].x, currentPath[currNode+1].y) 
                    + new Vector3(0, 0, -1f);


                
                Debug.DrawLine(start, end, Color.red);

                currNode++;
            }



        }


    }
//	//printCurrentPath prints the currentPath node list 
//	public void printCurrentPath()
//	{
//		if (currentPath != null) 
//		{
//			
//			using (System.IO.StreamWriter file = 
//				new System.IO.StreamWriter (@"/Users/qasimabbas/Documents/QasimMaps/AiProject/listOfNodes.txt")) 
//			{
//				file.writeLine("Current Path:");
//				foreach (Node n in currentPath) 
//				{
//					file.WriteLine (n.x + " " + n.y);
//				}
//			}
//		}
//		return;
//	}
//
    public void MoveNextTile() {
        float remainingMovement = moveSpeed;
        while (remainingMovement > 0) {

            if (currentPath == null) {
                return;
            }

            //Get cost from current tile to next tile
            remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y);
            //Remove old currentNode/firstNode from path
            //currentPath.RemoveAt(0);

			//Remove the old current tile
            tileX = currentPath[1].x;
            tileY = currentPath[1].y;
            transform.position = map.TileCoordToWorldCoord(tileX, tileY);	//update our unity world position

			currentPath.RemoveAt(0);

			if (currentPath.Count == 1) {
                //Only one tile left
                //DESTINATION
                currentPath = null;
            }
        }
    }
}
