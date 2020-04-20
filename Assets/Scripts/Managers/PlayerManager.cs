using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = gameObject.GetComponent<PlayerManager>();
		}
	}



	public HashSet<IInteractable> availableInteractable = new HashSet<IInteractable>();
}
