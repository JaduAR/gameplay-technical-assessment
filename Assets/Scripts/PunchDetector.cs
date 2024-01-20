using UnityEngine;

public class PunchDetector : MonoBehaviour
{

    public delegate void PunchLanded();
    public PunchLanded punchLanded;

    [SerializeField]
    GameObject _hitVFXPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            punchLanded();

            // better to have a pooling system for better performance
             Instantiate(_hitVFXPrefab,transform);
        }
    }
}
