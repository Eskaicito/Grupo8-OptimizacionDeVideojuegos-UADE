// Interfaz que define un contrato: cualquier clase que la implemente debe tener un m�todo Tick().
// Esto reemplaza al Update de Unity, d�ndote control total sobre qu� se actualiza y cu�ndo.

public interface IUpdatable
{
    void Tick(float deltaTime); // Se llama una vez por frame desde el CustomUpdateManager
}
