using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private Joystick _joystick;

    public bool LastFrameMoving { get; private set; }

    public bool Moving => _joystick.Direction != Vector2.zero;
    public bool StoppedMoving => !Moving && LastFrameMoving;
    
    public event Action Moved;

    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }

    private void OnDisable()
    {
        _movement?.Stop();
    }

    public void Update()
    {
        Vector3 direction = Vector3.zero;

        if (_joystick.Direction != Vector2.zero)
        {
            direction = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
        }

        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                direction += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction += Vector3.right;
            }
        }

        if (direction == Vector3.zero)
        {
            _movement.Stop();
            return;
        }

        _movement.Move(direction.normalized);
        Moved?.Invoke();
    }

    private void LateUpdate()
    {
        LastFrameMoving = Moving;
    }
}
