using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlatforms : MonoBehaviour, IUpdatable
{
    [SerializeField] private float _intervalMin = 1f;
    [SerializeField] private float _intervalMax = 3f;

    private float _timer;
    private float _actualInterval;
    private bool _isActive =  true;

    public void Tick(float deltaTime)
    {
        _timer += deltaTime;

        if (_timer >= _actualInterval)
        {
            _isActive = !_isActive;
            gameObject.SetActive(_isActive);
            _timer = 0f;
            _actualInterval = Random.Range(_intervalMin, _intervalMax);

        }
    }

    private void OnEnable()
    {
        _actualInterval = Random.Range(_intervalMin, _intervalMax);
        _isActive = true;
        _timer = 0f;
    }
}

