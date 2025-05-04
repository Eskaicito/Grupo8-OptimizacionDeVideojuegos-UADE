using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlatforms : MonoBehaviour, IUpdatable
{
    [SerializeField] private float _intervalMin = 3f;
    [SerializeField] private float _intervalMax = 5f;

    private float _timer;
    private float _actualInterval;
    private bool _isActive =  true;

    private Renderer _renderer;
    private Collider _collider;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();

        _actualInterval = Random.Range(_intervalMin, _intervalMax);
        _isActive = true;
        _timer = 0f;
    }

    public void Tick(float deltaTime)
    {
        _timer += deltaTime;

        if (_timer >= _actualInterval)
        {
            _isActive = !_isActive;
            _renderer.enabled = _isActive;
            _collider.enabled = _isActive;

            _timer = 0f;
            _actualInterval = Random.Range(_intervalMin, _intervalMax);

        }
    }

    //private void OnEnable()
    //{
    //    _actualInterval = Random.Range(_intervalMin, _intervalMax);
    //    _isActive = true;
    //    _timer = 0f;
    //}
}

