using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;

public class csLoadMain : MonoBehaviour
{
  /// <summary>
  /// 
  /// </summary>
  #region IntenalVars
  private PlayerInput m_pInput;
  #endregion
  void Start() {
    m_pInput = new PlayerInput();
    m_pInput.UI.Confirm.performed += ctx => btnOnClick("SampleScene");
  }

  void Update() {

  }

  /// <summary>
  /// Loads the sample scene
  /// </summary>
  /// <param name="_scnName"></param>
  public void btnOnClick(string _scnName) {
    SceneManager.LoadScene(_scnName);
  }
}
