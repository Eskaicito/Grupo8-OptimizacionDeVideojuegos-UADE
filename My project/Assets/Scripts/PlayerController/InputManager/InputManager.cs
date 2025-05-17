using UnityEngine;

/// <summary>
/// Gestor de entrada personalizado. Captura inputs de movimiento, salto, cámara y salida.
/// Se ejecuta mediante IUpdatable.
/// </summary>
public class InputManager : IUpdatable
{
    public Vector3 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool ExitPressed { get; private set; }
    public float CameraYawInput { get; private set; }

    public void Tick(float deltaTime)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        MoveInput = new Vector3(x, 0f, z);
        if (MoveInput.sqrMagnitude > 1f)
            MoveInput.Normalize();

        JumpPressed = Input.GetKeyDown(KeyCode.Space);
        ExitPressed = Input.GetKeyDown(KeyCode.Escape);

        // ✅ Captura de input para la rotación de cámara
        CameraYawInput = 0f;
        if (Input.GetKey(KeyCode.Q)) CameraYawInput = -1f;
        else if (Input.GetKey(KeyCode.E)) CameraYawInput = 1f;
    }
}
