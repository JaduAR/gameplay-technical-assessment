using UnityEngine;
//using UnityEngine.EventSystems;

public class TapController : MonoBehaviour //, IPointerDownHandler, IPointerUpHandler
{
    public delegate void TapAction(string name, float diff);
    public static event TapAction OnTap;

/*  FOR 'HOLD AND RELEASE' FEATURE
 *  
 *  public delegate void ReleaseAction(string name, float time);
    public static event ReleaseAction OnRelease;

    [SerializeField]
    public bool isPressed;
    [SerializeField]
    private float _timePressed = 0f;
    private float _startHoldTime = 0f;

    [Space(10)]
*/
    
    private float _lastTapTime = 0f;

    public void Tap()
    {
        float diff = Time.time - _lastTapTime;
        Debug.Log(gameObject.name + " - Time passed since last tap : " + diff + " seconds .");

        if (OnTap != null) 
            OnTap(gameObject.name, diff);
        
        _lastTapTime = Time.time;
    }

    /* FOR HOLD AND RELEASE FEATURE
     * 
     * public void OnPointerDown(PointerEventData data)
    {
        _startHoldTime = Time.time;
       
        //Debug.Log(gameObject.name + " - Tap Time : " + _startHoldTime + " seconds .");
        
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        float currentTime = Time.time;
        //Debug.Log(gameObject.name + " - Tap Release Time : " + currentTime + " seconds .");
        
        _timePressed = currentTime - _startHoldTime;
        //Debug.Log(gameObject.name + " - Holding time : " + _timePressed + " seconds .");

        isPressed = false;

        if (OnRelease != null) 
            OnRelease(gameObject.name, _timePressed);

        _timePressed = 0f;
    }*/

}
