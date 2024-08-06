using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Tools
{
  public class CanvasLookAtCamera : MonoBehaviour
  {
    private Camera _targetCamera;

    private void Awake() {
      _targetCamera = Camera.main;
    }

    void Update() {
      if (_targetCamera == null) {
        _targetCamera = Camera.main;
      }

      transform.LookAt(transform.position + _targetCamera.transform.rotation * Vector3.forward,
                       _targetCamera.transform.rotation * Vector3.up);
    }
  }
}