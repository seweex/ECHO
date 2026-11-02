using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1F;    
    
    private Keyboard _currentKeyboard = null;
    private Rigidbody _currentRigidbody = null;
    
    void Start()
    {
        _currentKeyboard = Keyboard.current;
        _currentRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_currentKeyboard == null)
            return;
        
        var direction = Vector3.zero;
        
        if (_currentKeyboard.wKey.isPressed) direction.z += 1f;
        if (_currentKeyboard.sKey.isPressed) direction.z -= 1f;
        
        if (_currentKeyboard.aKey.isPressed) direction.x -= 1f;
        if (_currentKeyboard.dKey.isPressed) direction.x += 1f;
        
        transform.Translate(Time.deltaTime * movementSpeed * direction, Space.Self);
    }
}
