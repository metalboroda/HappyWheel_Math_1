using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using System.Collections;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  [RequireComponent(typeof(AudioSource))]
  public class WheelAudioHandler : MonoBehaviour
  {
    [SerializeField] private AudioClip _audioClip;
    [Space]
    [SerializeField] private float _delay = 0.5f;

    private AudioSource _audioSource;

    private GameBootstrapper _gameBootstrapper;

    private EventBinding<EventStructs.StateChanged> _stateBinding;

    private void Awake() {
      _audioSource = GetComponent<AudioSource>();

      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable() {
      _stateBinding = new EventBinding<EventStructs.StateChanged>(OnStateChanged);
    }

    private void OnDisable() {
      _stateBinding.Remove(OnStateChanged);
    }

    private void Start() {
      if (_gameBootstrapper.StateMachine.CurrentState is GameplayState)
        StartCoroutine(DoPlayAudioCLip());
    }

    private void PlayAudioClip() {
      _audioSource.PlayOneShot(_audioClip);
    }

    private void OnStateChanged(EventStructs.StateChanged stateChanged) {
      if (stateChanged.State is GameplayState)
        PlayAudioClip();
    }

    private IEnumerator DoPlayAudioCLip() {
      yield return new WaitForSeconds(_delay);

      PlayAudioClip();
    }
  }
}