using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class HandController : MonoBehaviour
{
    [Header ("Current Item")]
    public GameObject currentObject;
    public int currentIndex;

    [Header ("Main Components")]
    [SerializeField] 
    Transform HandPoint;
    [SerializeField]
    Transform cameraTransform;

    [Header("Parameters")]
    [SerializeField]
    float dropForce = 5;

    private Rigidbody currentRigidbody;
    private GameObject lastPrefab;
    private int lastIndex;

    [Header ("Actions")]
    public Action<int> onItemDeleted;

    public void SetObject(GameObject prefab, int ind) 
    {
        lastPrefab = prefab;
        lastIndex = ind;

        if (currentObject != null)
        {
            Destroy(currentObject);
        }
        currentIndex = ind;
        currentObject = Instantiate(prefab, HandPoint.position, Quaternion.identity, HandPoint);
        currentRigidbody = currentObject.GetComponent<Rigidbody>();
        currentRigidbody.isKinematic = true;
    }

    public void ResetObject()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        currentIndex = lastIndex;
        currentObject = Instantiate(lastPrefab, HandPoint.position, Quaternion.identity, HandPoint);
        currentRigidbody = currentObject.GetComponent<Rigidbody>();
        currentRigidbody.isKinematic = true;
    }

    public void Use() 
    {
        if (currentObject != null)
        {
            currentObject.GetComponent<Item>().Use();

            ResetObject();

            onItemDeleted?.Invoke(currentIndex);
        }
    }

    public void Drop() 
    {
        if (currentObject != null)
        {
            currentObject.transform.parent = null;
            currentRigidbody.isKinematic = false;
            currentRigidbody.AddForce(cameraTransform.forward * dropForce, ForceMode.Impulse);
            currentObject = null;

            ResetObject();

            onItemDeleted?.Invoke(currentIndex);
        }
    }

    public void Clear() 
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        currentObject = null;
        currentRigidbody = null;
        currentIndex = 0;
    }
}
