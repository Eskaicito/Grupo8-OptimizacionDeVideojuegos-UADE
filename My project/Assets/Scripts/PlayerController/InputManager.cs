using UnityEngine;

public class InputManager : IUpdatable
{
    public Vector3 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }

    public void Tick(float deltaTime)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        MoveInput = new Vector3(x, 0f, z);
        if (MoveInput.sqrMagnitude > 1f)
            MoveInput.Normalize();

        JumpPressed = Input.GetKeyDown(KeyCode.Space);
    }
}
