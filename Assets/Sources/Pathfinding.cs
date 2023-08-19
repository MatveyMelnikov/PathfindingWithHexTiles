using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding
{
    // Due to the positioning of the hex cells, 
    // the positions of the neighbors will differ depending on the column
    static Vector2Int[] offsetsForOrdinaryColumn = { 
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1)
    };
    static Vector2Int[] offsetsForOffsetColumn = { 
            new Vector2Int(1, -1),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1)
    };

    // Algorithm A* is used
    // The last argument is a function to estimate the cost of a tile. 
    // The name of the texture (tile image) is passed to the argument of this function
    static public List<Vector3Int> CalculatePath(
        in Tilemap tilemap, 
        Vector3Int start, 
        Vector3Int end,
        Func<string, int> GetCostOfTile
    ) {
        Utils.PriorityQueue<Vector3Int, int> frontier = new Utils.PriorityQueue<Vector3Int, int>();
        Dictionary<Vector3Int, Vector3Int> came_from = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, int> cost = new Dictionary<Vector3Int, int>();

        frontier.Enqueue(start, 0);
        came_from.Add(start, start);
        cost.Add(start, 0);

        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();

            if (current == end)
                break;

            foreach (Vector3Int next in GetNeighbors(tilemap, current))
            {
                int new_cost = cost[current] + GetCostOfTile(GetTileName(tilemap, next));

                if (!cost.ContainsKey(next) || new_cost < cost[next])
                {
                    cost[next] = new_cost;
                    frontier.Enqueue(next, new_cost + HeuristicFunction(end, next));
                    came_from[next] = current;
                }
            }
        }

        // Collect result path
        List<Vector3Int> path = new List<Vector3Int>() { end };
        do
        {
            Vector3Int last = path[path.Count - 1];
            path.Add(came_from[last]);
        } while (path[path.Count - 1] != start);
        path.Reverse();

        return path;
    }

    static int HeuristicFunction(Vector3Int a, Vector3Int b)
    {
        // Manhattan distance
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    static List<Vector3Int> GetNeighbors(in Tilemap tilemap, in Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        Vector2Int[] offsets = IsOffsetRow(position) ? 
            offsetsForOffsetColumn : offsetsForOrdinaryColumn;
        
        foreach (Vector3Int offset in offsets) {
            Vector3Int neighborPosition = position + offset;
            if (tilemap.HasTile(neighborPosition))
                neighbors.Add(neighborPosition);
        }

        return neighbors;
    }

    static bool IsOffsetRow(in Vector3Int position)
    {
        return position.y % 2 != 0;
    }

    static String GetTileName(in Tilemap tilemap, in Vector3Int position) {
        Sprite sprite = tilemap.GetSprite(position);
        return sprite.texture.name;
    }
}
