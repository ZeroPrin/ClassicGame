using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerComponentsProvider
{
    public Transform Transform { get; }
    public Transform HandTransform { get; }
    public Transform CameraTransform { get;  }
    public Rigidbody RigidBody { get; }
    public Transform RayOriginTransform { get; }
}
