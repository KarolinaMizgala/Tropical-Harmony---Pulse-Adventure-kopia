using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// Represents the game board.
/// </summary>
public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private Transform tilesParent;

    [Inject]
    private DialogSystem dialogSystem;
    [Inject]
    private SceneService sceneManager;
    [Inject]
    private LevelSystem levelSystem;

    private List<Tile> tileList;

    private Vector2Int puzzleSize = new Vector2Int(4, 4);
    private float neighborTileDistance = 136;

    /// <summary>
    /// Gets or sets the position of the empty tile.
    /// </summary>
    public Vector3 EmptyTilePosition { set; get; }

    /// <summary>
    /// Gets the playtime.
    /// </summary>
    public int Playtime { private set; get; } = 0;

    /// <summary>
    /// Gets the move count.
    /// </summary>
    public int MoveCount { private set; get; } = 0;

    /// <summary>
    /// Starts the game when the script instance is being loaded.
    /// </summary>
    private IEnumerator Start()
    {
        tileList = new List<Tile>();

        SpawnTiles();

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(tilesParent.GetComponent<RectTransform>());

        yield return new WaitForEndOfFrame();

        tileList.ForEach(x => x.SetCorrectPosition());

        StartCoroutine("OnSuffle");
        StartCoroutine("CalculatePlaytime");
    }

    /// <summary>
    /// Spawns the tiles.
    /// </summary>
    private void SpawnTiles()
    {
        for (int y = 0; y < puzzleSize.y; ++y)
        {
            for (int x = 0; x < puzzleSize.x; ++x)
            {
                GameObject clone = Instantiate(tilePrefab, tilesParent);
                Tile tile = clone.GetComponent<Tile>();

                tile.Setup(this, puzzleSize.x * puzzleSize.y, y * puzzleSize.x + x + 1);

                tileList.Add(tile);
            }
        }
    }

    /// <summary>
    /// Shuffles the tiles.
    /// </summary>
    private IEnumerator OnSuffle()
    {
        float current = 0;
        float percent = 0;
        float time = 1.5f;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            int index = Random.Range(0, puzzleSize.x * puzzleSize.y);
            tileList[index].transform.SetAsLastSibling();

            yield return null;
        }

        EmptyTilePosition = tileList[tileList.Count - 1].GetComponent<RectTransform>().localPosition;
    }

    /// <summary>
    /// Checks if a tile can be moved.
    /// </summary>
    /// <param name="tile">The tile to check.</param>
    public void IsMoveTile(Tile tile)
    {
        if (Vector3.Distance(EmptyTilePosition, tile.GetComponent<RectTransform>().localPosition) == neighborTileDistance)
        {
            Vector3 goalPosition = EmptyTilePosition;

            EmptyTilePosition = tile.GetComponent<RectTransform>().localPosition;

            tile.OnMoveTo(goalPosition);

            MoveCount++;
        }
    }

    /// <summary>
    /// Checks if the game is over.
    /// </summary>
    public void IsGameOver()
    {
        List<Tile> tiles = tileList.FindAll(x => x.IsCorrected == true);

        Debug.Log("Correct Count : " + tiles.Count);
        if (tiles.Count == puzzleSize.x * puzzleSize.y - 1)
        {
            Debug.Log("GameClear");
            StopCoroutine("CalculatePlaytime");
            levelSystem.AddPoints(5);
            dialogSystem.ShowConfirmationDialog("Congratulations, you've completed the game! Would you like to try again?", () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }, () => { sceneManager.LoadScene(SceneType.RestfulScene); });
        }
    }

    /// <summary>
    /// Calculates the playtime.
    /// </summary>
    private IEnumerator CalculatePlaytime()
    {
        while (true)
        {
            Playtime++;

            yield return new WaitForSeconds(1);
        }
    }
}