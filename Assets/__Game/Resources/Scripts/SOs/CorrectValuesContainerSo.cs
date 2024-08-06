using UnityEngine;

namespace Assets.__Game.Resources.Scripts.SOs
{
  [CreateAssetMenu(fileName = "CorrectValuesContainer", menuName = "SOs/Containers/CorrectValuesContainer")]
  public class CorrectValuesContainerSo : ScriptableObject
  {
    [field: SerializeField] public string[] CorrectValues { get; private set; }
  }
}