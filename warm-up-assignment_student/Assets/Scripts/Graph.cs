using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

    public int Size()
    {
        return adjacencyList.Count;
    }

    public T ReturnByIndex(int index)
    {
        if (index > adjacencyList.Count)
        {
            Debug.Log("Node does not exist in the graph.");
        }
        return adjacencyList.Keys.ElementAt(index);
    }


    public int BFS(T startNode)
    {
        T currentNode = startNode;
        List<T> fifo = new List<T>();
        List<T> discovered = new List<T>();
        fifo.Add(currentNode);
        discovered.Add(currentNode);
        while (fifo.Count != 0)
        {
            currentNode = fifo.First();
            fifo.Remove(currentNode);
            List<T> allNeighbors = GetNeighbors(currentNode);
            for (int i = 0; i < allNeighbors.Count; i++)
            {
                if (!discovered.Contains(allNeighbors[i]))
                {
                    fifo.Add(allNeighbors[i]);
                    discovered.Add(allNeighbors[i]);
                }
            }
        }
        return discovered.Count;
    }

    public int DFS(T startNode)
    {
        T currentNode = startNode;
        List<T> fifo = new List<T>();
        List<T> discovered = new List<T>();
        fifo.Add(currentNode);
        discovered.Add(currentNode);
        while (fifo.Count != 0)
        {
            currentNode = fifo.Last();
            fifo.Remove(currentNode);
            List<T> allNeighbors = GetNeighbors(currentNode);
            for (int i = 0; i < allNeighbors.Count; i++)
            {
                if (!discovered.Contains(allNeighbors[i]))
                {
                    fifo.Add(allNeighbors[i]);
                    discovered.Add(allNeighbors[i]);

                }
            }
        }
        return discovered.Count;
    }
}
