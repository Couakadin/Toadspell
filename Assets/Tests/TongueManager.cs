using Data.Runtime;
using UnityEngine;
using Sirenix.OdinInspector;

public class TongueManager : MonoBehaviour
{
    #region Unity

    private void Awake()
    {
        _speed = _tongueBlackboard.GetValue<float>("Speed");
        _maxDistance = _tongueBlackboard.GetValue<float>("MaxDistance");
    }

    private void Start()
    {
        _initialTonguePosition = _tongueTip.localPosition;  // Safe initial position
    }

    private void Update()
    {
        if (_isExtending)
            ExtendTongue();
        else if (_isReturning)
            ReturnTongue();
    }

    #endregion

    #region Methods

    public void OnGrabEvent()
    {
        if (!_isExtending && !_isReturning)
            _isExtending = true;  // Trigger the Tongue extension
    }

    private void ExtendTongue()
    {
        // Move Tongue forward
        _tongueTip.Translate(Vector3.forward * _speed * Time.deltaTime);

        // Check max distance
        if (Vector3.Distance(_tongueTip.position, _player.position) >= _maxDistance)
        {
            _isExtending = false;
            _isReturning = true;  // Start returning if Tongue reached its max distance
        }

        // Raycast to detect objets on the path
        RaycastHit hit;
        if (Physics.Raycast(_player.position, _tongueTip.position - _player.position, out hit, _maxDistance, _grabLayer))
        {
            _isExtending = false;
            _isReturning = true;
            _grabbedObject = hit.transform;  // Save grabed object
            _grabbedObject.position = _tongueTip.position;  // Attach the object to Tongue
        }
    }

    private void ReturnTongue()
    {
        // Go back to initial position
        _tongueTip.localPosition = Vector3.MoveTowards(_tongueTip.localPosition, _initialTonguePosition, _speed * Time.deltaTime);

        // If an object is catched, bring it back to the player
        if (_grabbedObject != null)
            _grabbedObject.position = _tongueTip.position;  // The object follows Tongue

        // Check if Tongue is at initial position
        if (_tongueTip.localPosition == _initialTonguePosition)
        {
            _isReturning = false;
            
            if (_grabbedObject != null)
            {
                // Set the object position in front of the player
                _grabbedObject.position = _player.position + _player.forward * 1f;
                _grabbedObject = null;  // Reset catched object
            }
        }
    }

    #endregion

    #region Privates

    [Title("Blackboard")]
    [SerializeField]
    private Blackboard _tongueBlackboard;

    [Title("Blackboard variables")]
    private float _speed;
    private float _maxDistance;

    [Title("Layers")]
    [SerializeField]
    private LayerMask _grabLayer;  // Layer of grabbing objects

    [Title("Transforms")]
    [SerializeField]
    private Transform _player;  // Player reference
    [SerializeField]
    private Transform _tongueTip;  // Tip of Tongue collider

    [Title("Privates")]
    private Vector3 _initialTonguePosition;  // Initial Tongue position
    
    private Transform _grabbedObject = null;  // Grabbed object
    
    private bool _isExtending = false;  // The tongue is currently extending
    private bool _isReturning = false;  // The tongue is currently returning

    #endregion
}
