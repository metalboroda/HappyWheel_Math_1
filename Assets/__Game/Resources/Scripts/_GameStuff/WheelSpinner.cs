using DG.Tweening;
using UnityEngine;

public class WheelSpinner : MonoBehaviour
{
  [SerializeField] private float _spinSpeed = 10f;
  [Space]
  [SerializeField] private Transform _wheelTransform;
  [Space]
  [SerializeField] private Vector3 _rotationDirection;

  private void Start() {
    Rotate();
  }

  private void Rotate() {
    _wheelTransform.DORotate(_rotationDirection, _spinSpeed, RotateMode.FastBeyond360)
      .SetSpeedBased(true)
      .SetLoops(-1);
  }

  private void StopRotate() {
    DOTween.Kill(_wheelTransform);
  }
}