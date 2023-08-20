using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestControl : MonoBehaviour
{
    public Tilemap tilemap;
    
    Vector3Int start = new Vector3Int();
    Vector3Int end = new Vector3Int();
    bool isStartSelected = false;
    List<Vector3Int> positions = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = GetMouseWorldPosition();

            if (!isStartSelected)
            {
                start = tilemap.WorldToCell(mousePos);
                isStartSelected = true;
                Debug.Log("Start selected");
                return;
            }
            end = tilemap.WorldToCell(mousePos);
            Debug.Log("End selected");

            isStartSelected = false;
            if (!tilemap.HasTile(start) || !tilemap.HasTile(end))
            {
                Debug.Log("Selected non-existent tile");
                return;
            }

            // Test run
            positions = Pathfinding.CalculatePath(tilemap, start, end, GetTileCost);
        }
    }

    // Function to determine the cost of each tile.
    // If the price is negative, then the tile is considered unattainable.
    // The name of the texture (tile image) is passed to the argument.
    int GetTileCost(string name)
    {
        switch(name[name.Length - 1])
        {
            case '1':
                return 2;
            case '2':
                return -1;
            case '3':
                return 0;
            default:
                return 10;
        }
    }

    void OnDrawGizmos()
    {
        if (positions == null)
            return;

        foreach (Vector3Int pos in positions)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(tilemap.CellToWorld(pos), 0.2f);
        }
    }

    Vector2 GetMouseWorldPosition() {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
