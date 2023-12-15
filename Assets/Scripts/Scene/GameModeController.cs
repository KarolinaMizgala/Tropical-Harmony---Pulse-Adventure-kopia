using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameMode
{
    ServerMode,
    ManualMode
}
public class GameModeController : MonoBehaviour
{
    private GameMode currentMode;

    public void SetGameMode(GameMode mode)
    {
        currentMode = mode;

    }

   public GameMode GetGameMode()
    {
        return currentMode; 
    }
}
