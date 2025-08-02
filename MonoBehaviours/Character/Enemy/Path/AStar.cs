using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private HashSet<Vector2Int> blockedTiles;

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        Dictionary<Vector2Int, PathNode> openSet = new(); // 다음에 탐색할 후보 노드들
        HashSet<Vector2Int> closedSet = new(); // closedSet: 이미 탐색한 후보들

        // PriorityQueue(node, ?)
        PriorityQueue<PathNode> pq = new();

        PathNode startNode = new PathNode()
        {
            position = start,
            g = 0,
            h = Heuristic(start, goal)
        };
        openSet[start] = startNode;
        pq.Enqueue(startNode, startNode.f);

        while (pq.Count > 0)
        {
            // 1. pq에서 가장 우선순위인 걸 하나 꺼냄
            PathNode current = pq.Dequeue();
            if (current.position == goal)
            {
                // ReconstructPath(node)
                return ReconstructPath(current);
            }

            // 2. 자 이제 시작
            closedSet.Add(current.position);

            // 3. 4방향 이웃 노드들을 확인한다
            foreach (Vector2Int dir in Directions)
            {
                Vector2Int neighborPos = current.position + dir;
                if (blockedTiles.Contains(neighborPos) || closedSet.Contains(neighborPos))
                    continue ;
                int tentativeG = current.g + 1;
                if (openSet.TryGetValue(neighborPos, out var neighbor))
                {
                    if (tentativeG < neighbor.g)
                    {
                        neighbor.g = tentativeG;
                        neighbor.parent = current;
                    }
                }
                else
                {
                    neighbor = new PathNode()
                    {
                        position = neighborPos,
                        g = tentativeG,
                        h = Heuristic(neighborPos, goal),
                        parent = current
                    };
                    openSet[neighborPos] = neighbor;
                    pq.Enqueue(neighbor, neighbor.f);
                }
            }

        }

        return null;
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector2Int> ReconstructPath(PathNode node)
    {
        List<Vector2Int> path = new();
        while (node != null)
        {
            path.Add(node.position);
            node = node.parent;
        }
        path.Reverse();
        return path;
    }

    private static readonly Vector2Int[] Directions = {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    public void SetBlockedTiles(HashSet<Vector2Int> blocked)
    {
        blockedTiles = blocked;
    }
}