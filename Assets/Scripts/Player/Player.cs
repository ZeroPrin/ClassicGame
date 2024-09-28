using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IMovable
{
    public Transform Transform => transform;
    public Transform CameraTransform => _cameraTransform;
    public Rigidbody RigidBody => _rb;
    public Transform RayOriginTransform => _rayOriginTransform;

    [Header("Main Components")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _rayOriginTransform;
}
