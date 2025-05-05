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
    

    Graph<RectInt> graph = new Graph<RectInt>();
    public DungeonAssetPlacer assetPlacer;
    
    IEnumerator Start()
    {
        //start of the algorithm
        GenerateRoomData();
        yield return new WaitForSeconds(cooldown);
        print("rooms done");
        //make doors
        GenerateDoorData();
        yield return new WaitForSeconds(cooldown);
        print("doors done");
        //add all doors to the graph
        for (int i = 0; i < rooms.Count; i++)
        {
            graph.AddNode(rooms[i]);
        }
        //add all doors to the graph
        for (int i = 0; i < doors.Count; i++)
        {
            graph.AddNode(doors[i]);
        }
        //find which doors and which rooms belong together and add this data to the graph
        ConnectNodes();
        yield return new WaitForSeconds(cooldown);
        
        Debug.Log("Graph Structure:");
        graph.PrintGraph();//function that prints all the data in the graph
        print("NumberOfRooms:");
        print(rooms.Count);
        print("NumberOfDoors:");
        print(doors.Count);
        print("NumberOfNodes:");
        print(graph.BFS(rooms[0]));
        print("NumberOfRooms + NumberOfDoors - NumberofNodes:");
        print(rooms.Count + doors.Count - graph.BFS(rooms[0]));
        yield return new WaitForSeconds(cooldown);
        assetPlacer.PlaceAssets();
    }

    void Update()
    {
        //displayrooms
        for (int i = 0; i < rooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(rooms[i], colors[i]);
        }

        //display doors
        for (int i = 0; i < doors.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(doors[i], Color.red);
        }

        //display the graph
        for (int i = 0; i < graph.Size(); i++)
        {
            RectInt currentRoom = graph.ReturnByIndex(i);
            List<RectInt> neighbors = graph.GetNeighbors(currentRoom);
            for (int j = 0; j < neighbors.Count; j++)
            {
                Vector3 arrowStart = new Vector3(currentRoom.center.x, 0, currentRoom.center.y);
                Vector3 arrowEnd = new Vector3(neighbors[j].center.x, 0, neighbors[j].center.y) - arrowStart;
                DebugExtension.DebugArrow(arrowStart, arrowEnd, Color.magenta);
            }

        }

    }

    void GenerateRoomData()
    {
        rooms.Add(startSquare);
        colors.Add(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f));
        for (int i = 0; i < steps; i++)//keep splitting rooms untill all rooms cannot be split anymore
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
            }
            else
            {
                completedRooms++;
            }
        }
    }

    void GenerateDoorData()
    {
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
    }

    void ConnectNodes()
    {
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
    }


    bool SplitRoom(RectInt roomToSplit)
    {//controlls the splitting of the room. chooses whether to split horizontally or vertically and handles cases when the room was too small to split
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

    bool SplitRoomHorizontal(RectInt roomToSplit)//splits room horizontally
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
    bool SplitRoomVertical(RectInt roomToSplit)//splits room vertically
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

    RectInt TwoPosToRectInt(Vector2Int low, Vector2Int high)//creates a room out of the two corner positions.
    {
        RectInt newRoom = new RectInt();
        newRoom.SetMinMax(low, high);
        return newRoom;
    }

    private void MakeDoor(RectInt intersectArea)//makes a door
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
