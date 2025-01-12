namespace Common;

public class Cell2D
{
    public readonly int X;
    public readonly int Y;
    public Cell2D? Up { get; private set; }
    public Cell2D? Right { get; private set; }
    public Cell2D? Down { get; private set; }
    public Cell2D? Left { get; private set; }
    
    public Cell2D() {}

    public Cell2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void ConnectUp(Cell2D up, bool bothWays = true)
    {
        Up = up;
        if (bothWays)
            up.Down = this;
    }
    
    public void ConnectRight(Cell2D right, bool bothWays = true)
    {
        Right = right;
        if (bothWays)
            right.Left = this;
    }
    
    public void ConnectDown(Cell2D down, bool bothWays = true)
    {
        Down = down;
        if (bothWays)
            down.Up = this;
    }
    
    public void ConnectLeft(Cell2D left, bool bothWays = true)
    {
        Left = left;
        if (bothWays)
            left.Right = this;
    }
}