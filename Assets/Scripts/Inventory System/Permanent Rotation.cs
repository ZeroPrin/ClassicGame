using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PermanentRotation : MonoBehaviour
{
    public bool _rotationActive;

    [SerializeField, Range(0.1f, 10)]
    float rotationSpeed;

    public void FixedUpdate()
    {
        if (_rotationActive)
        {
            transform.Rotate(0, rotationSpeed, 0);
        }
    }
}
