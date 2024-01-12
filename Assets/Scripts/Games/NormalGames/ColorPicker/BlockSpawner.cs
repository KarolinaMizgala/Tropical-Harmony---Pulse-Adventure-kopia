using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Spawns blocks in the game.
/// </summary>
public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    private Block blockPrefab;
    [SerializeField]
    private GridLayoutGroup gridLayout;

    /// <summary>
    /// Spawns a specified number of blocks.
    /// </summary>
    /// <param name="blockCount">The number of blocks to spawn.</param>
    /// <returns>A list of the spawned blocks.</returns>
    public List<Block> SpawnBlocks(int blockCount)
    {
        List<Block> blockList = new List<Block>(blockCount * blockCount);

        int cellSize = 300 - 50 * (blockCount - 2);
        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        gridLayout.constraintCount = blockCount;

        for (int y = 0; y < blockCount; ++y)
        {
            for (int x = 0; x < blockCount; ++x)
            {
                Block block = Instantiate(blockPrefab, gridLayout.transform);
                blockList.Add(block);
            }
        }

        return blockList;
    }
}