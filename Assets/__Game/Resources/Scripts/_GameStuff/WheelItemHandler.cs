using __Game.Resources.Scripts.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class WheelItemHandler : MonoBehaviour, IPointerClickHandler
  {
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

    private Color[] _colors;

    private void Awake() {
      _colors = new Color[] { _colorA, _colorB, _colorC, _colorD, _colorE };
    }

    private void Start() {
      SetRandomColor();
    }

    public void SetItem(bool showSprite, Sprite sprite, bool showValue, string value) {
      if (sprite != null || showSprite == false)
        _spriteRenderer.sprite = sprite;

      if (showValue == false)
        _valueText.text = value;
    }

    private void SetRandomColor() {
      int randomIndex = Random.Range(0, _colors.Length);

      _borderRenderer.color = _colors[randomIndex];
    }

    public void OnPointerClick(PointerEventData eventData) {
      EventBus<EventStructs.VariantClickedEvent>.Raise(new EventStructs.VariantClickedEvent {
        VariantValue = _valueText.text
      });
    }
  }
}