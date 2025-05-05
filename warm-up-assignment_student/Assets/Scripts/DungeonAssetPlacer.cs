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
    public HashSet<Vector2> wallPositions = new HashSet<Vector2>();
    public NavMeshSurface navMeshSurface;
    private Vector3 wallOffset = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 floorOffset = new Vector3(0.5f, 0, 0.5f);
    private Vector2 gridspacePosition;
    private Vector3 worldSpacePosition;
    public void PlaceAssets()
    {
        //walls
        GameObject wallParrent = Instantiate(EmptyGameObjectPrefab, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < dungeonGenerator.rooms.Count; i++)
        {
            RectInt room = dungeonGenerator.rooms[i];
            for (int j = 0; j < room.width; j++)
            {
                wallPositions.Add(room.min + new Vector2(j, 0));
                wallPositions.Add(room.min + new Vector2(j, room.height - 1));
            }
            for (int j = 0; j < room.height; j++)
            {
                wallPositions.Add(room.min + new Vector2(0, j));
                wallPositions.Add(room.min + new Vector2(room.width - 1, j));
            }
        }
        for (int i = 0; i < dungeonGenerator.doors.Count; i++)
        {
            wallPositions.Remove(dungeonGenerator.doors[i].position);
        }
        
        foreach (var item in wallPositions)
        {
            gridspacePosition = item;
            worldSpacePosition = new Vector3(gridspacePosition.x, 0, gridspacePosition.y) + wallOffset;
            Instantiate(wallPrefab, worldSpacePosition, Quaternion.identity, wallParrent.transform);
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
                    gridspacePosition = new Vector2(j, k) + room.min;
                    worldSpacePosition = new Vector3(gridspacePosition.x, 0, gridspacePosition.y) + floorOffset;
                    Instantiate(floorPrefab, worldSpacePosition, Quaternion.identity, floorParrent.transform);
                }
            }
        }
        for (int i = 0; i < dungeonGenerator.doors.Count; i++)
        {
            gridspacePosition = dungeonGenerator.doors[i].position;
            worldSpacePosition = new Vector3(gridspacePosition.x, 0, gridspacePosition.y) + floorOffset;
            Instantiate(floorPrefab, worldSpacePosition, Quaternion.identity, floorParrent.transform);
        }
        
    }
    public void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
