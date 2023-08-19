# PathfindingWithHexTiles
Implementation of the A* pathfinding algorithm using a tilemap as a graph.

## Advantages
To work, there is no need to create a cost matrix for each tile. The cost depends on the name of the tile (its image) and is set in the external function passed to the method with the last argument.

## How to use
You need to transfer the [file with the pathfinding](https://github.com/MatveyMelnikov/PathfindingWithHexTiles/blob/master/Assets/Sources/Pathfinding.cs) code and the [external implementation](https://github.com/MatveyMelnikov/PathfindingWithHexTiles/blob/master/Assets/Sources/External/PriorityQueue.cs) of the priority queue to your project.

Now prepare your tilemap. Remember that the names of their images are used to identify each tile in the script. (The example uses tiles 'grass_1', 'grass_2' and 'grass_3').

The code that calls the path lookup will look something like this:
```
void Update()
{
  if (Input.GetMouseButtonDown(0))
  {
    // Get the global position of the mouse and the position of the selected cell
    // Example:
    // Vector2 mousePos = GetMouseWorldPosition();
    // start = tilemap.WorldToCell(mousePos);

    positions = Pathfinding.CalculatePath(tilemap, start, end, GetTileCost);
  }
}

// Function to determine the cost of each tile
// The name of the texture (tile image) is passed to the argument
int GetTileCost(string name)
{
  switch(name[name.Length - 1])
  {
    case '1':
      return 2;
    case '2':
      return 4;
    case '3':
      return 0;
    default:
      return 10;
  }
}
```
