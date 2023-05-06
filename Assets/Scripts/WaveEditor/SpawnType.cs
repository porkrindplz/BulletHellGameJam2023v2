using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnTypes
{
    Missile,
    Enemy
}

public static class SpawnType
{
    public static SpawnTypes Spawn = new SpawnTypes();
}
