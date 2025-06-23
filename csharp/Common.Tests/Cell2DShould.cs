namespace Common.Tests;

public class Cell2DShould
{
    [Fact]
    public void HaveAllItsNeighborsSetToNull_WhenCreating()
    {
        Cell2D cell = new();
        
        Assert.Null(cell.Up);
        Assert.Null(cell.UpRight);
        Assert.Null(cell.Right);
        Assert.Null(cell.DownRight);
        Assert.Null(cell.Down);
        Assert.Null(cell.DownLeft);
        Assert.Null(cell.Left);
        Assert.Null(cell.UpLeft);
    }

    [Fact]
    public void HaveTheCorrectNeighbor_AndConnectBothWays_WhenConnectingToAnotherCell()
    {
        Cell2D cell = new();
        Cell2D upNeighbor = new();
        Cell2D upRightNeighbor = new();
        Cell2D rightNeighbor = new();
        Cell2D downRightNeighbor = new();
        Cell2D downNeighbor = new();
        Cell2D downLeftNeighbor = new();
        Cell2D leftNeighbor = new();
        Cell2D upLeftNeighbor = new();

        cell.ConnectUp(upNeighbor);
        cell.ConnectUpRight(upRightNeighbor);
        cell.ConnectRight(rightNeighbor);
        cell.ConnectDownRight(downRightNeighbor);
        cell.ConnectDown(downNeighbor);
        cell.ConnectDownLeft(downLeftNeighbor);
        cell.ConnectLeft(leftNeighbor);
        cell.ConnectUpLeft(upLeftNeighbor);

        Assert.Equal(upNeighbor, cell.Up);
        Assert.Equal(cell, upNeighbor.Down);
        
        Assert.Equal(upRightNeighbor, cell.UpRight);
        Assert.Equal(cell, upRightNeighbor.DownLeft);
        
        Assert.Equal(rightNeighbor, cell.Right);
        Assert.Equal(cell, rightNeighbor.Left);
        
        Assert.Equal(downRightNeighbor, cell.DownRight);
        Assert.Equal(cell, downRightNeighbor.UpLeft);
        
        Assert.Equal(downNeighbor, cell.Down);
        Assert.Equal(cell, downNeighbor.Up);
        
        Assert.Equal(downLeftNeighbor, cell.DownLeft);
        Assert.Equal(cell, downLeftNeighbor.UpRight);
        
        Assert.Equal(leftNeighbor, cell.Left);
        Assert.Equal(cell, leftNeighbor.Right);
        
        Assert.Equal(upLeftNeighbor, cell.UpLeft);
        Assert.Equal(cell, upLeftNeighbor.DownRight);
    }
    
    [Fact]
    public void NotConnectBack_WhenConnectingToAnotherCell_GivenNotBothWays()
    {
        Cell2D cell = new();
        Cell2D upNeighbor = new();
        Cell2D rightNeighbor = new();
        Cell2D downNeighbor = new();
        Cell2D leftNeighbor = new();

        cell.ConnectUp(upNeighbor, false);
        cell.ConnectRight(rightNeighbor, false);
        cell.ConnectDown(downNeighbor, false);
        cell.ConnectLeft(leftNeighbor, false);

        Assert.Equal(upNeighbor, cell.Up);
        Assert.Equal(rightNeighbor, cell.Right);
        Assert.Equal(downNeighbor, cell.Down);
        Assert.Equal(leftNeighbor, cell.Left);
        Assert.Null(upNeighbor.Down);
        Assert.Null(rightNeighbor.Left);
        Assert.Null(downNeighbor.Up);
        Assert.Null(leftNeighbor.Right);
    }
}