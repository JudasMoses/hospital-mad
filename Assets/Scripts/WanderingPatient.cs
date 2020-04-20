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
	Vector2? destination = null;

	private void FixedUpdate()
	{
		if (destination != null)
		{
			Vector2 dir = (Vector2) position.position - (Vector2) destination;
			dir.Normalize();
			dir *= speed;

			_rigidbody.velocity = dir;
		}
	}

	// TEST - REMOVE FROM BUILD
	private void Start()
	{
		PatientEvent();
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
				destination = new Vector2(x, y);
				path.RemoveAt(0);

				// Wait until at point
				while(position.position != new Vector3(x, y)) { }
			}
			yield return new WaitForSeconds(decisionTime);
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
}
