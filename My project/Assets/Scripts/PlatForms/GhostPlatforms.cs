using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlatforms : MonoBehaviour, IUpdatable
{
    [SerializeField] private float _intervalMin = 3f;
    [SerializeField] private float _intervalMax = 5f;
    [SerializeField] private float _warningTime = 1f; // Warning time before disappearing
    [SerializeField] private Color _normalColor = Color.white; 
    [SerializeField] private Color _warningColor = Color.red; 

    private float _timer;
    private float _actualInterval;
    private bool _isActive =  true;
    private bool _hasWarned = false;

    private Renderer _renderer;
    private Collider _collider;
    private MaterialPropertyBlock _materialBlock;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        _materialBlock = new MaterialPropertyBlock();

        _actualInterval = Random.Range(_intervalMin, _intervalMax);
        _isActive = true;
        _timer = 0f;
        _hasWarned = false;

        SetColor(_normalColor);
    }

    public void Tick(float deltaTime)
    {
        _timer += deltaTime;

        if(_isActive && !_hasWarned && _actualInterval - _timer <= _warningTime)
        {
            // Change color to warning color
            SetColor(_warningColor);
            _hasWarned = true;
        }
      


        if (_timer >= _actualInterval)
        {
            _isActive = !_isActive;
            _renderer.enabled = _isActive;
            _collider.enabled = _isActive;

            _timer = 0f;
            _actualInterval = Random.Range(_intervalMin, _intervalMax);
            _hasWarned = false;

            // Reset color to normal color
            if (_isActive)
            {
                SetColor(_normalColor);
            }

        }
    }

    private void SetColor(Color color)
    {
        _materialBlock.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_materialBlock);
    }


}

