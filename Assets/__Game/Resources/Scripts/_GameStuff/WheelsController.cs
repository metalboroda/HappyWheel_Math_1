using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class WheelsController : MonoBehaviour
  {
    [SerializeField] private List<WheelHandler> _wheelsHandlers = new();

    private int _currentWheelIndex = 0;

    private GameBootstrapper _gameBootstrapper;

    private void Awake() {
      _gameBootstrapper = GameBootstrapper.Instance;

      foreach (var wheelHandler in _wheelsHandlers)
        wheelHandler.gameObject.SetActive(false);
    }

    private void OnEnable() {
      foreach (var wheelHandler in _wheelsHandlers)
        wheelHandler.WheelCompleted += ActivateNextWheel;
    }

    private void OnDisable() {
      foreach (var wheelHandler in _wheelsHandlers)
        wheelHandler.WheelCompleted -= ActivateNextWheel;
    }

    private void Start() {
      ActivateNextWheel();
    }

    private void CheckAllWheelsForCompletion() {
      foreach (var wheel in _wheelsHandlers) {
        if (wheel.Completed == false) {
          return;
        }
      }

      if (_gameBootstrapper != null) {
        _gameBootstrapper.StateMachine.ChangeState(new GameWinState(_gameBootstrapper));
      }

      Debug.Log("Win");
    }

    private void ActivateNextWheel() {
      StartCoroutine(DoActivateNextWheel());
    }

    private IEnumerator DoActivateNextWheel() {
      yield return new WaitForEndOfFrame();

      foreach (var wheelHandler in _wheelsHandlers)
        wheelHandler.gameObject.SetActive(false);

      if (_currentWheelIndex < _wheelsHandlers.Count) {
        _wheelsHandlers[_currentWheelIndex].gameObject.SetActive(true);
        _currentWheelIndex++;
      }

      CheckAllWheelsForCompletion();
    }
  }
}