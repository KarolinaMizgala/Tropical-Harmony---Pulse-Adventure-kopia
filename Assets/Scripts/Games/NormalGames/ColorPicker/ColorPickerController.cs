using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// Controls the color picking game.
/// </summary>
public class ColorPickerController : MonoBehaviour
{
    [SerializeField]
    private Color[] colorPalette;
    [SerializeField]
    private float difficultyModifier;

    [SerializeField]
    [Range(2, 5)]
    private int blockCount = 2;
    [SerializeField]
    private BlockSpawner blockSpawner;

    [Inject] DialogSystem dialogSystem;
    [Inject] LevelSystem levelSystem;
    [Inject] SceneService sceneService;

    private List<Block> blockList = new List<Block>();

    private Color currentColor;
    private Color otherOneColor;

    private int otherBlockIndex;
    private float gameTime = 0f;

    /// <summary>
    /// Updates the game time every frame.
    /// </summary>
    private void Update()
    {
        gameTime += Time.deltaTime;
    }

    /// <summary>
    /// Sets up the game when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        blockList = blockSpawner.SpawnBlocks(blockCount);
        for (int i = 0; i < blockList.Count; ++i)
        {
            blockList[i].Setup(this);
        }

        SetColors();
    }

    /// <summary>
    /// Sets the colors for the blocks.
    /// </summary>
    private void SetColors()
    {
        difficultyModifier *= 0.92f;

        currentColor = colorPalette[Random.Range(0, colorPalette.Length)];

        float diff = (1.0f / 255.0f) * difficultyModifier;
        otherOneColor = new Color(currentColor.r - diff, currentColor.g - diff, currentColor.b - diff);

        otherBlockIndex = Random.Range(0, blockList.Count);

        for (int i = 0; i < blockList.Count; ++i)
        {
            if (i == otherBlockIndex)
            {
                blockList[i].Color = otherOneColor;
            }
            else
            {
                blockList[i].Color = currentColor;
            }
        }
    }

    /// <summary>
    /// Checks if the clicked block is the correct one.
    /// </summary>
    /// <param name="color">The color of the clicked block.</param>
    public void CheckBlock(Color color)
    {
        if (blockList[otherBlockIndex].Color == color)
        {
            SetColors();
        }
        else
        {
            if (gameTime > 2f)
            {
                levelSystem.AddPoints(5);
            }
            dialogSystem.ShowConfirmationDialog("Do you want to try again?", Restart, Back);
        }
    }

    /// <summary>
    /// Goes back to the previous scene.
    /// </summary>
    private void Back()
    {
        sceneService.LoadScene(SceneType.Energetic);
    }

    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameTime = 0f;
    }
}