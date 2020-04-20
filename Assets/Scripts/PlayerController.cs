using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
	// PUBLIC VARIABLES
	[Header("Settings")]
	public int speed;
	[Header("References")]
	public Animator animator;

	// Cached Components
	InputMaster controls;
	Rigidbody2D _rigidBody;
	Collider2D _collider;

	//PRIVATE VARIABLES
	Vector2 moveDir;


	private void Awake()
	{
		// Instatiate input control
		controls = new InputMaster();
		controls.Player.Move.performed += context => moveDir = context.ReadValue<Vector2>();
		controls.Player.Interact.started += context => Interact(true);
		controls.Player.Interact.canceled += context => Interact(false);

		// Cache components
		_rigidBody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();
	}
	private void OnEnable()
	{
		controls.Enable();
	}
	private void OnDisable()
	{
		controls.Disable();
	}

	private void FixedUpdate()
	{
		_rigidBody.velocity = moveDir * speed;
	}
	private void Update()
	{
		animator.SetFloat("velX", moveDir.x);
		animator.SetFloat("velY", moveDir.y);
		animator.SetFloat("speed", moveDir.magnitude);
	}



	void Interact(bool interacting)
	{
		IInteractable[] interactables = PlayerManager.instance.availableInteractable.ToArray();
		// If there is nothing to interact with cancel interaction
		if (interactables.Length < 1) { return; }
		IInteractable interact = interactables[0];

		interact.Interact(interacting);
	}
}
