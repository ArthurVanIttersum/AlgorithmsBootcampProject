using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public RectInt startSquare = new RectInt(10, 0, 100, 100);
    
    public List<RectInt> rooms = new List<RectInt>();
    public List<Color> colors = new List<Color>();
    public float cooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        
        rooms.Add(startSquare);
        colors.Add(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f));
        for (int i = 0; i < 100000; i++)
        {
            if (rooms.Count > i)
            {
                if (SplitRoom(rooms[i]))
                {
                    colors.Add(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f));
                    colors.Add(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f));
                    yield return new WaitForSeconds(cooldown);
                }
                
            }
            else
            {
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            RectInt withwalls = new RectInt(rooms[i].position, new Vector2Int(rooms[i].size.x + 1, rooms[i].size.y + 1));
            AlgorithmsUtils.DebugRectInt(withwalls, colors[i]);
            //AlgorithmsUtils.DebugRectInt(rooms[i], colors[i], 0, false, i * 10);


            //RectInt smaller = new RectInt(rooms[i].position.x + 1, rooms[i].position.y + 1, rooms[i].size.x - 2, rooms[i].size.y - 2);
            //AlgorithmsUtils.DebugRectInt(smaller, colors[i],0,false,i * 10);
        }
        
    }
    bool SplitRoom(RectInt roomToSplit)
    {

        if (Random.Range(0, 2) == 0)
        {
            if (!SplitRoomHorizontal(roomToSplit))
            {
                return SplitRoomVertical(roomToSplit);
            }
            return true;
        }
        else
        {
            if (!SplitRoomVertical(roomToSplit))
            {
                return SplitRoomHorizontal(roomToSplit);
            }
            return true;
        }
    }

    bool SplitRoomHorizontal(RectInt roomToSplit)
    {

        if (roomToSplit.width < 10)
        {
            return false;
        }
        int splitDistance = Random.Range(3, roomToSplit.width - 3);


        Vector2Int newSize1 = new Vector2Int(splitDistance, roomToSplit.size.y);
        Vector2Int newSize2 = new Vector2Int(roomToSplit.size.x - splitDistance, roomToSplit.size.y);
        Vector2Int newPos  = new Vector2Int(splitDistance + roomToSplit.position.x, roomToSplit.position.y);
        RectInt split1 = new RectInt(roomToSplit.position, newSize1);
        RectInt split2 = new RectInt(newPos, newSize2);
        rooms.Add(split1);
        rooms.Add(split2);
        return true;
    }
    bool SplitRoomVertical(RectInt roomToSplit)
    {
        if (roomToSplit.height < 10)
        {
            return false;
        }
        int splitDistance = Random.Range(3, roomToSplit.height - 3);


        Vector2Int newSize1 = new Vector2Int(roomToSplit.size.x, splitDistance);
        Vector2Int newSize2 = new Vector2Int(roomToSplit.size.x, roomToSplit.size.y - splitDistance);
        Vector2Int newPos = new Vector2Int(roomToSplit.position.x, splitDistance + roomToSplit.position.y);
        RectInt split1 = new RectInt(roomToSplit.position, newSize1);
        RectInt split2 = new RectInt(newPos, newSize2);
        rooms.Add(split1);
        rooms.Add(split2);
        return true;
    }
}
