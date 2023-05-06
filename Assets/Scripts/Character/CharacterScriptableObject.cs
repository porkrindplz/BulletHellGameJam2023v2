using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Character
{
    public string name;
    public Sprite portrait;
}
[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/CharacterScriptableObject")]
public class CharacterScriptableObject : ScriptableObject
{
    public Character character;
}
