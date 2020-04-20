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


		// Setup
		GetGrid();
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


	public void GetGrid()
	{
		// Pathfinding
		floor.CompressBounds();
		BoundsInt tilemapBounds = floor.cellBounds;
		bool[,] tilesmap = new bool[tilemapBounds.xMax, tilemapBounds.yMax];

		for (int x = tilemapBounds.xMin; x < tilemapBounds.xMax; x++)
		{
			for (int y = tilemapBounds.yMin; y < tilemapBounds.yMax; y++)
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
