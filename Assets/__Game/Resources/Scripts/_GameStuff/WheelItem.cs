using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  [Serializable]
  public class WheelItem
  {
    public GameObject ItemPrefab;
    [Space]
    public bool ShowSprite;
    public Sprite Sprite;
    [Space]
    public bool ShowValue;
    public string Value;
    [Space]
    public bool Tutorial;
  }
}