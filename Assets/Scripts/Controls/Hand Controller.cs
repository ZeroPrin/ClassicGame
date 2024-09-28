using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class HandController : IInitializable, IDisposable
{
    [Header ("Current Item")]
    public GameObject _currentObject;
    public int _currentIndex;

    [Header ("Main Components")]
    Transform _handPoint;
    Transform _cameraTransform;

    [Header("Parameters")]
    float _dropForce = 5;

    private Rigidbody _currentRigidbody;
    private GameObject _lastPrefab;
    private int _lastIndex;

    [Header ("Actions")]
    public Action<int> OnItemDeleted;

    private PlayerController _playerController;

    [Inject] private DiContainer _container;

    [Inject]
    public void Construct(PlayerController playerController, IPlayerComponentsProvider playerComponentsProvider)
    {
        _playerController = playerController;
        _handPoint = playerComponentsProvider.HandTransform;
        _cameraTransform = playerComponentsProvider.CameraTransform;
    }

    void IInitializable.Initialize() 
    {
        _playerController.OnUse += Use; 
        _playerController.OnDrop += Drop;
    }

    void IDisposable.Dispose() 
    {
        _playerController.OnUse -= Use;
        _playerController.OnDrop -= Drop;
    }

    public void SetObject(GameObject prefab, int ind) 
    {
        _lastPrefab = prefab;
        _lastIndex = ind;

        if (_currentObject != null)
        {
            UnityEngine.Object.Destroy(_currentObject);
        }

        _currentIndex = ind;
        _currentObject = _container.InstantiatePrefab(prefab, _handPoint.position, Quaternion.identity, _handPoint);
        _currentRigidbody = _currentObject.GetComponent<Rigidbody>();
        _currentRigidbody.isKinematic = true;
    }

    public void ResetObject()
    {
        if (_currentObject != null)
        {
            UnityEngine.Object.Destroy(_currentObject);
        }

        _currentIndex = _lastIndex;
        _currentObject = _container.InstantiatePrefab(_lastPrefab, _handPoint.position, Quaternion.identity, _handPoint);
        _currentRigidbody = _currentObject.GetComponent<Rigidbody>();
        _currentRigidbody.isKinematic = true;
    }

    public void Use() 
    {
        if (_currentObject != null)
        {
            _currentObject.GetComponent<Item>().Use();

            ResetObject();

            OnItemDeleted?.Invoke(_currentIndex);
        }
    }

    public void Drop() 
    {
        if (_currentObject != null)
        {
            _currentObject.transform.parent = null;
            _currentRigidbody.isKinematic = false;
            _currentRigidbody.AddForce(_cameraTransform.forward * _dropForce, ForceMode.Impulse);
            _currentObject = null;

            ResetObject();

            OnItemDeleted?.Invoke(_currentIndex);
        }
    }

    public void Clear() 
    {
        if (_currentObject != null)
        {
            UnityEngine.Object.Destroy(_currentObject);
        }

        _currentObject = null;
        _currentRigidbody = null;
        _currentIndex = 0;
    }
}
