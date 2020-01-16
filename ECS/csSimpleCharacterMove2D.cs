using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class csSimpleCharacterMove2D : MonoBehaviour
{

  #region PrivateData
  private Rigidbody2D rb;
  private PlayerInput m_pInput;
  [Range(0.0f, 50.0f)]
  [SerializeField]private float m_plyrMoveSpeed;
  private float2 velocity;
  #endregion

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    m_pInput = new PlayerInput();
    m_pInput.Player.Enable();
    m_pInput.Player.LeftStick.performed += ctx => movePlayer(ctx.ReadValue<Vector2>());
  }

  void Update()
  {
        
  }

  void movePlayer(Vector2 _lStick) {
    rb.velocity += new Vector2(_lStick.x * Time.deltaTime * m_plyrMoveSpeed,
                               _lStick.y * Time.deltaTime * m_plyrMoveSpeed);
  }
}
