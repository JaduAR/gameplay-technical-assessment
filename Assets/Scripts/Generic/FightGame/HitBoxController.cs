using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    [SerializeField]
    GameObject PunchHitBoxLeft;
    [SerializeField]
    GameObject PunchHitBoxRight;

    public void ActiveHitBoxLeft()
    {
        if (PunchHitBoxLeft) PunchHitBoxLeft.SetActive(true);
    }

    public void DisableHitBoxLeft()
    {
        if (PunchHitBoxLeft) PunchHitBoxLeft.SetActive(false);
    }

    public void ActiveHitBoxRight()
    {
        if (PunchHitBoxRight) PunchHitBoxRight.SetActive(true);
    }

    public void DisableHitBoxRight()
    {
        if (PunchHitBoxRight) PunchHitBoxRight.SetActive(false);
    }
}
