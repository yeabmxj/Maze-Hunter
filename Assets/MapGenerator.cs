using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0,100)]
    public int fillPercent;

    int [,] map;

    void Start() {
        GenerateMap();
    }

    void Update() {
        if (Input.GetMouseButton(0)) { GenerateMap();}
    }

    void GenerateMap() { 
        map = new int[width,height];
        RandomFillMap();

        for (int i = 0; i < 5; i++) {
            SmoothMap();
        }

        MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
        meshGenerator.GenerateMesh(map, 1);

    }

    void RandomFillMap() {
        if (useRandomSeed) { seed = Time.time.ToString();}
        //psudo-random number generator
        System.Random psudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                map[x,y] = (x == 0 || x == width - 1 || y == 0 || y == height - 1) ? 1 : (psudoRandom.Next(0,100) < fillPercent)? 1 : 0;
            }
        }
    }

    void SmoothMap() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighborWallTiles = GetSurroundingWallCount(x,y);
                map[x,y] = (neighborWallTiles > 4)? 1 : (neighborWallTiles < 4)? 0 : map[x,y];
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY) {
        int wallcount = 0;
        
        for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++) {
            for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++) {
                if (neighborX >= 0 && neighborX < width && neighborY >=0 && neighborY < height) {
                    if (neighborX != gridX || neighborY != gridY) {
                        wallcount+=map[neighborX,neighborY];
                    }
                }
                else {
                    wallcount++;
                }
            }   
        }
        return wallcount;
    }

    void OnDrawGizmos() {
    //     if (map != null) {
    //         for (int x = 0; x < width; x++) {
    //             for (int y = 0; y < height; y++) {
    //                 Gizmos.color = (map[x,y] == 1)? Color.black : Color.white;
    //                 Vector3 position = new Vector3(-width/2 + x + .5f, -height/2 + y + .5f);
    //                 Gizmos.DrawCube(position,Vector3.one);
    //             }
    //         }
    //     }
    // }
    }

}