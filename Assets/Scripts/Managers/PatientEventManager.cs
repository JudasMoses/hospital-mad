using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PatientEventManager : MonoBehaviour
{   
    public static PatientEventManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<PatientEventManager>();
        }
    }


    public Level[] difficultyLevels;
    int currentLevel = 0;
    public Patient[] patients;

    Timer eventTimer;



    void Start()
    {
        MakeTimer(difficultyLevels[currentLevel].timeBetweenPatientEvents);
        PatientEvent();
    }

    void MakeTimer(int seconds)
    {
        eventTimer = new Timer(seconds);
        eventTimer.AutoReset = true;

        eventTimer.Elapsed += EventTimer_Elapsed;
    }

    private void EventTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        PatientEvent();

        //Check if we need to progress onto next difficulty
        if (Time.timeSinceLevelLoad > difficultyLevels[currentLevel].timeIntoLevel)
        {
            //There are more level to use
            if (currentLevel < difficultyLevels.Length - 1)
            {
                currentLevel++;
                MakeTimer(difficultyLevels[currentLevel].timeBetweenPatientEvents);
            }
        }
    }

    void PatientEvent()
    {
        Debug.Log($"Triggering patient event for {difficultyLevels[currentLevel].numberOfEventsToTrigger} patients");

        List<Patient> patientsList = patients.ToList();
        patientsList.Shuffle();

        // Trigger set amount of events
        for (int i = 0; i < difficultyLevels[currentLevel].numberOfEventsToTrigger - 1; i++)
        {
            // Loop through each patient to find available one
            Debug.Log($"Triggering patient {i}");
            for (int patient = 0; patient < patientsList.Count; patient++)
            {
                // If event already triggered
                if (patientsList[patient].active)
                {
                    patientsList.RemoveAt(patient);
                    continue;
                }
                // If not triggered
                else
                {
                    Debug.Log("Triggered patient event action");
                    patientsList[patient].PatientEventTrigger();
                    patientsList.RemoveAt(patient);
                    break;
                }
            }
            // If there are no available patients left return
            if (patientsList.Count < 1) { break; }
        }
    }
}
