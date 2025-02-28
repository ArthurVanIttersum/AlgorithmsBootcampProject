using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public RectInt test1 = new RectInt(0, 0, 100, 50);
    public RectInt test2 = new RectInt(0, 50, 100, 50);
    private List<RectInt> rooms = new List<RectInt>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rooms.Add(test1);
        rooms.Add(test2);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(rooms[i], Color.red);
        }
        
    }
}
