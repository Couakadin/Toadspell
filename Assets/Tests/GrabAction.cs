using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GrabAction : MonoBehaviour
{
    #region Unity

    private void OnEnable() =>
        _grabAction.Enable();

    private void OnDisable() =>
        _grabAction.Disable();

    private void Update()
    {
        if (IsGrabTrigger()) _grabEvent.Invoke();
    }

    #endregion

    #region Utils

    /// <summary>
    /// Bolean method detecting if Grab key input is triggered.
    /// </summary>
    /// <returns>True or false grabbing</returns>
    private bool IsGrabTrigger() => _grabAction.triggered;

    #endregion

    #region Privates

    [Title("Inputs")]
    [SerializeField]
    private InputAction _grabAction;

    [Title("Events")]
    [SerializeField]
    private UnityEvent _grabEvent;

    #endregion
}
