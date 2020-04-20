using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableZone : MonoBehaviour
{
	[SerializeField, SerializeReference]
	public Patient interactable;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		interactable.CanInteract(true);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		interactable.CanInteract(false);
	}
}
