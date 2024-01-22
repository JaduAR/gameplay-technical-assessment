using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButtonBehavior : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button _attackButton;

    private bool _uiButtonPressed = false;
    private bool _buttonPressed = false;

    public bool ButtonPressed => _buttonPressed;

    private void Awake()
    {
        _attackButton.onClick.AddListener(OnAttackClicked);
    }

    private void Update()
    {
        if (GameLoopManager.Instance.GameIsEnded)
            return;
        
        _buttonPressed = Input.GetKey(KeyCode.Space) || _uiButtonPressed;
        
        if(Input.GetKeyDown(KeyCode.Space))
            OnAttackClicked();
    }

    private void OnAttackClicked()
    {
        AttackSequenceManager.Instance.ReceiveAttackInput();
    }
 
    public void OnPointerDown(PointerEventData eventData){
        _uiButtonPressed = true;
    }
 
    public void OnPointerUp(PointerEventData eventData){
        _uiButtonPressed = false;
    }
}
