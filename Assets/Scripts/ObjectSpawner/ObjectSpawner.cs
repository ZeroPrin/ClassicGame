using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Transform rayOriginPoint;
    [SerializeField]
    private float checkDistance = 5f;
    [SerializeField]
    private float spawnDelay = 1f;

    private float timeSinceLastCheck = 0f;

    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(rayOriginPoint.position, Vector3.up, out hit, checkDistance))
        {
            timeSinceLastCheck = 0f;
        }
        else
        {
            timeSinceLastCheck += Time.fixedDeltaTime;

            if (timeSinceLastCheck >= spawnDelay)
            {
                SpawnObject();
                timeSinceLastCheck = 0f;
            }
        }
    }

    void SpawnObject()
    {
        GameObject newObj = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);

        if (newObj.GetComponent<Item>())
            newObj.GetComponent<Item>().PermanentRotation.rotationActive = true;
    }
}
