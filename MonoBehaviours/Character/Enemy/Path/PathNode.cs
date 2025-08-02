using UnityEngine;

// PathNode.cs
public class PathNode {
    public Vector2Int position;
    public int g;
    public int h;
    public int f => g + h;

    public PathNode parent;
}