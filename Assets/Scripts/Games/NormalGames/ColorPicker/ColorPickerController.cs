using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

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

    private void Update()
    {
        gameTime += Time.deltaTime;
    }
    private void Awake()
    {
        blockList = blockSpawner.SpawnBlocks(blockCount);
        for (int i = 0; i < blockList.Count; ++i)
        {
            blockList[i].Setup(this);
        }

        SetColors();
    }

    private void SetColors()
    {
        difficultyModifier *= 0.92f;

        currentColor = colorPalette[Random.Range(0, colorPalette.Length)];

        float diff = (1.0f / 255.0f) * difficultyModifier;
        otherOneColor = new Color(currentColor.r - diff, currentColor.g - diff, currentColor.b - diff);

        otherBlockIndex = Random.Range(0, blockList.Count);
        Debug.Log(otherBlockIndex);

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

    public void CheckBlock(Color color)
    {

        if (blockList[otherBlockIndex].Color == color)
        {
            SetColors();
        }
        else
        {
            if (gameTime > 1f)
            {
                levelSystem.AddPoints(1);
            }
            dialogSystem.ShowConfirmationDialog("Do you want to try again?", Restart, null);
        }
    }
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameTime = 0f;
    }
}

