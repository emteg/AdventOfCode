namespace Common;

public class Cell2D<T>
{
    public T? Payload { get; set; } = default;
    public Cell2D<T>? Up { get; private set; }
    public Cell2D<T>? Right { get; private set; }
    public Cell2D<T>? Down { get; private set; }
    public Cell2D<T>? Left { get; private set; }

    public void ConnectUp(Cell2D<T> up, bool bothWays = true)
    {
        Up = up;
        if (bothWays)
            up.Down = this;
    }
    
    public void ConnectRight(Cell2D<T> right, bool bothWays = true)
    {
        Right = right;
        if (bothWays)
            right.Left = this;
    }
    
    public void ConnectDown(Cell2D<T> down, bool bothWays = true)
    {
        Down = down;
        if (bothWays)
            down.Up = this;
    }
    
    public void ConnectLeft(Cell2D<T> left, bool bothWays = true)
    {
        Left = left;
        if (bothWays)
            left.Right = this;
    }
}