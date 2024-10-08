using Data.Runtime;
using UnityEngine;
using Sirenix.OdinInspector;

public class TongueManager : MonoBehaviour
{
    #region Unity

    private void Awake()
    {
        _speed = _tongueBlackboard.GetValue<float>("Speed");
        _cooldown = _tongueBlackboard.GetValue<float>("Cooldown");
        _grabDelay = _tongueBlackboard.GetValue<float>("GrabDelay");
        _maxDistance = _tongueBlackboard.GetValue<float>("MaxDistance");
        _cooldownTimer = _tongueBlackboard.GetValue<float>("CooldownTimer");
    }

    private void Start()
    {
        _initialTonguePosition = _tongueTip.localPosition; // Save initial position
    }

    private void Update()
    {
        if (_cooldownTimer > 0) 
            _cooldownTimer -= Time.deltaTime;

        if (_isGrabbling)
            StartGrabble();
    }

    #endregion

    #region Methods

    public void OnGrabEvent()
    {
        if (!_isGrabbling)
            _isGrabbling = true; // Trigger the Tongue grabbler
    }

    #endregion

    #region Utils

    private void StartGrabble()
    {
        if (_cooldownTimer > 0) return;
        
        _tongueTip.Translate(Vector3.forward * _speed * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(_player.position, _player.forward, out hit, _maxDistance, _grabLayer) && Vector3.Distance(_tongueTip.position, _player.position) >= _maxDistance)
        {
            _grabblePoint = hit.point;
            _grabbedObject = hit.transform;

            Invoke(nameof(ExtendTongue), _grabDelay);
        }
        else
        {
            _grabblePoint = _player.position + _player.forward * _maxDistance;
            Invoke(nameof(ReturnTongue), _grabDelay);
        }
    }

    private void ExtendTongue()
    {
        EnemyManager grabManager = _grabbedObject.GetComponent<EnemyManager>();

        if (grabManager.m_grapSize == IGrappable.Size.Small)
        {
            _grabbedObject.position = _player.position;
        }
        else if (grabManager.m_grapSize == IGrappable.Size.Large)
        {
            _player.position = _grabblePoint;
        }

        _tongueTip.localPosition = Vector3.MoveTowards(_tongueTip.localPosition, _initialTonguePosition, _speed * Time.deltaTime);

        _isGrabbling = false;
    }

    private void ReturnTongue()
    {
        _isGrabbling = false;
        _cooldownTimer = _cooldown;
    }

    #endregion

    #region Privates

    [Title("Blackboard")]
    [SerializeField]
    private Blackboard _tongueBlackboard;

    [Title("Blackboard variables")]
    private float _speed;
    private float _cooldown;
    private float _grabDelay;
    private float _maxDistance;
    private float _cooldownTimer;

    [Title("Layers")]
    [SerializeField]
    private LayerMask _grabLayer; // Layer of grabbing objects

    [Title("Transforms")]
    [SerializeField]
    private Transform _player; // Player reference
    [SerializeField]
    private Transform _tongueTip; // Tip of Tongue collider

    [Title("Privates")]
    private Vector3 _grabblePoint; // Target point on hit
    private Vector3 _initialTonguePosition; // Initial Tongue position

    private Transform _grabbedObject = null; // Grabbed object
    
    private bool _isGrabbling = false; // The Tongue is currently grappling

    #endregion
}
