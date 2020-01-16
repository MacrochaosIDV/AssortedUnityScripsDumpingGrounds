using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMoveAndCam : MonoBehaviour
{
  #region PlayerAndCamData
  private Vector3 m_pPos;
  #endregion

  #region PublicEditorData
  [Range(1.0f, 10.0f)]
  public float m_camDist;

  [Range(0.0f, 10.0f)]
  public float m_plyrMoveSpeed;

  [Range(1.0f, 180.0f)]
  public float m_camRotateSpeed;
  private PlayerInput m_pInput;
  #endregion

  void Start()
  {
    m_pInput = new PlayerInput();
    m_pInput.Player.Enable();
    m_pInput.Player.LeftStick.performed += ctx => movePlayer(ctx.ReadValue<Vector2>());
    m_pInput.Player.RightStick.performed += ctx => rotateCam(ctx.ReadValue<Vector2>());
    m_pInput.Player.WASD.performed += ctx => movePlayer(ctx.ReadValue<Vector2>());
  }

  void Update()
  {

  }

  void rotateCam(Vector2 _rStick) {
    Debug.Log("Rotating");
  }

  void movePlayer(Vector2 _lStick) {
    Debug.Log("Moving");
    m_pPos = new Vector3(transform.position.x + _lStick.x * Time.deltaTime * m_plyrMoveSpeed, 
                         transform.position.y, 
                         transform.position.z + _lStick.y * Time.deltaTime * m_plyrMoveSpeed);
    transform.position = m_pPos;
  }
}
