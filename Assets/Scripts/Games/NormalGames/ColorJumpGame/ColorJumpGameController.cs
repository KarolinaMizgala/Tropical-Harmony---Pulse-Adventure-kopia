using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

/// <summary>
/// Main controller for the ColorJump game.
/// </summary>
public class ColorJumpGameController : MonoBehaviour
{
    [Inject] DialogSystem dialogSystem;
    [Inject] SceneService sceneService;
    [Inject] LevelSystem levelSystem;
    [SerializeField] private Transform wallPrefab;
    [SerializeField] private Transform leftWalls;
    [SerializeField] private Transform rightWalls;
    [SerializeField] private TMP_Text score;

    [SerializeField] private int currentLevel = 1;
    private int maxLevel = 7;
    private int currentScore = 0;

    [SerializeField] private List<Color32> colors;
    [SerializeField] private ColorJumpPlayer player;

    private readonly float wallMaxScaleY = 108;
    private readonly int[] wallCountByLevel = new int[7] { 1, 2, 3, 4, 5, 6, 7 };
    private readonly int[] needLevelUpScore = new int[7] { 1, 2, 4, 8, 16, 32, 64 };
    private float gameTime = 0f;
    private GameObject toolTip;
/// <summary>
    /// Coroutine that waits until the tooltip becomes inactive, then spawns walls and sets colors.
    /// </summary>
    private IEnumerator Start()
    {
        toolTip = GameObject.Find("Tooltip");

        // Wait until the tooltip becomes inactive
        while (toolTip.activeSelf)
        {
            yield return null;
        }
        SpawnWalls();
        SetColors();
        yield return null;
    }
        /// <summary>
    /// Updates the game state every frame.
    /// </summary>
    private void Update()
    {
        if (toolTip.activeSelf || dialogSystem.IsDialogVisible())
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        gameTime += Time.deltaTime;
        
    }
    /// <summary>
    /// Spawns walls based on the current level.
    /// </summary>
    private void SpawnWalls()
    {
        int numberOfWalls = wallCountByLevel[currentLevel - 1];

        int currentWallCount = leftWalls.childCount;
        if (currentWallCount < numberOfWalls)
        {
            for (int i = 0; i < numberOfWalls - currentWallCount; ++i)
            {
                Instantiate(wallPrefab, leftWalls);
                Instantiate(wallPrefab, rightWalls);
            }
        }
        for (int i = 0; i < numberOfWalls; ++i)
        {
            Vector3 scale = new Vector3(2f, 1, 1);
            scale.y = wallMaxScaleY / numberOfWalls;

            Vector3 position = Vector3.zero;
            position.y = scale.y * (numberOfWalls / 2 - i) - (numberOfWalls % 2 == 0 ? scale.y / 2 : 0);

            SetTransform(leftWalls.GetChild(i), position, scale);
            SetTransform(rightWalls.GetChild(i), position, scale);
        }

    }
      /// <summary>
    /// Sets the position and scale of a transform.
    /// </summary>
    private void SetTransform(Transform transform, Vector3 position, Vector3 scale)
    {
        transform.localPosition = position;
        transform.localScale = scale;
    }
        /// <summary>
    /// Sets the colors of the walls and player.
    /// </summary>
    private void SetColors()
    {
        var tempColors = new List<Color32>();
        int[] indexs = Utils.RandomNumerics(colors.Count, wallCountByLevel[currentLevel - 1]);
        for (int i = 0; i < indexs.Length; ++i)
        {
            tempColors.Add(colors[indexs[i]]);
        }

        int colorCount = tempColors.Count;
        int[] leftWallIndexs = Utils.RandomNumerics(colorCount, colorCount);
        for (int i = 0; i < colorCount; ++i)
        {
            leftWalls.GetChild(i).GetComponent<SpriteRenderer>().color = tempColors[leftWallIndexs[i]];
        }

        int[] rightWallIndexs = Utils.RandomNumerics(colorCount, colorCount);
        for (int i = 0; i < colorCount; ++i)
        {
            rightWalls.GetChild(i).GetComponent<SpriteRenderer>().color = tempColors[rightWallIndexs[i]];
        }

        int index = Random.Range(0, tempColors.Count);
        player.GetComponent<SpriteRenderer>().color = tempColors[index];
    }
     /// <summary>
    /// Handles collision with a wall.
    /// </summary>
    public void CollisionWithWall()
    {
        currentScore++;

        score.text = currentScore.ToString(); 
        if (currentLevel < maxLevel && needLevelUpScore[currentLevel] < currentScore)
        {
            currentLevel++;
            SpawnWalls();
        }
        SetColors();
    }
     /// <summary>
    /// Handles game over.
    /// </summary>
    public void GameOver()
    {
        if (gameTime > 2f)
        {
            levelSystem.AddPoints(5);
        }
        dialogSystem.ShowConfirmationDialog("Do you want to try again?", () =>
        {
            sceneService.LoadScene(SceneType.ColorJumpGame);
        }, () =>
        {
            sceneService.LoadScene(SceneType.Energetic);
        });
    }
}
