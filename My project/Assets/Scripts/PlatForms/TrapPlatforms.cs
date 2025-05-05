using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatforms : MonoBehaviour, IUpdatable
{
    [Header("Falling Cube")]
    [SerializeField] private Transform _fallingCube;
    [SerializeField] private Transform _objective;
    [SerializeField] private float _fallingSpeed = 200f;
    //[SerializeField] private Vector3 _areaDetection = new Vector3(5f, 5f, 5f);

    private bool _isFalling = false;
    private Transform _player;

    private Vector2 _platformXZ;
    private float _minHeight = 0.5f;
    private float _maxHeight = 2f;
    private float _triggerDistanceSqr = 0.25f;

    private void Awake()
    {
        _platformXZ = new Vector2(transform.position.x, transform.position.z);
    }

    public void SetPlayer(Transform player)
    {
        this._player = player;
    }

    public void Tick(float deltaTime)
    {
        if (_player == null) return;
       
        if (!_isFalling && IsPlayerInArea())
        {
            _isFalling = true;
            _fallingCube.gameObject.SetActive(true);
        }

        if (_isFalling && _fallingCube != null)
        {
            _fallingCube.position = Vector3.MoveTowards(_fallingCube.position, _objective.position, _fallingSpeed * deltaTime);

        }
    }

    private bool IsPlayerInArea()
    {
        Vector3 pos = _player.position;
        float horizontalDistanceSqr = (new Vector2(pos.x, pos.z) - _platformXZ).sqrMagnitude;
        float vertcialOffset = pos.y - transform.position.y;

        return horizontalDistanceSqr < _triggerDistanceSqr && vertcialOffset > _minHeight && vertcialOffset < _maxHeight;
    }

}
