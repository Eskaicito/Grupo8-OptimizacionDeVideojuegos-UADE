using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que maneja la lógica de las plataformas fantasma.
// Estas plataformas aparecen y desaparecen en intervalos aleatorios.
// También cambian de color cuando están a punto de desaparecer.

public class GhostPlatforms : IUpdatable
{
    private readonly Renderer _renderer;
    private readonly Collider _collider;
    private static readonly MaterialPropertyBlock block = new MaterialPropertyBlock();

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


    // Constructor que inicializa las propiedades necesarias para la lógica de las plataformas fantasma.
    // El constructor recibe el Renderer y Collider de la plataforma, los intervalos de tiempo, el color normal y el color de advertencia.
    public GhostPlatforms(Renderer rend, Collider col, float min, float max, float warning, Color normal, Color warningCol)
    {
        _renderer = rend;
        _collider = col;
        intervalMin = min;
        intervalMax = max;
        warningTime = warning;
        normalColor = normal;
        warningColor = warningCol;

        InitializeInterval();
        SetColor(normalColor);
    }


    // Este método se llama cada frame para actualizar el estado de las plataformas fantasma.
    // Se encarga de manejar el temporizador y alternar la visibilidad de la plataforma.
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

            InitializeInterval();
            hasWarned = false;

            if (isActive)
                SetColor(normalColor);
        }
    }

    // Método que se encarga de cambiar el color de la plataforma.
    // Utiliza un MaterialPropertyBlock para evitar la creación de instancias innecesarias.
    private void SetColor(Color color)
    {
        block.SetColor(ColorID, color);
        _renderer.SetPropertyBlock(block);
    }

    // Método que inicializa el temporizador y establece un intervalo aleatorio entre las plataformas.
    // Utiliza Random.Range para establecer un intervalo entre el mínimo y el máximo especificado.
    private void InitializeInterval()
    {
        timer = 0f;
        actualInterval = Random.Range(intervalMin, intervalMax);
    }
}

