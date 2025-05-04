using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatforms : MonoBehaviour, IUpdatable
{
    [Header("Falling Cube")]
    [SerializeField] private Transform _fallingCube;
    [SerializeField] private Transform _objective;
    [SerializeField] private float _fallingSpeed = 3f;

    [SerializeField] private Vector3 _areaDetection = new Vector3(1f, 1f, 1f);

    private bool _isFalling = false;
    private bool _isPlayerOnPlatform = false;

    public void SetPlayerOnPlatform(bool value)
    {
        _isPlayerOnPlatform = value;

        if(_isPlayerOnPlatform && _isFalling)
        {
            _fallingCube.gameObject.SetActive(true);
            _isFalling = true;
        }
    }

    public void Tick(float deltaTime)
    {
        if(!_isFalling && _isPlayerOnPlatform)
        {
            _isFalling = true;
            _fallingCube.gameObject.SetActive(true);
        }

        if (_isFalling && _fallingCube != null)
        {
            _fallingCube.position = Vector3.MoveTowards(_fallingCube.position, _objective.position, _fallingSpeed * deltaTime);

            if (Vector3.Distance(_fallingCube.position, _objective.position) < 0.1f)
            {
                _isFalling = false;
                _fallingCube.gameObject.SetActive(false);
               
            }
        }
    }

    private bool IsPlayerInArea()
    {
        Vector3 center = transform.position + Vector3.up * 0.5f;
        Bounds zone = new Bounds(center, _areaDetection);

        return zone.Contains(transform.position);
    }
}
