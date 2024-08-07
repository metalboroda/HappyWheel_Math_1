using DG.Tweening;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class WheelVisualHandler : MonoBehaviour
  {
    [Header("Animation")]
    [SerializeField] private float _scaleDuration = 0.25f;
    [SerializeField] private float _scaleDelay = 0.25f;

    [Header("VFX")]
    [SerializeField] private GameObject _correctParticles;
    [SerializeField] private GameObject _incorrectParticles;

    private Vector3 _initScale;

    private WheelHandler _wheelHandler;

    private void Awake() {
      _wheelHandler = GetComponent<WheelHandler>();

      _initScale = transform.localScale;
      transform.localScale = Vector3.zero;
    }

    private void OnEnable() {
      _wheelHandler.WheelCompletedBool += OnWheelCorrect;
    }

    private void OnDisable() {
      _wheelHandler.WheelCompletedBool -= OnWheelCorrect;
    }

    private void Start() {
      transform.DOScale(1, _scaleDuration)
        .SetDelay(_scaleDelay);
    }

    private void OnWheelCorrect(bool correct) {
      if (correct == true)
        Instantiate(_correctParticles, transform.position, Quaternion.identity);
      else
        Instantiate(_incorrectParticles, transform.position, Quaternion.identity);
    }
  }
}