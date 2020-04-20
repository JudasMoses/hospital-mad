public interface IInteractable
{
	void Interact(bool interacting);
}

public interface IPatient: IInteractable
{
	void PatientEvent();
}