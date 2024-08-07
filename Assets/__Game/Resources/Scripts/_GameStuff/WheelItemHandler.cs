using __Game.Resources.Scripts.EventBus;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class WheelItemHandler : MonoBehaviour, IPointerDownHandler
  {
    public event Action<string> Clicked;

    [Header("References")]
    [SerializeField] private SpriteRenderer _borderRenderer;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshProUGUI _valueText;

    [Header("Colors")]
    [SerializeField] private Color _colorA;
    [SerializeField] private Color _colorB;
    [SerializeField] private Color _colorC;
    [SerializeField] private Color _colorD;
    [SerializeField] private Color _colorE;

    [Header("Tutorial")]
    [SerializeField] private GameObject _tutorialFinger;

    private Color[] _colors;

    private void Awake() {
      _colors = new Color[] { _colorA, _colorB, _colorC, _colorD, _colorE };
    }

    private void Start() {
      SetRandomColor();
    }

    public void SetItem(bool showSprite, Sprite sprite, bool showValue, string value, bool tutorial) {
      _valueText.gameObject.SetActive(false);
      _tutorialFinger.SetActive(false);

      if (sprite != null) {
        _spriteRenderer.sprite = sprite;

        if (showSprite == true)
          _spriteRenderer.gameObject.SetActive(true);
        else
          _spriteRenderer.gameObject.SetActive(false);
      }
      else
        _spriteRenderer.gameObject.SetActive(false);

      if (value != "" || value != " ")
        _valueText.text = value;

      _valueText.gameObject.SetActive(showValue);
      _tutorialFinger.SetActive(tutorial);
    }

    private void SetRandomColor() {
      int randomIndex = Random.Range(0, _colors.Length);

      _borderRenderer.color = _colors[randomIndex];
    }

    public void OnPointerDown(PointerEventData eventData) {
      Clicked?.Invoke(_valueText.text);

      Debug.Log(_valueText.text);
    }
  }
}