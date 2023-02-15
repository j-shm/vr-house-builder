using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallImporter : MonoBehaviour
{   
    [SerializeField]
    private GameObject wall;
    void Start()
    {
        List<int> example = new List<int>{
						1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
						1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
                        };
        PlaceWalls(Import(example,16),16);
    }

    private void PlaceWalls(List<string> walls, int GridSize) {
        foreach(string wall in walls ) {
            Debug.Log(wall);
        }
        for(int i =0; i< walls.Count;i++) {
            if(walls[i] != "o") {
                if(walls[i] == "|" || walls[i] == "-") {
                    var thiswall = Instantiate(wall,
                    new Vector3 (i%GridSize,0,GridSize - i/GridSize), 
                    Quaternion.Euler(new Vector3(0,walls[i] == "|" ? 90 : 0,0))
                    );
                    thiswall.name = i.ToString();
                } else {
                    if(walls[i] == "┌") {
                        Instantiate(wall,
                        new Vector3 (i%GridSize,0,GridSize - i/GridSize), 
                        Quaternion.Euler(new Vector3(0,90,0))
                        );
                        Instantiate(wall,
                        new Vector3 (i%GridSize,0,GridSize - i/GridSize), 
                        Quaternion.Euler(new Vector3(0,0,0))
                        );
                    } else if(walls[i] == "┐") {
                        Instantiate(wall,
                        new Vector3 (GridSize-1,0,GridSize), 
                        Quaternion.Euler(new Vector3(0,90,0))
                        );
                    } else if(walls[i] == "└") {
                        Instantiate(wall,
                        new Vector3 (i%GridSize,0,GridSize - i/GridSize), 
                        Quaternion.Euler(new Vector3(0,0,0))
                        );
                    }
                }
            }
        }
        
    }

    private List<string> Import(List<int> walls, int GridSize) {
        List<string> stringedwalls = new List<string>();
        for(int i=0; i< walls.Count; i++) {
            int right = i+1;
            int left = i-1;
            int down = i+(GridSize);
            int up = i-(GridSize);


            //only works for a square

            if(walls[i] == 0) {
                stringedwalls.Add("o");
                continue;
            }

            if((left >= 0 && walls[left] == 1) && (left%GridSize != 15) && (down < walls.Count && walls[down] == 1)) {
                stringedwalls.Add("┐");
                continue;
            }

            if((left >= 0 && walls[left] == 1) && (left%GridSize != 15) && (up >= 0 && walls[up] == 1)) {
                stringedwalls.Add("┘");
                continue;
            }

            if((right < walls.Count && walls[right] == 1) && (right%GridSize != 0) && (down < walls.Count && walls[down] == 1)) {
                stringedwalls.Add("┌");
                continue;
            }

            if((right < walls.Count && walls[right] == 1) && (right%GridSize != 0) && (up >= 0 && walls[up] == 1)) {
                stringedwalls.Add("└");
                continue;
            }

            if(right < walls.Count && walls[right] == 0) {
                stringedwalls.Add("|");
                continue;
            }

            if((left >= 0 && walls[left] == 0)) {
                stringedwalls.Add("|");
                continue;
            }

            if(down < walls.Count && walls[down] == 0) {
                stringedwalls.Add("-");
                continue;
            }

            if(up >= 0 && walls[up] == 0) {
                stringedwalls.Add("-");
                continue;
            }
            
            stringedwalls.Add("z");
        }
        foreach(string wall in stringedwalls) {
            if(wall == "z") {
                Debug.LogError("problem with walls.");
            }
            Debug.Log(stringedwalls);
        }
        return stringedwalls;
    }

}
