using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "LevelDifficulty", menuName = "Level ")]
public class Level : ScriptableObject
{
    // The amount of seconds that the player needs to play (in total) before it starts this level
    public int timeIntoLevel;
    public int timeBetweenPatientEvents;
    public int numberOfEventsToTrigger;
}