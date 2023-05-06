using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveCreator
{
    public int objectID;
    public GameObject objectToSpawn;
    public SpawnTypes spawnType;
    public float delayAtBeginningOfWave;

    public float majorSpawnRate; //How quickly 
    public float minorSpawnRate; //How quickly enemy cycles through spawn locations


}
[System.Serializable]
public class CharacterDialogue
{
    public CharacterScriptableObject characterToTalk;
    public string dialogue;
    public float textTime;
    public bool complete;
    private void OnEnable()
    {
        complete = false;
    }
}

[CreateAssetMenu(fileName = "WaveScriptableObject", menuName = "ScriptableObjects/WaveScriptableObject")]
public class WaveScriptableObject : ScriptableObject
{
    public float waveLength; //Seconds that the wave is active
    public float endingTimeBuffer;
    public float waveElapsed = 0;

    public bool waveStarted;

    public WaveCreator[] objectsToSpawn;
    public CharacterDialogue[] characterDialogue;

    private void OnEnable()
    {
        waveElapsed = 0;
        waveStarted = false;
        if (characterDialogue != null)
        {
            foreach (CharacterDialogue dialogue in characterDialogue)
            {
                dialogue.complete = false;
            }
        }
    }

}