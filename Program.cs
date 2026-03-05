using System;

class SimpleTetris
{
    const int W = 10;
    const int H = 15;
    static char[,] field = new char[H, W];

    static int x = W / 2 - 1;
    static int y = 0;
    static int tick = 0;
    static int score = 0;
    static Random r = new Random();

    static void Main()
    {
        Console.CursorVisible = false;

        while (true)
        {
            Console.Clear();
            DrawField();
            DrawPiece();

            if (Console.KeyAvailable)
            {
                var k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.LeftArrow && !Collision(x - 1, y)) x--;
                if (k == ConsoleKey.RightArrow && !Collision(x + 1, y)) x++;
                if (k == ConsoleKey.DownArrow) DropOne();
                if (k == ConsoleKey.Escape) break;
            }

            tick++;
            if (tick % 12 == 0) DropOne();

            if (Collision(x, y + 1))
            {
                PlacePiece();
                ClearLines();
                x = W / 2 - 1;
                y = 0;

                if (Collision(x, y))
                {
                    Console.Clear();
                    Console.WriteLine("\n\n   ИГРА ОКОНЧЕНА");
                    Console.WriteLine($"   Очки: {score}");
                    Console.ReadKey(true);
                    break;
                }
            }

            System.Threading.Thread.Sleep(50);
        }
    }

    static bool Collision(int cx, int cy)
    {
        for (int py = 0; py < 4; py++)
        {
            for (int px = 0; px < 4; px++)
            {
                if (GetBlock(px, py) == ' ')
                    continue;

                int fx = cx + px;
                int fy = cy + py;

                if (fx < 0 || fx >= W || fy >= H)
                    return true;

                if (fy >= 0 && field[fy, fx] != ' ')
                    return true;
            }
        }
        return false;
    }

    static void DropOne()
    {
        if (!Collision(x, y + 1))
            y++;
    }

    static void PlacePiece()
    {
        for (int py = 0; py < 4; py++)
            for (int px = 0; px < 4; px++)
            {
                if (GetBlock(px, py) != ' ')
                {
                    int fy = y + py;
                    int fx = x + px;
                    if (fy >= 0 && fy < H)
                        field[fy, fx] = '#';
                }
            }
    }

    static char GetBlock(int px, int py)
    {
        if (py == 1 && px >= 0 && px < 3)
                return '#';
        return ' ';
    }

    static void ClearLines()
    {
        for (int row = H - 1; row >= 0; row--)
        {
            bool full = true;
            for (int col = 0; col < W; col++)
            {
                if (field[row, col] == ' ')
                {
                    full = false;
                    break;
                }
            }

            if (full)
            {
                score += 10;
                for (int y = row; y > 0; y--)
                    for (int x = 0; x < W; x++)
                        field[y, x] = field[y - 1, x];

                for (int x = 0; x < W; x++)
                    field[0, x] = ' ';

                row++; 
            }
        }
    }

    static void DrawField()
    {
        for (int row = 0; row < H; row++)
        {
            Console.Write("|");
            for (int col = 0; col < W; col++)
            {
                Console.Write(field[row, col] == ' ' ? "  " : "##");
            }
            Console.WriteLine("|");
        }
        Console.WriteLine("+" + new string('-', W * 2) + "+");
        Console.WriteLine($" Очки: {score}");
        Console.WriteLine(" Управление ");
        Console.WriteLine(" стрелка влево ");
        Console.WriteLine(" стрелка вниз ");
        Console.WriteLine(" стрелка вверх ");
        Console.WriteLine(" стрелка вправа ");
        Console.WriteLine(" esc - выход ");
    }

    static void DrawPiece()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        for (int py = 0; py < 4; py++)
        {
            for (int px = 0; px < 4; px++)
            {
                if (GetBlock(px, py) != ' ')
                {
                    int sx = x + px;
                    int sy = y + py;
                    if (sy >= 0 && sy < H && sx >= 0 && sx < W)
                    {
                        Console.SetCursorPosition(sx * 2 + 1, sy);
                        Console.Write("##");
                    }
                }
            }
        }
        Console.ResetColor();
    }
}