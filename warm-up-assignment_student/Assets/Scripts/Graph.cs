using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph<T>
{
    private Dictionary<T, List<T>> adjacencyList;
    public Graph() { adjacencyList = new Dictionary<T, List<T>>(); }
    public void AddNode(T node)
    {
        if (!adjacencyList.ContainsKey(node))
        {
            adjacencyList[node] = new List<T>();
        }
    }
    public void AddEdge(T fromNode, T toNode)
    {
        if (!adjacencyList.ContainsKey(fromNode) || !adjacencyList.ContainsKey(toNode))
        {
            Debug.Log("One or both nodes do not exist in the graph.");
            return;
        }
        adjacencyList[fromNode].Add(toNode);
        adjacencyList[toNode].Add(fromNode);
    }
    public List<T> GetNeighbors(T node)
    {
        if (!adjacencyList.ContainsKey(node))
        {
            Debug.Log("Node does not exist in the graph.");
        }


        return adjacencyList[node];
    }

    public void PrintGraph()
    {
        List<T> nodes = adjacencyList.Keys.ToList();
        List<T> ToPrint = new List<T>();
        string toPrint = "";

        for (int i = 0; i < adjacencyList.Count; i++)
        {
            
            toPrint = toPrint + "\n" + nodes[i] + "\n";

            List<T> neighbors = GetNeighbors(nodes[i]);
            for (int j = 0; j < neighbors.Count; j++)
            {
                
                toPrint = toPrint + " " + neighbors[j];

            }
        }
        
        Debug.Log(toPrint);
        
        
        
    }
}
