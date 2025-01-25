using System;

[System.Serializable]
public class BlockData
{
    public int blockID;
    public float[] position;

    public BlockData(Block block)
    {
        blockID = block.blockID;
        position = new float[3] { block.transform.position.x, block.transform.position.y, block.transform.position.z };
    }
}
