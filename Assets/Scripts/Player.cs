using UnityEngine;

public class Player : MonoBehaviour
{
    private Avatar _avatar;

    // Start is called before the first frame update
    void Start()
    {
        _avatar = GetComponent<Avatar>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (_avatar == null) return;

        if (_avatar.AreActionsDisabled) 
        {
            if (_avatar.IsMoving)
            {
                _avatar.Stop();
            }
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            _avatar.Move(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _avatar.Move(Vector2.down);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _avatar.Move(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _avatar.Move(Vector2.right);
        }
        else
        {
            _avatar.Move(Vector2.zero);
        }
    }
}
