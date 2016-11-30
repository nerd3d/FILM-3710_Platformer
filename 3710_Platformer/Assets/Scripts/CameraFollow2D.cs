using System;
using UnityEngine;
using Prime31;

public class CameraFollow2D : MonoBehaviour {
  public Transform target;

  public float damping = 0.1f;
  public float lookAheadFactor = 0.5f;
  public float lookAheadReturnSpeed = 0.5f;
  public float lookAheadMoveThreshold = 0.1f;

  public bool platformSnap = true;

  private float m_OffsetZ;
  private Vector3 m_LastTargetPosition;
  private Vector3 m_CurrentVelocity;
  private Vector3 m_LookAheadPos;

  private Transform currentTarget;

  private int shakeFrames = 0;
  private float shakeMag = 0.5f;
  private System.Random randInt;

  // Use this for initialization
  private void Start() {
    randInt = new System.Random();
    
  }

  /// <summary>
  /// Used to apply a simple camera shake
  /// </summary>
  /// <param name="duration">number of frames to apply the "shake"</param>
  public void impact(int duration) {
    shakeFrames = duration;
  }

  // Update is called once per frame
  private void Update() {
    // only update lookahead pos if accelerating or changed direction
    float xMoveDelta = (currentTarget.position - m_LastTargetPosition).x;

    bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

    if (updateLookAheadTarget) {
      m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
    } else {
      m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
    }

    Vector3 aheadTargetPos = currentTarget.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
    Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

    if (shakeFrames > 0) {
      float deltaX = (float)randInt.NextDouble()*shakeMag - 0.5f*shakeMag;
      float deltaY = (float)randInt.NextDouble()*shakeMag - 0.5f*shakeMag;
      newPos = new Vector3(newPos.x + deltaX, newPos.y + deltaY,newPos.z);
      shakeFrames--;
    }

    transform.position = newPos;

    m_LastTargetPosition = currentTarget.position;
  }

  public void startCameraFollow(GameObject newTarget) {
    currentTarget = newTarget.transform;
    m_LastTargetPosition = currentTarget.position;
    m_OffsetZ = transform.position.z;
    transform.parent = null;
  }


  public void stopCameraFollow() {

    currentTarget = this.transform;
    m_LastTargetPosition = currentTarget.position;
    m_OffsetZ = 0;
  }

  public void setDamping(float value) {
    damping = value;
  }

  public void setTarget(GameObject newTarget) {
    currentTarget = newTarget.transform;
  }
}

