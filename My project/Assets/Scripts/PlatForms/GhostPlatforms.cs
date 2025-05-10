using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlatforms : MonoBehaviour, IUpdatable
{
    private readonly Renderer _renderer;
    private readonly Collider _collider;
    private readonly MaterialPropertyBlock block;
    private readonly Color normalColor;
    private readonly Color warningColor;

    private float timer;
    private float actualInterval;
    private readonly float warningTime;
    private readonly float intervalMin;
    private readonly float intervalMax;

    private bool isActive = true;
    private bool hasWarned = false;

    private static readonly int ColorID = Shader.PropertyToID("_Color");

    public GhostPlatforms(Renderer rend, Collider col, float min, float max, float warning, Color normal, Color warningCol)
    {
        _renderer = rend;
        _collider = col;
        intervalMin = min;
        intervalMax = max;
        warningTime = warning;
        normalColor = normal;
        warningColor = warningCol;

        block = new MaterialPropertyBlock();
        actualInterval = Random.Range(intervalMin, intervalMax);
        timer = 0f;
        SetColor(normalColor);
    }

    public void Tick(float deltaTime)
    {
        timer += deltaTime;

        if (isActive && !hasWarned && actualInterval - timer <= warningTime)
        {
            SetColor(warningColor);
            hasWarned = true;
        }

        if (timer >= actualInterval)
        {
            isActive = !isActive;
            _renderer.enabled = isActive;
            _collider.enabled = isActive;

            timer = 0f;
            actualInterval = Random.Range(intervalMin, intervalMax);
            hasWarned = false;

            if (isActive)
                SetColor(normalColor);
        }
    }

    private void SetColor(Color color)
    {
        block.SetColor(ColorID, color);
        _renderer.SetPropertyBlock(block);
    }
}

