using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public RectInt startSquare = new RectInt(10, 0, 100, 100);
    
    public List<RectInt> rooms = new List<RectInt>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rooms.Add(startSquare);
        SplitRoom(rooms[0]);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(rooms[i], Color.red);
        }
        
    }

    void SplitRoom(RectInt roomToSplit)
    {
        //first only horizontally
        int splitDistance = roomToSplit.width / 2;
        Vector2Int newSize = new Vector2Int(splitDistance, roomToSplit.size.y);
        Vector2Int newPos  = new Vector2Int(splitDistance + roomToSplit.position.x, roomToSplit.position.y);
        RectInt split1 = new RectInt(roomToSplit.position, newSize);
        RectInt split2 = new RectInt(newPos, newSize);
        rooms.Add(split1);
        rooms.Add(split2);
    }
}
