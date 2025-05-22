using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string sceneName;
    public int sceneBuildIndex;
    public Vector3 playerPosition;
    public float playerHealth;
    public string timestamp;
    public float totalPlayTimeSeconds;
}
