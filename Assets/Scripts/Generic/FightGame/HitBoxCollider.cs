using UnityEngine;

public class HitBoxCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            Debug.Log("HIT" + other.gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
