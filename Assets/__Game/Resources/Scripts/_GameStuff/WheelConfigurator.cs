using Assets.__Game.Resources.Scripts._GameStuff;
using System.Collections.Generic;
using UnityEngine;

public class WheelConfigurator : MonoBehaviour
{
  [SerializeField] private float _radius = 5f;
  [Space]
  [SerializeField] private List<WheelItem> _wheelItems = new();
  [SerializeField] private bool constrainX = true;
  [SerializeField] private bool constrainY = false;
  [SerializeField] private bool constrainZ = true;
  [Space]
  [SerializeField] private bool invertX = false;
  [SerializeField] private bool invertY = false;
  [SerializeField] private bool invertZ = false;

  private List<GameObject> _spawnedObjects = new();
  private Camera _mainCamera;

  private void Start() {
    _mainCamera = Camera.main;
    SpawnAndArrangeObjects();
  }

  private void Update() {
    MaintainChildRotation();
  }

  public void SpawnAndArrangeObjects() {
    SpawnObjects();
    ArrangeObjects();
  }

  private void SpawnObjects() {
    foreach (Transform child in transform) {
      DestroyImmediate(child.gameObject);
    }

    _spawnedObjects.Clear();

    foreach (WheelItem wheelItem in _wheelItems) {
      GameObject spawnedObject = Instantiate(wheelItem.ItemPrefab, transform);

      _spawnedObjects.Add(spawnedObject);

      WheelItemHandler wheelItemHandler = spawnedObject.GetComponent<WheelItemHandler>();

      if (wheelItemHandler != null)
        wheelItemHandler.SetItem(wheelItem.Sprite, wheelItem.Value);
    }
  }

  private void ArrangeObjects() {
    int count = _spawnedObjects.Count;
    List<Vector3> positions = new List<Vector3>();
    int index = 0;

    foreach (GameObject spawnedObject in _spawnedObjects) {
      float angle = index * (360f / count);
      Vector3 position = GetPositionOnWheel(angle);

      positions.Add(position);
      index++;
    }

    index = 0;

    foreach (GameObject spawnedObject in _spawnedObjects) {
      if (spawnedObject != null) {
        spawnedObject.transform.localPosition = positions[index];
      }
      index++;
    }
  }

  private Vector3 GetPositionOnWheel(float angle) {
    float rad = angle * Mathf.Deg2Rad;
    float x = Mathf.Cos(rad) * _radius;
    float y = Mathf.Sin(rad) * _radius;

    return new Vector3(x, y, 0);
  }

  private void MaintainChildRotation() {
    foreach (GameObject spawnedObject in _spawnedObjects) {
      if (spawnedObject != null) {
        Vector3 lookAtPosition = _mainCamera.transform.position;

        if (constrainX) {
          lookAtPosition.x = spawnedObject.transform.position.x;
        }
        if (constrainY) {
          lookAtPosition.y = spawnedObject.transform.position.y;
        }
        if (constrainZ) {
          lookAtPosition.z = spawnedObject.transform.position.z;
        }

        Vector3 direction = lookAtPosition - spawnedObject.transform.position;
        if (invertX) {
          direction.x = -direction.x;
        }
        if (invertY) {
          direction.y = -direction.y;
        }
        if (invertZ) {
          direction.z = -direction.z;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        spawnedObject.transform.rotation = targetRotation;
      }
    }
  }
}