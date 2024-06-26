namespace Connect_4;

public class GameBoard
{
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    public int[,] Board { get; private set; }
    public int AvailableMoves { get; private set; }

    private Dictionary<string, ConsoleColor> activePlayers = [];

    const string EMPTY_CELL = " ";
    const string FILLED_CELL = "#";

    public GameBoard(int row, int column)
    {
        Rows = row;
        Columns = column;
        Board = new int[Rows, Columns];
        AvailableMoves = row * column;
    }

    private void showRemainingMoves()
    {
        Console.Write("  ");
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($" {AvailableMoves} ");
        Console.ResetColor();
        Console.WriteLine(" options remaining");
    }

    public void printBoard(string[] highlightedCells = null)
    {
        Console.Clear();
        showRemainingMoves();

        Console.WriteLine();
        string colHeader = "    ";
        for (int colHeaderNumber = 0; colHeaderNumber < Columns; colHeaderNumber++)
        {
            colHeader = $"{colHeader} {colHeaderNumber + 1} ";
        }
        Console.WriteLine(colHeader);

        for (int row = 0; row < Rows; row++)
        {
            bool lineNumberPrinted = false;
            for (int col = 0; col < Columns; col++)
            {
                if (!lineNumberPrinted)
                {
                    Console.Write(" {0} ", (row + 1).ToString().PadLeft(2));
                    lineNumberPrinted = true;
                }

                if (Board[row, col] == 0)
                {
                    printEmptyBoardCell();
                }
                else
                {
                    if (highlightedCells != null && highlightedCells.Contains($"{row}-{col}"))
                    {
                        printWinnerCells();
                    }
                    else
                    {
                        printFormattedBoardCell(Board[row, col].ToString());
                    }
                }

            }
            Console.WriteLine();
        }
    }

    private void printEmptyBoardCell()
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("[{0}]", EMPTY_CELL);
        Console.ResetColor();
    }

    private void printFormattedBoardCell(string boardValue)
    {
        Console.BackgroundColor = activePlayers[boardValue];
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("[{0}]", FILLED_CELL);
        Console.ResetColor();
    }

    private void printWinnerCells()
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("[{0}]", FILLED_CELL);
        Console.ResetColor();
    }

    public bool fillBoardCell(Player player, int column)
    {
        //Populate active players list when each player does the first move
        if (!activePlayers.ContainsKey(player.Value.ToString()))
        {
            activePlayers.Add(player.Value.ToString(), player.Color);
        }

        int selectedIndex = column - 1;
        int emptyRowIndex = getEmptyRowIndex(selectedIndex);

        if (emptyRowIndex != -1)
        {
            Board[emptyRowIndex, selectedIndex] = player.Value;
            AvailableMoves--;
            return true;
        }

        return false;
    }

    private int getEmptyRowIndex(int column)
    {
        int isEmpty = -1;
        for (int row = Rows - 1; row >= 0; row--)
        {

            //Console.Write("Board[{0}, {1}] = {2}", row, column, Board[row, column]);
            if (Board[row, column] == 0)
            {
                isEmpty = row;
                break;
            }
        }

        return isEmpty;
    }
    public void clearBoard()
    {
        Board = new int[Rows, Columns];
        AvailableMoves = Rows * Columns;
        Console.WriteLine("Gameboard reset!");
        printBoard();
    }
}
