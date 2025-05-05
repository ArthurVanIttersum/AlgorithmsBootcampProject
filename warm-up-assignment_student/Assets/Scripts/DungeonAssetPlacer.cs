using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

public class DungeonAssetPlacer : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject EmptyGameObjectPrefab;
    public GameObject floorPrefab;
    public DungeonGenerator dungeonGenerator;
    public HashSet<Vector3> wallPositions = new HashSet<Vector3>();
    public NavMeshSurface navMeshSurface;
    public void PlaceAssets()
    {
        //walls
        GameObject wallParrent = Instantiate(EmptyGameObjectPrefab, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < dungeonGenerator.rooms.Count; i++)
        {
            RectInt room = dungeonGenerator.rooms[i];
            for (int j = 0; j < room.width; j++)
            {
                Vector2 pos1 = room.min + new Vector2(j, 0);
                Vector3 pos1b = new Vector3(pos1.x, 0, pos1.y) + new Vector3(0.5f, 0.5f, 0.5f);
                wallPositions.Add(pos1b);

                Vector2 pos2 = room.min + new Vector2(j, room.height - 1);
                Vector3 pos2b = new Vector3(pos2.x, 0, pos2.y) + new Vector3(0.5f, 0.5f, 0.5f);
                wallPositions.Add(pos2b);

            }
            for (int j = 0; j < room.height; j++)
            {
                Vector2 pos1 = room.min + new Vector2(0, j);
                Vector3 pos1b = new Vector3(pos1.x, 0, pos1.y) + new Vector3(0.5f, 0.5f, 0.5f);
                wallPositions.Add(pos1b);

                Vector2 pos2 = room.min + new Vector2(room.width - 1, j);
                Vector3 pos2b = new Vector3(pos2.x, 0, pos2.y) + new Vector3(0.5f, 0.5f, 0.5f);
                wallPositions.Add(pos2b);

            }
        }
        for (int i = 0; i < dungeonGenerator.doors.Count; i++)
        {
            wallPositions.Remove(new Vector3(dungeonGenerator.doors[i].position.x, 0, dungeonGenerator.doors[i].position.y) + new Vector3(0.5f, 0.5f, 0.5f));
        }
        
        Vector3[] hashData = wallPositions.ToArray();
        

        
        for (int i = 0; i < hashData.Length; i++)
        {
            Instantiate(wallPrefab, hashData[i], Quaternion.identity, wallParrent.transform);
        }

        //floor
        GameObject floorParrent = Instantiate(EmptyGameObjectPrefab, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < dungeonGenerator.rooms.Count; i++)
        {
            RectInt room = dungeonGenerator.rooms[i];
            for (int j = 1; j < room.width - 1; j++)
            {
                for (int k = 1; k < room.height - 1; k++)
                {
                    Instantiate(floorPrefab, new Vector3(j + room.xMin, 0, k + room.yMin) + new Vector3(0.5f, 0, 0.5f), Quaternion.identity, floorParrent.transform);
                }
            }
        }
        for (int i = 0; i < dungeonGenerator.doors.Count; i++)
        {
            Instantiate(floorPrefab, new Vector3(dungeonGenerator.doors[i].position.x, 0, dungeonGenerator.doors[i].position.y) + new Vector3(0.5f, 0, 0.5f), Quaternion.identity, floorParrent.transform);
        }
        
    }
    public void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
