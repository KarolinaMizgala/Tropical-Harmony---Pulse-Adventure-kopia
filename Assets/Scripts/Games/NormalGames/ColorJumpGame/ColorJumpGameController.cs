using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

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


    private void Awake()
    {
       
        SpawnWalls();
        SetColors();
    }
    private void Update()
    {
        gameTime += Time.deltaTime;
        
    }
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
    private void SetTransform(Transform transform, Vector3 position, Vector3 scale)
    {
        transform.localPosition = position;
        transform.localScale = scale;
    }
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
