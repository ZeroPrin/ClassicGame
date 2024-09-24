using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _rayOriginPoint;
    [SerializeField] private float _checkDistance = 5f;
    [SerializeField] private float _spawnDelay = 1f;

    private float _timeSinceLastCheck = 0f;

    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(_rayOriginPoint.position, Vector3.up, out hit, _checkDistance))
        {
            _timeSinceLastCheck = 0f;
        }
        else
        {
            _timeSinceLastCheck += Time.fixedDeltaTime;

            if (_timeSinceLastCheck >= _spawnDelay)
            {
                SpawnObject();
                _timeSinceLastCheck = 0f;
            }
        }
    }

    void SpawnObject()
    {
        GameObject newObj = Instantiate(_objectToSpawn, _spawnPoint.position, Quaternion.identity);

        if (newObj.GetComponent<Item>())
            newObj.GetComponent<Item>().PermanentRotation.rotationActive = true;
    }
}
