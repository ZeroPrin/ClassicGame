using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable 
{
    public Transform Transform { get; }
    public Transform CameraTransform { get;  }
    public Rigidbody RigidBody { get; }
    public Transform RayOriginTransform { get; }
}
