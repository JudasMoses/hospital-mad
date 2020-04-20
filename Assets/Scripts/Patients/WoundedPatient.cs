using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoundedPatient : Patient
{
	[Header("Settings")]
	public int healTime;
	public int bleedoutTime;    //In seconds
	[Range(0, 5)]
	public float spawnBloodChance;

	[Header("References")]
	public Animator _animator;
	public Image healthBar;
	public GameObject UI;


	bool currentlyHealing = false;
	State healState = State.Waiting;
	float health = 100;
	bool hasBlood = false;


	enum State
	{
		Waiting, Bleeding
	}

	public override void PatientEventTrigger()
	{
		base.PatientEventTrigger();

		healState = State.Bleeding;
		bleedingCoroutine = Bleeding();
		StartCoroutine(bleedingCoroutine);

		// Graphics
		_animator.SetBool("wounded", true);
		UI.SetActive(true);
	}
	public override void Interact(bool interacting)
	{
		if (interacting && !currentlyHealing)
		{
			Debug.Log("Started Healing");
			healingCoroutine = Healing();
			StartCoroutine(healingCoroutine);
			StopCoroutine(bleedingCoroutine);
		}
		else if (!interacting)
		{
			Debug.Log("Stopped Healing");
			currentlyHealing = false;
			StopCoroutine(healingCoroutine);
			StartCoroutine(bleedingCoroutine);
		}
	}

	IEnumerator healingCoroutine;
	IEnumerator Healing()
	{
		currentlyHealing = true;
		yield return new WaitForSeconds(healTime);
		Debug.Log("Healed");
		PatientEventFinish();
	}
	protected override void PatientEventFinish()
	{
		base.PatientEventFinish();

		// Remove from interactables
		PlayerManager.instance.availableInteractable.Remove(this);
		// Stop Bleeding
		StopCoroutine(bleedingCoroutine);
		health = 100;

		currentlyHealing = false;
		healState = State.Waiting;
		_animator.SetBool("wounded", false);
		UI.SetActive(false);
	}

	IEnumerator bleedingCoroutine;
	IEnumerator Bleeding()
	{
		while (health > 0)
		{
			yield return new WaitForSecondsRealtime(1);
			health -= 100 / bleedoutTime;
			healthBar.fillAmount = health / 100;
		}
	}

	public override void CanInteract(bool interactable) {
		if (interactable)
		{
			if (healState != State.Waiting)
			{
				PlayerManager.instance.availableInteractable.Add(this);
			}
		}
		else
		{
			if (currentlyHealing && healState != State.Waiting)
			{
				Interact(false);
			}

			PlayerManager.instance.availableInteractable.Remove(this);
		}
	}
}
