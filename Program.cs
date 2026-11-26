/*
A1. 
0 -> [1, 4]
1 -> [0, 2, 3]
2 -> [1, 3]
3 -> [1, 2, 4]
4 -> [0, 3, 5]
5 -> [4]

A2.
    0. 1. 2. 3. 4. 5   
0   0  1. 0  0  1. 0
1.  1. 0. 1. 1. 0. 0
2.  0. 1. 0. 1. 0  0
3.  0. 1. 1. 0. 1. 0
4.  1. 0. 0. 1. 0. 1
5.  0. 0. 0. 0. 1. 0
2E = 2(7) = 14, Every edge is counted twice

A3. 
Vertex.  Degree
0        2
1        3
2        2
3        3
4        3
5        1
2 + 3 + 2 + 3 + 3 + 1 = 14

A4. 
Step    Removed     Added   Queue
i       -           0       [0]
1       0           1, 4    [1, 4] 
2       1           2, 3    [4, 2, 3]   
3       4           5       [2, 3, 5]
4       2           none    [3, 5]
5       3           none    [5]
6       5           none    []
BFS Order: 0, 1, 4, 2, 3, 5

A5.
0 -> 1
1 -> skip 0, 2
2 -> skip 1, 3
3 -> skip 1 & 2, 4
4 -> skip 0 & 3, 5
5 -> skip 4, return

DFS Order: 0, 1, 2, 3 ,4, 5

Call Stack:
DFS(5) - Top
DFS(4)
DFS(3)
DFS(2)
DFS(1) 
DFS(0) - Bottom

A6.
2 - 3 - 4 - 5
BFS finds the shortest path because it visits verticies according to distance from the starting vertex.

A7.
1 -> 2 -> 3 -> 1

A8. 
0 -> [1, 4]
1 -> [0, 2]
2 -> [1]
4 -> [0, 5]
5 -> [4]

2 -> 1 -> 0 -> 4 -> 5

Yes, the graph is still connected because all of the vertices can still be reached

A9. 
Adjacency matrix: 10000 x 10000 = 100000000 entries
Adjacency list = 10000 x 30 = 300000 neighbor entires

4-byte integers:
Matrix: 400 mb
List: 1.2 mb

Adjacency list would be better because the graph is spread out. Each student follows 30 students, so adjacency matrices would waste memory.

*/

using System;

public class Program
{
    static void Main()
    {
        Graph graph = new Graph();

        graph.AddEdge(0, 1);
        graph.AddEdge(1, 2);
        graph.AddEdge(0, 3);
        graph.AddEdge(3, 4);
        graph.AddEdge(1, 4);
        graph.AddEdge(6, 7);
        graph.AddEdge(4, 6);
        graph.AddEdge(2, 5);
        graph.AddEdge(4, 5);
        graph.AddVertex(8);

        Console.WriteLine("Initial Graph:");
        graph.Print();

        Console.WriteLine("\nBFS(0): " +
            string.Join(", ", graph.BFS(0)));

        Console.WriteLine("DFS(0): " +
            string.Join(", ", graph.DFS(0)));

        Console.WriteLine("BFS(42): [" +
            string.Join(", ", graph.BFS(42)) + "]");

        Console.WriteLine("ShortestPath(0, 7): " +
            string.Join(", ", graph.ShortestPath(0, 7)));

        Console.WriteLine("ShortestPath(0, 8): [" +
            string.Join(", ", graph.ShortestPath(0, 8)) + "]");

        Console.WriteLine("HasEdge(4, 1): " + graph.HasEdge(4, 1));
        Console.WriteLine("CountPieces(): " + graph.CountPieces());
        Console.WriteLine("HasCycle(): " + graph.HasCycle());

        graph.AddEdge(5, 4);
        graph.AddEdge(7, 7);

        Console.WriteLine("\nGraph after adding invalid edges:");
        graph.Print();

        graph.RemoveEdge(1, 4);
        graph.RemoveEdge(4, 5);

        Console.WriteLine("\nAfter removing edges:");
        Console.WriteLine("HasCycle(): " + graph.HasCycle());

        graph.RemoveVertex(4);

        Console.WriteLine("\nAfter removing vertex 4:");
        graph.Print();
        Console.WriteLine("CountPieces(): " + graph.CountPieces());
    }

    public class Graph
    {
        private Dictionary<int, List<int>> adjacency;

        public Graph()
        {
            adjacency = new Dictionary<int, List<int>>();
        }

        // O(1) amortized
        public void AddVertex(int vertex)
        {
            if (!adjacency.ContainsKey(vertex))
            {
                adjacency.Add(vertex, new List<int>());
            }
        }

        // O(V)
        public void AddEdge(int vertex1, int vertex2)
        {
            AddVertex(vertex1);
            AddVertex(vertex2);

            if (vertex1 == vertex2 || HasEdge(vertex1, vertex2))
                return;

            adjacency[vertex1].Add(vertex2);
            adjacency[vertex2].Add(vertex1);
        }

        // O(V)
        public bool HasEdge(int vertex1, int vertex2)
        {
            if (!adjacency.ContainsKey(vertex1))
                return false;

            return adjacency[vertex1].Contains(vertex2);
        }

        // O(V)
        public void RemoveEdge(int vertex1, int vertex2)
        {
            if (!adjacency.ContainsKey(vertex1) || !adjacency.ContainsKey(vertex2))
                return;

            adjacency[vertex1].Remove(vertex2);
            adjacency[vertex2].Remove(vertex1);
        }

        // O(V+E)
        public void RemoveVertex(int vertex)
        {
            if (!adjacency.ContainsKey(vertex))
                return;

            foreach (int neighbor in adjacency[vertex])
            {
                adjacency[neighbor].Remove(vertex);
            }

            adjacency.Remove(vertex);
        }

        // O(VlogV + E)
        public void Print()
        {
            List<int> vertices = new List<int>(adjacency.Keys);
            vertices.Sort();

            foreach (int vertex in vertices)
            {
                Console.WriteLine(vertex + " -> [" +
                    string.Join(", ", adjacency[vertex]) + "]");
            }
        }

        // O(V+E)
        public List<int> BFS(int start)
        {
            List<int> result = new List<int>();

            if (!adjacency.ContainsKey(start))
                return result;

            Queue<int> queue = new Queue<int>();
            HashSet<int> visited = new HashSet<int>();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                result.Add(current);

                foreach (int neighbor in adjacency[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return result;
        }

        // O(V+E)
        public List<int> DFS(int startVertex)
        {
            List<int> result = new List<int>();
            HashSet<int> visited = new HashSet<int>();

            if (!adjacency.ContainsKey(startVertex))
                return result;

            void Visit(int currentVertex)
            {
                visited.Add(currentVertex);
                result.Add(currentVertex);

                foreach (int neighbor in adjacency[currentVertex])
                {
                    if (!visited.Contains(neighbor))
                    {
                        Visit(neighbor);
                    }
                }
            }

            Visit(startVertex);

            return result;
        }

        // O(V+E)
        public List<int> ShortestPath(int from, int to)
        {
            List<int> path = new List<int>();

            if (!adjacency.ContainsKey(from) || !adjacency.ContainsKey(to))
                return path;

            Queue<int> queue = new Queue<int>();
            HashSet<int> visited = new HashSet<int>();
            Dictionary<int, int> parent = new Dictionary<int, int>();

            queue.Enqueue(from);
            visited.Add(from);

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();

                if (current == to)
                {
                    while (current != from)
                    {
                        path.Add(current);
                        current = parent[current];
                    }

                    path.Add(from);
                    path.Reverse();

                    return path;
                }

                foreach (int neighbor in adjacency[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        parent[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return path;
        }

        // O(V+E)
        public int CountPieces()
        {
            HashSet<int> visited = new HashSet<int>();
            int count = 0;

            foreach (int vertex in adjacency.Keys)
            {
                if (!visited.Contains(vertex))
                {
                    List<int> piece = DFS(vertex);

                    foreach (int neighbor in piece)
                    {
                        visited.Add(neighbor);
                    }

                    count++;
                }
            }

            return count;
        }

        //O(V+E)
        public bool HasCycle()
        {
            HashSet<int> visited = new HashSet<int>();

            bool Visit(int currentVertex, int parent)
            {
                visited.Add(currentVertex);

                foreach (int neighbor in adjacency[currentVertex])
                {
                    if (!visited.Contains(neighbor))
                    {
                        if (Visit(neighbor, currentVertex))
                            return true;
                    }
                    else if (neighbor != parent)
                    {
                        return true;
                    }
                }

                return false;
            }

            foreach (int vertex in adjacency.Keys)
            {
                if (!visited.Contains(vertex))
                {
                    if (Visit(vertex, vertex))
                        return true;
                }
            }

            return false;
        }
    }
}