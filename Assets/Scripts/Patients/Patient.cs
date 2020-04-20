using UnityEngine;

public abstract class Patient : MonoBehaviour, IInteractable
{
	[HideInInspector]
	public bool active = false;

	public abstract void Interact(bool interacting);
	public virtual void PatientEventTrigger()
	{
		active = true;
	}
	protected virtual void PatientEventFinish()
	{
		active = false;
	}

	public virtual void CanInteract(bool interactable)
	{
		if (interactable)
		{
			PlayerManager.instance.availableInteractable.Add(this);
		}
		else
		{
			PlayerManager.instance.availableInteractable.Remove(this);
		}
	}
}