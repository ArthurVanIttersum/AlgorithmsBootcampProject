using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public RectInt startSquare = new RectInt(10, 0, 100, 100);
    public int steps;
    
    public List<RectInt> rooms = new List<RectInt>();
    public List<Color> colors = new List<Color>();
    public List<RectInt> doors = new List<RectInt>();
    
    
    public float cooldown;
    int completedRooms = 0;
    public bool displayDictionary;

    Graph<RectInt> graph = new Graph<RectInt>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        
        rooms.Add(startSquare);
        colors.Add(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f));
        for (int i = 0; i < steps; i++)
        {
            if (rooms.Count <= completedRooms)
            {
                break;
            }
            if (SplitRoom(rooms[completedRooms]))
            {
                colors.Add(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f));
                colors.Add(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f));
                colors.Remove(colors[completedRooms]);
                yield return new WaitForSeconds(cooldown);
            }
            else
            {
                completedRooms++;
            }
        }
        yield return new WaitForSeconds(cooldown);
        print("rooms done");
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = i; j < rooms.Count; j++)
            {
                if (i != j)
                {
                    if (AlgorithmsUtils.Intersects(rooms[i], rooms[j]))
                    {
                        RectInt intersectArea = AlgorithmsUtils.Intersect(rooms[i], rooms[j]);
                        if (intersectArea.width * intersectArea.height > 2)
                        {
                            MakeDoor(intersectArea);
                        }

                    }
                }
            } 
        }
        yield return new WaitForSeconds(cooldown);
        print("doors done");
        for (int i = 0; i < rooms.Count; i++)
        {
            graph.AddNode(rooms[i]);
        }
        for (int i = 0; i < doors.Count; i++)
        {
            graph.AddNode(doors[i]);
        }
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = 0; j < doors.Count; j++)
            {
                if (AlgorithmsUtils.Intersects(rooms[i], doors[j]))
                {
                    graph.AddEdge(rooms[i], doors[j]);
                }
            }
        }
        Debug.Log("Graph Structure:");
        graph.PrintGraph();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            //RectInt withwalls = new RectInt(rooms[i].position, new Vector2Int(rooms[i].size.x + 1, rooms[i].size.y + 1));
            AlgorithmsUtils.DebugRectInt(rooms[i], colors[i]);
            //AlgorithmsUtils.DebugRectInt(rooms[i], colors[i], 0, false, i * 10);


            //RectInt smaller = new RectInt(rooms[i].position.x + 1, rooms[i].position.y + 1, rooms[i].size.x - 2, rooms[i].size.y - 2);
            //AlgorithmsUtils.DebugRectInt(smaller, colors[i],0,false,i * 10);
        }
        
        for (int i = 0; i < doors.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(doors[i], Color.red);
        }

        if (displayDictionary)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                List<RectInt> neighbors = graph.GetNeighbors(rooms[i]);
                for (int j = 0; j < neighbors.Count; j++)
                {
                    Vector3 arrowStart = new Vector3(rooms[i].center.x, 0, rooms[i].center.y);
                    Vector3 arrowEnd = new Vector3(neighbors[j].center.x, 0, neighbors[j].center.y) - arrowStart;
                    DebugExtension.DebugArrow(arrowStart, arrowEnd, Color.magenta);
                }
            }
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

        Vector2Int splitTop = new Vector2Int(roomToSplit.xMin + splitDistance + 1, roomToSplit.yMax);
        Vector2Int splitBottom = new Vector2Int(roomToSplit.xMin + splitDistance, roomToSplit.yMin);

        RectInt splitroom1 = TwoPosToRectInt(roomToSplit.min, splitTop);
        RectInt splitroom2 = TwoPosToRectInt(splitBottom, roomToSplit.max);

        rooms.Add(splitroom1);
        rooms.Add(splitroom2);
        rooms.Remove(roomToSplit);
        return true;
    }
    bool SplitRoomVertical(RectInt roomToSplit)
    {
        if (roomToSplit.height < 10)
        {
            return false;
        }
        int splitDistance = Random.Range(3, roomToSplit.height - 3);


        Vector2Int splitRight = new Vector2Int(roomToSplit.xMax, roomToSplit.yMin + splitDistance + 1);
        Vector2Int splitLeft = new Vector2Int(roomToSplit.xMin, roomToSplit.yMin + splitDistance);

        RectInt splitroom1 = TwoPosToRectInt(roomToSplit.min, splitRight);
        RectInt splitroom2 = TwoPosToRectInt(splitLeft, roomToSplit.max);

        rooms.Add(splitroom1);
        rooms.Add(splitroom2);
        rooms.Remove(roomToSplit);

        return true;
    }

    RectInt TwoPosToRectInt(Vector2Int low, Vector2Int high)
    {
        RectInt newRoom = new RectInt();
        newRoom.SetMinMax(low, high);
        return newRoom;
    }

    private void MakeDoor(RectInt intersectArea)
    {
        RectInt doorArea;
        if (intersectArea.width == 1)
        {
            int doorPosition = Random.Range(1, intersectArea.height - 2);
            doorArea = new RectInt(new Vector2Int(intersectArea.xMin, intersectArea.yMin + doorPosition) , new Vector2Int(1,1));
            doors.Add(doorArea);
        }
        else
        {
            int doorPosition = Random.Range(1, intersectArea.width - 2);
            doorArea = new RectInt(new Vector2Int(intersectArea.xMin + doorPosition, intersectArea.yMin), new Vector2Int(1, 1));
            doors.Add(doorArea);
        }
        
    }


}
