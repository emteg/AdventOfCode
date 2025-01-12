namespace Common.Tests;

public class Cell2DShould
{
    [Fact]
    public void HaveAllItsFourNeighborsSetToNull_WhenCreating()
    {
        Cell2D<byte> cell = new();
        
        Assert.Null(cell.Up);
        Assert.Null(cell.Right);
        Assert.Null(cell.Down);
        Assert.Null(cell.Left);
    }

    [Fact]
    public void HaveTheCorrectNeighbor_AndConnectBothWays_WhenConnectingToAnotherCell()
    {
        Cell2D<byte> cell = new();
        Cell2D<byte> upNeighbor = new();
        Cell2D<byte> rightNeighbor = new();
        Cell2D<byte> downNeighbor = new();
        Cell2D<byte> leftNeighbor = new();

        cell.ConnectUp(upNeighbor);
        cell.ConnectRight(rightNeighbor);
        cell.ConnectDown(downNeighbor);
        cell.ConnectLeft(leftNeighbor);

        Assert.Equal(upNeighbor, cell.Up);
        Assert.Equal(cell, upNeighbor.Down);
        Assert.Equal(rightNeighbor, cell.Right);
        Assert.Equal(cell, rightNeighbor.Left);
        Assert.Equal(downNeighbor, cell.Down);
        Assert.Equal(cell, downNeighbor.Up);
        Assert.Equal(leftNeighbor, cell.Left);
        Assert.Equal(cell, leftNeighbor.Right);
    }
    
    [Fact]
    public void NotConnectBack_WhenConnectingToAnotherCell_GivenNotBothWays()
    {
        Cell2D<byte> cell = new();
        Cell2D<byte> upNeighbor = new();
        Cell2D<byte> rightNeighbor = new();
        Cell2D<byte> downNeighbor = new();
        Cell2D<byte> leftNeighbor = new();

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