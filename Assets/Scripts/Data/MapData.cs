using System;
using System.Collections.Generic;

[System.Serializable]
public class MapData
{
    public List<BlockData> blocks;

    public MapData(List<Block> mapBlocks)
    {
        blocks = new List<BlockData>();
        foreach (Block block in mapBlocks)
        {
            blocks.Add(new BlockData(block));
        }
    }
}
