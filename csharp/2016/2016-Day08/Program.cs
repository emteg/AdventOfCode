namespace _2016_Day08;

public static class Program
{
    public static void Main()
    {
        Console.Clear();
        Console.OutputEncoding = System.Text.Encoding.UTF8; 
        DoubleBorderBox(5, 10, 50, 5, "Performing operation");

        for (uint i = 0; i < 100; i++)
        {
            ProgressBar(7, 12, 46, 100, i);
            Thread.Sleep(100);
        }
    }

    private static void DoubleBorderBox(ushort top, ushort left, ushort width, ushort height, string title)
    {
        // cut off title if width is not enough
        if (title.Length > width - 2)
            title = title[..(width - 2)];
        
        // how much border to draw between the corners and before/after the title
        int remainingWidth = width - title.Length - 2;
        int beforeTitle = remainingWidth / 2;
        int afterTitle = remainingWidth - beforeTitle;

        // top border with title
        Console.SetCursorPosition(left, top);
        Console.Write(DoubleBorderCornerTopLeft);
        Console.Write(new string(DoubleBorderHorizontal, beforeTitle));
        Console.Write(title);
        Console.Write(new string(DoubleBorderHorizontal, afterTitle));
        Console.Write(DoubleBorderCornerTopRight);
        
        // side borders
        for (int i = 0; i < height - 2; i++)
        {
            Console.SetCursorPosition(left, ++top);
            Console.WriteLine(DoubleBorderVertical);
            Console.SetCursorPosition(left + width - 1, top);
            Console.WriteLine(DoubleBorderVertical);
        }
        
        // bottom border
        Console.SetCursorPosition(left, ++top);
        Console.Write(DoubleBorderCornerBottomLeft);
        Console.Write(new string(DoubleBorderHorizontal, width - 2));
        Console.Write(DoubleBorderCornerBottomRight);
    }

    private static void ProgressBar(ushort top, ushort left, ushort width, uint max, uint position)
    {
        position = Math.Clamp(position, 0, max);
        
        double progress = position / (double)max;
        int filled = (int)Math.Round(progress * width);
        int open = width - filled;
        
        Console.SetCursorPosition(left, top);
        Console.Write(new string(BlockFull, filled));
        Console.Write(new string(BlockFullShadedLight, open));
        Console.SetCursorPosition(left + filled, top);
    }

    private const char DoubleBorderHorizontal = '\x2550';
    private const char DoubleBorderVertical = '\x2551';
    private const char DoubleBorderCornerTopLeft = '\x2554';
    private const char DoubleBorderCornerTopRight = '\x2557';
    private const char DoubleBorderCornerBottomLeft = '\x255A';
    private const char DoubleBorderCornerBottomRight = '\x255D';

    private const char BlockFull = '\x2588';
    private const char BlockFullShadedLight = '\x2591';
    private const char BlockFullShadedMedium = '\x2592';
    private const char BlockFullShadedDark = '\x2593';
}