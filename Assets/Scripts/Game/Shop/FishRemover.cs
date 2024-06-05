using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishRemover : MonoBehaviour
{
    public event Action<GameObject> Removed;

    private void OnDisable()
    {
        Removed?.Invoke(gameObject);
    }
}
