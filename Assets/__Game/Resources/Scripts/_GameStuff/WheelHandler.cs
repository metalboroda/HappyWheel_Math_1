﻿using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class WheelHandler : MonoBehaviour
  {
    public event Action WheelCompleted;
    public event Action<bool> WheelCompletedBool;

    [Header("Central Item")]
    [SerializeField] private bool _showCentralSprite;
    [SerializeField] private Sprite _centralSprite;
    [SerializeField] private bool _showCentralValue;
    [SerializeField] private string _centralValue;
    [SerializeField] private string _centralValueName;

    [Header("Settings")]
    [SerializeField] private float _radius = 5f;
    [Space]
    [SerializeField] private bool constrainX = true;
    [SerializeField] private bool constrainY = false;
    [SerializeField] private bool constrainZ = true;
    [Space]
    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;
    [SerializeField] private bool invertZ = false;

    [Header("References")]
    [SerializeField] private Transform _wheelTransform;
    [SerializeField] private Image _centralImage;
    [SerializeField] private TextMeshProUGUI _centralValueText;

    [Header("")]
    [SerializeField] private List<WheelItem> _wheelItems = new();

    public bool Completed { get; private set; } = false;

    private List<GameObject> _spawnedObjects = new();

    private GameBootstrapper _gameBootstrapper;
    private Camera _mainCamera;

    private void Awake() {
      _gameBootstrapper = GameBootstrapper.Instance;
      _mainCamera = Camera.main;

      ShuffleWheelItems();
    }

    private void OnDisable() {
      foreach (var spawnedObject in _spawnedObjects) {
        spawnedObject.GetComponent<WheelItemHandler>().Clicked -= OnVariantClickedEvent;
      }
    }

    private void Start() {
      SpawnAndArrangeObjects();
      SetCentral();
    }

    private void Update() {
      MaintainChildRotation();
    }

    public void SpawnAndArrangeObjects() {
      SpawnObjects();
      ArrangeObjects();
    }

    private void SpawnObjects() {
      foreach (WheelItem wheelItem in _wheelItems) {
        GameObject spawnedObject = Instantiate(wheelItem.ItemPrefab, _wheelTransform);

        _spawnedObjects.Add(spawnedObject);

        WheelItemHandler wheelItemHandler = spawnedObject.GetComponent<WheelItemHandler>();

        if (wheelItemHandler != null)
          wheelItemHandler.SetItem(wheelItem.ShowSprite, wheelItem.Sprite, wheelItem.ShowValue, wheelItem.Value, wheelItem.Tutorial);
      }

      foreach (var spawnedObject in _spawnedObjects) {
        spawnedObject.GetComponent<WheelItemHandler>().Clicked += OnVariantClickedEvent;
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

          if (constrainX)
            lookAtPosition.x = spawnedObject.transform.position.x;

          if (constrainY)
            lookAtPosition.y = spawnedObject.transform.position.y;

          if (constrainZ)
            lookAtPosition.z = spawnedObject.transform.position.z;

          Vector3 direction = lookAtPosition - spawnedObject.transform.position;

          if (invertX)
            direction.x = -direction.x;

          if (invertY)
            direction.y = -direction.y;

          if (invertZ)
            direction.z = -direction.z;

          Quaternion targetRotation = Quaternion.LookRotation(direction);

          spawnedObject.transform.rotation = targetRotation;
        }
      }
    }

    private void ShuffleWheelItems() {
      System.Random rng = new System.Random();
      int n = _wheelItems.Count;

      while (n > 1) {
        n--;

        int k = rng.Next(n + 1);
        WheelItem value = _wheelItems[k];

        _wheelItems[k] = _wheelItems[n];
        _wheelItems[n] = value;
      }
    }

    private void SetCentral() {
      if (_showCentralSprite && _centralSprite != null) {
        _centralImage.sprite = _centralSprite;
      }

      if (_showCentralValue) {
        _centralValueText.text = _centralValueName;
      }
    }

    private void OnVariantClickedEvent(string value) {
      if (value == _centralValue) {
        Completed = true;

        WheelCompleted?.Invoke();
      }
      else {
        Completed = false;

        if (_gameBootstrapper != null)
          _gameBootstrapper.StateMachine.ChangeState(new GameLoseState(_gameBootstrapper));
      }

      WheelCompletedBool?.Invoke(Completed);
    }
  }
}