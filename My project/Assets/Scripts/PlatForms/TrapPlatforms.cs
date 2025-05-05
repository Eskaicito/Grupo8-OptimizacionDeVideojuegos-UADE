using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatforms : MonoBehaviour, IUpdatable
{
    [Header("Falling Cube")]
    [SerializeField] private Transform _fallingCube;
    [SerializeField] private Transform _objective;
    [SerializeField] private float _fallingSpeed = 50f;
    [SerializeField] private Vector3 _areaDetection = new Vector3(1f, 1f, 1f);

    private bool _isFalling = false;
    private Transform _player;   

    public void SetPlayer(Transform player)
    {
        this._player = player;
    }

    public void Tick(float deltaTime)
    {
        if(!_isFalling && IsPlayerInArea())
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
        Vector3 playerPosition = _player.position;
        Vector3 platformPosition = transform.position;

        float horizontalDistance = Vector2.Distance(
            new Vector2(playerPosition.x, playerPosition.z),
            new Vector2(platformPosition.x, platformPosition.z)
        );

        float verticalOffset = playerPosition.y - platformPosition.y;

        return horizontalDistance < 0.5f && verticalOffset > 0.5f && verticalOffset < 2f;
    }

}
