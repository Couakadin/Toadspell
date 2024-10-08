using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
	#region Publics

	[Header("Ref")]
	public Transform m_playerTransform;
	public Transform m_tongueTip;
	public LayerMask m_whatIsGrappleableMask;
	public LineRenderer m_lineRenderer;

	[Header("Grappling")]
	public float m_maxGrappleDistance;
	public float m_grappleDelayTime;

	[Header("Cooldown")]
	public float m_grapplingCooldown;

	[Header("Input")]
	public KeyCode m_grappleKey = KeyCode.Mouse1;

    #endregion


    #region Unity API
		
	void Update()
	{
		if (Input.GetKeyUp(m_grappleKey)) StartGrapple();

		if(
			_grapplingCooldownTimer > 0) _grapplingCooldownTimer -= Time.deltaTime;
	}

    private void LateUpdate()
    {
		if (_isGrappling) m_lineRenderer.SetPosition(0, m_tongueTip.position);
    }

    #endregion


    #region Main Methods

    private void StartGrapple()
	{
		if (_grapplingCooldownTimer > 0) return;

		_isGrappling = true;
		RaycastHit hit;
		if (Physics.Raycast(m_playerTransform.position, m_playerTransform.forward, out hit, m_maxGrappleDistance, m_whatIsGrappleableMask))
		{
			_grapplePoint = hit.point;

			Invoke(nameof(ExecuteGrapple), m_grappleDelayTime);
		}
		else
		{
			_grapplePoint = m_playerTransform.position + m_playerTransform.forward * m_maxGrappleDistance;
            Invoke(nameof(StopGrapple), m_grappleDelayTime);
        }

		m_lineRenderer.enabled = true;
		m_lineRenderer.SetPosition(1, _grapplePoint);


	}

	private void ExecuteGrapple()
	{
		Debug.Log("Send Grapple");
		m_playerTransform.position = _grapplePoint;
        _isGrappling = false;
        m_lineRenderer.enabled = false;
    }

	private void StopGrapple()
	{
		_isGrappling = false;

		_grapplingCooldownTimer = m_grapplingCooldown;

		m_lineRenderer.enabled = false;
	}

	#endregion


	#region Utils

	#endregion


	#region Privates & Protected

	private Vector3 _grapplePoint;
	[SerializeField] private float _grapplingCooldownTimer;
	private bool _isGrappling;

    #endregion
}