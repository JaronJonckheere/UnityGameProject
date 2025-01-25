using System;
using UnityEngine;

[Serializable]
public class MonsterData
{
    public int monsterID;
    public int health;
    public float[] position;  // Array to hold position (x, y, z)

    public MonsterData(Monster monster)
    {
        monsterID = monster.monsterID;
        health = monster.health;

        // Store position as an array of floats
        position = new float[3];
        position[0] = monster.transform.position.x;
        position[1] = monster.transform.position.y;
        position[2] = monster.transform.position.z;
    }
}
