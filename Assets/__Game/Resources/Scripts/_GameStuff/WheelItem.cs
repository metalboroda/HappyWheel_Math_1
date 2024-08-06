using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  [Serializable]
  public class WheelItem
  {
    public GameObject ItemPrefab;
    public bool ShowSprite;
    public Sprite Sprite;
    public bool ShowValue;
    public string Value;
  }
}