using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private HashSet<KeyData> keys = new HashSet<KeyData>();

    public bool HasKey(KeyData key)
    {
        return keys.Contains(key);
    }

    public void AddKey(KeyData key)
    {
        keys.Add(key);
        Debug.Log($"Added key: {key.displayName}");
    }
}