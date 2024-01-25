using UnityEngine;

public class TapController : MonoBehaviour
{
    public delegate void TapAction(string name, float diff);
    public static event TapAction OnTap;
   
    private float _lastTapTime = 0f;

    public void Tap()
    {
        float diff = Time.time - _lastTapTime;
        // Debug.Log(gameObject.name + " - Time passed since last tap : " + diff + " seconds .");

        if (OnTap != null) 
            OnTap(gameObject.name, diff);
        
        _lastTapTime = Time.time;
    }

}
