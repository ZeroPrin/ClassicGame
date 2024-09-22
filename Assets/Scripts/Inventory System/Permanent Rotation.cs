using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PermanentRotation : MonoBehaviour
{
    public bool rotationActive;

    [SerializeField]
    [Range(0.1f, 10)]
    float rotationSpeed;

    public void FixedUpdate()
    {
        if (rotationActive)
            transform.Rotate(0, rotationSpeed, 0);
    }
}
