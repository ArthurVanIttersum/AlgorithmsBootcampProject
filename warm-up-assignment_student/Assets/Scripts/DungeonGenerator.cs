using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public RectInt startSquare = new RectInt(10, 0, 100, 100);
    
    public List<RectInt> rooms = new List<RectInt>();
    public List<Color> colors = new List<Color>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rooms.Add(startSquare);
        //SplitRoom(rooms[0]);
        //SplitRoom(rooms[1]);
        //SplitRoom(rooms[2]);
        //SplitRoom(rooms[3]);
        //SplitRoom(rooms[4]);
        //SplitRoom(rooms[5]);
        //SplitRoom(rooms[6]);
        //SplitRoom(rooms[7]);
        //SplitRoom(rooms[8]);
        //SplitRoom(rooms[9]);
        for (int i = 0; i < 1000; i++)
        {
            SplitRoom(rooms[i]);
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            colors.Add(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(rooms[i], colors[i]);
            //AlgorithmsUtils.DebugRectInt(rooms[i], colors[i], 0, false, i * 10);


            //RectInt smaller = new RectInt(rooms[i].position.x + 1, rooms[i].position.y + 1, rooms[i].size.x - 2, rooms[i].size.y - 2);
            //AlgorithmsUtils.DebugRectInt(smaller, colors[i],0,false,i * 10);
        }
        
    }
    void SplitRoom(RectInt roomToSplit)
    {
        if (Random.Range(0, 2) == 0)
        {
            SplitRoomHorizontal(roomToSplit);
        }
        else
        {
            SplitRoomVertical(roomToSplit);
        }
    }

    void SplitRoomHorizontal(RectInt roomToSplit)
    {

        if (roomToSplit.width < 10)
        {
            return;
        }
        int splitDistance = Random.Range(3, roomToSplit.width - 3);


        Vector2Int newSize1 = new Vector2Int(splitDistance, roomToSplit.size.y);
        Vector2Int newSize2 = new Vector2Int(roomToSplit.size.x - splitDistance, roomToSplit.size.y);
        Vector2Int newPos  = new Vector2Int(splitDistance + roomToSplit.position.x, roomToSplit.position.y);
        RectInt split1 = new RectInt(roomToSplit.position, newSize1);
        RectInt split2 = new RectInt(newPos, newSize2);
        rooms.Add(split1);
        rooms.Add(split2);

    }
    void SplitRoomVertical(RectInt roomToSplit)
    {
        if (roomToSplit.height < 10)
        {
            return;
        }
        int splitDistance = Random.Range(3, roomToSplit.height - 3);


        Vector2Int newSize1 = new Vector2Int(roomToSplit.size.x, splitDistance);
        Vector2Int newSize2 = new Vector2Int(roomToSplit.size.x, roomToSplit.size.y - splitDistance);
        Vector2Int newPos = new Vector2Int(roomToSplit.position.x, splitDistance + roomToSplit.position.y);
        RectInt split1 = new RectInt(roomToSplit.position, newSize1);
        RectInt split2 = new RectInt(newPos, newSize2);
        rooms.Add(split1);
        rooms.Add(split2);
    }
}
