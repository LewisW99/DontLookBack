using System;
using UnityEngine;

public static class SoundManager
{
    public static event Action<Vector3> OnSoundMade;

    public static void ReportSound(Vector3 position)
    {
        OnSoundMade?.Invoke(position);
    }
}
