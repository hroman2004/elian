using System;

// Clase estatica (no es un MonoBehaviour): no vive en ningun GameObject,
// asi que su estado sobrevive cuando PlayerDeath recarga la escena con
// SceneManager.LoadScene. Se resetea solo si cerras y volves a abrir
// Play Mode (domain reload), que es el comportamiento esperado.
public static class LivesManager
{
    public const int StartingLives = 3;

    public static int CurrentLives { get; private set; } = StartingLives;

    public static event Action<int> LivesChanged;
    public static event Action GameOver;

    // Llamar esto desde PlayerDeath cuando Elian muere.
    // Devuelve true si todavia quedan vidas (hay que reiniciar el nivel),
    // false si se acabaron (hay que ir a Game Over).
    public static bool LoseLife()
    {
        if (CurrentLives <= 0)
            return false;

        CurrentLives--;
        LivesChanged?.Invoke(CurrentLives);

        if (CurrentLives <= 0)
        {
            GameOver?.Invoke();
            return false;
        }

        return true;
    }

    // Llamar esto al arrancar una partida nueva de cero (ej. desde un menu principal).
    public static void ResetLives()
    {
        CurrentLives = StartingLives;
        LivesChanged?.Invoke(CurrentLives);
    }
}
