using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject player;
    public float force;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Grappleable"))
        {
            player.GetComponent<Rigidbody>().AddForce((transform.position - player.transform.position).normalized * force, ForceMode.Impulse);
        }
    }
}