using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public float hookSpeed;
    public GameObject GrapplingHookPrefab;
    public Transform GrapplingHookSpawnPoint;

    private GameObject currentHook;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootGrapplingHook();
        }

        if (Input.GetMouseButtonUp(0))
        {
            RemoveGrapplingHook();
        }
    }

    private void ShootGrapplingHook()
    {
        currentHook = Instantiate(GrapplingHookPrefab, GrapplingHookSpawnPoint.position, GrapplingHookSpawnPoint.rotation);
        currentHook.GetComponent<Rigidbody>().velocity = GrapplingHookSpawnPoint.forward * hookSpeed;
    }

    private void RemoveGrapplingHook()
    {
        Destroy(currentHook);
    }
}