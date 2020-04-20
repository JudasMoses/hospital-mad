using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using PathFind = NesScripts.Controls.PathFind;

public class WanderingPatient : MonoBehaviour, IPatient
{
	[Header("Settings")]
	public int minWanderRadius;
	public int maxWanderRadius;
	public int speed;
	public int decisionTime;
	public int pointsToTravelBeforeDecision;

	[Header("References")]
	public Transform position;
	GameObject bed;
	public Rigidbody2D _rigidbody;

	bool inBed = true;
	bool following = false;
	Vector2Int destinationPoint;
	Vector2Int currentCellPos;

	// TEST - REMOVE FROM BUILD
	public Transform destinationTest;
	private void Start()
	{
		//PatientEvent();

		Vector2Int start = PositionToCellPosition(position.position);
		PathFind.Point _start = new PathFind.Point(start.x, start.y);
		Vector2Int finalDestination = PositionToCellPosition(destinationTest.position);
		Debug.Log(finalDestination);
		PathFind.Point _dest = new PathFind.Point(finalDestination.x + 1, finalDestination.y + 1);
		Debug.Log(finalDestination.x);


		List<PathFind.Point> path = PathFind.Pathfinding.FindPath(TileManager.instance.grid, _start, _dest);
		Debug.Log(_dest);

		Debug.Log(path.Count);

		StartCoroutine(Travelling(path));
	}


	private void FixedUpdate()
	{
		if (destinationPoint != Vector2.zero)
		{
			Vector3 destinationPointWorldPos = TileManager.instance.floor.CellToWorld((Vector3Int) destinationPoint);

			Vector2 dir = new Vector2(destinationPointWorldPos.x, destinationPointWorldPos.y) - (Vector2)position.position;
			dir.Normalize();
			dir *= speed;

			_rigidbody.velocity = dir;
			currentCellPos = PositionToCellPosition(position.position);
		}
		else
		{
			_rigidbody.velocity = Vector2.zero;
		}
	}

	public void PatientEvent()
	{
		StartCoroutine(Wandering());
	}
	public void Interact(bool interacting)
	{
		throw new System.NotImplementedException();
	}

	IEnumerator wanderingCoroutine;
	IEnumerator Wandering()
	{
		List<PathFind.Point> path = new List<PathFind.Point>();

		Vector3Int pos = TileManager.instance.floor.WorldToCell(transform.position);
		PathFind.Point _start = new PathFind.Point(pos.x, pos.y);
		
		// Get a valid destination with path
		while (path.Count < 1)
		{
			Vector2 destination = RandomDestination();
			PathFind.Point _dest = new PathFind.Point((int) destination.x, (int) destination.y);

			path = PathFind.Pathfinding.FindPath(TileManager.instance.grid, _start, _dest);
		}

		// Travel along path
		while (path.Count > 1)
		{
			for (int i = 0; i < pointsToTravelBeforeDecision || i < path.Count - 1; i++)
			{
				int x = path[0].x;
				int y = path[0].y;
				destinationPoint = new Vector2Int(x, y);
				path.RemoveAt(0);

				// Wait until at point
				while(position.position != new Vector3(x, y)) { }
			}
			yield return new WaitForSeconds(decisionTime);
		}
	}
	
	IEnumerator Travelling(List<PathFind.Point> path)
	{
		destinationPoint = new Vector2Int(path[0].x, path[0].y);

		while (path.Count > 1)
		{
			if (currentCellPos != destinationPoint)
			{
				yield return null; 
			}
			else
			{
				path.RemoveAt(0);
				destinationPoint = new Vector2Int(path[0].x, path[0].y);
			}
		}
	}

	Vector2 RandomDestination()
	{
		int x = Random.Range(minWanderRadius, maxWanderRadius);
		int y = Random.Range(minWanderRadius, maxWanderRadius);
		Vector2Int wanderDir = new Vector2Int(x, y);

		Vector2 currentPos = (Vector2)position.position;
		currentPos.x = Mathf.Floor(currentPos.x);
		currentPos.y = Mathf.Floor(currentPos.y);

		return currentPos + wanderDir;
	}
	Vector2Int PositionToCellPosition(Vector2 pos)
	{
		Tilemap floor = TileManager.instance.floor;
		Vector3Int cellPos3 = floor.WorldToCell(pos);
		Vector2Int cellPos = new Vector2Int(cellPos3.x, cellPos3.y);
		return cellPos;
	}
}
