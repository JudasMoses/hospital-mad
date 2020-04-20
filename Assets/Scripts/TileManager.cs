using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PathFind = NesScripts.Controls.PathFind;

public class TileManager : MonoBehaviour
{
	public static TileManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = gameObject.GetComponent<TileManager>();
		}
	}


	[Header("Settings")]
	public int difficulty;

	[Header("References")]
	// Tilemaps
	public Tilemap floor;
	public Tilemap floorProps;
	// Tiles
	public Tile floorTile;
	// Pathfinding
	public PathFind.Grid grid;



	private void Start()
	{
		GetGrid();

		PathFind.Point _from = new PathFind.Point(10, 1);
		PathFind.Point _to = new PathFind.Point(10, 10);
		List<PathFind.Point> path = PathFind.Pathfinding.FindPath(grid, _from, _to);

		Debug.Log(path);
	}


	public void GetGrid()
	{
		// Pathfinding
		BoundsInt tilemapBounds = floor.cellBounds;
		bool[,] tilesmap = new bool[tilemapBounds.xMax, tilemapBounds.yMax];

		for (int x = 0; x < tilemapBounds.xMax - 1; x++)
		{
			for (int y = 0; y < tilemapBounds.yMax - 1; y++)
			{
				Vector3Int pos = new Vector3Int(x, y, 0);

				// If the cell isnt a floor tile remove
				if (floor.GetTile(pos) == null) { continue; }

				// If the cell is occupied by a floor prop remove
				if (floorProps.GetTile(pos) != null) { continue; }

				// Otherwise set tile to true
				tilesmap[x, y] = true;
			}
		}

		grid = new PathFind.Grid(tilesmap);
	}
}
