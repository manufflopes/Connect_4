namespace Connect_4;

public class Game
{
    private static List<Player> Players { get; set; } = new List<Player>();
    private GameBoard Gameboard { get; set; }
    public static int Turn { get; private set; }

    public Game(GameBoard _gameboard)
    {
        Gameboard = _gameboard;
    }

    private static void setupPlayers()
    {

        int playerNumber = 1;

        Dictionary<string, ConsoleColor> availableColors = new Dictionary<string, ConsoleColor>()
        {
            { "red", ConsoleColor.DarkRed },
            { "gray", ConsoleColor.Gray },
            { "blue", ConsoleColor.DarkBlue },
            { "green", ConsoleColor.DarkGreen },
            { "purple", ConsoleColor.DarkMagenta },
            { "pink", ConsoleColor.Magenta },
            { "cyan", ConsoleColor.Cyan }
        };

        List<string> colors = availableColors.Keys.ToList();
        string formattedColors = string.Join(",", colors.Take(colors.Count() - 1)).ToUpper() + " OR " + colors.Last().ToUpper();

        while (playerNumber < 3)
        {
            //GETTING NAME
            Console.WriteLine("\nInput player {0} name:", playerNumber);

            string playerName = Console.ReadLine();

            while (playerName.Equals(string.Empty) || playerName == null)
            {
                Console.WriteLine("Incorrect input, please enter a valid name!");
                playerName = Console.ReadLine();
            }

            Console.WriteLine("\n{0} - Select a color ({1})", playerName, formattedColors);

            string playerColor = Console.ReadLine().Trim().ToLower();

            while (!availableColors.ContainsKey(playerColor))
            {
                Console.WriteLine("\nIncorrect input, please enter a valid color: ({0})!", formattedColors);
                playerColor = Console.ReadLine().Trim().ToLower();
            }

            Player player = new Player(playerName, availableColors[playerColor], playerNumber);

            Players.Add(player);

            playerNumber++;
        }
    }

    public void startGame()
    {
        setupPlayers();

        Turn = 0;
        int gameTurn = 1;
        Gameboard.clearBoard();

        while (true)
        {
            Console.WriteLine();
            Players[Turn].introducePlayer();
            Console.WriteLine("'s turn");
            Console.WriteLine("\nSelect a valid column number OR 0 to finish the game");

            //GETTING PLAYER SELECTED COLUMN
            var userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int selectedColumn))
            {
                if (selectedColumn == 0)
                {
                    Console.WriteLine("Ending game!");
                    break;
                }

                if (checkValidMove(selectedColumn) && makeMove(selectedColumn))
                {
                    string[] hasAWinner = checkWin();

                    if (hasAWinner.Length > 0)
                    {

                        Gameboard.printBoard(hasAWinner);

                        Console.WriteLine("\n\n## Game Over ##");
                        Console.Write("\nThe winner is: ");
                        Players[Turn].introducePlayer();

                        Console.WriteLine();
                        Console.WriteLine("\n\nDo you want to play again? (Y/N)");

                        var playAgain = Console.ReadLine().Trim().ToLower();

                        while (!playAgain.Equals("y") && !playAgain.Equals("n"))
                        {
                            Console.WriteLine("\n\nDo you want to play again? (Y/N)");
                            playAgain = Console.ReadLine().Trim().ToLower();
                        }


                        if (playAgain.Equals("y"))
                        {
                            gameReset();
                            continue;
                        }
                        else if (playAgain.Equals("n"))
                        {
                            Console.WriteLine("Ending game!");
                            break;
                        }
                    }

                    if (isGameTie())
                    {
                        Console.WriteLine("\n\n## Game Over ##");
                        Console.Write("\nThis is a tie!\n");
                        Console.WriteLine("\n\nDo you want to play again? (Y/N)");

                        var playAgain = Console.ReadLine().Trim().ToLower();

                        if (playAgain.Equals("y"))
                        {
                            gameReset();
                        }
                        else
                        {
                            Console.WriteLine("Ending game!");
                            break;
                        }
                    }

                    endTurn();
                };

            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(userInput);
                Console.ResetColor();
                Console.WriteLine(" is not a number");
            }
        }
    }

    public bool isGameTie()
    {
        return Gameboard.AvailableMoves == 0;
    }

    public void endTurn()
    {
        Turn = (Turn == 1) ? 0 : 1;
    }

    // Check if the selected column is valid and return a message if it is not
    public bool checkValidMove(int column)
    {
        if (!(column >= 0 && column <= Gameboard.Columns))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Only values between 1 and {0} are accepted!", Gameboard.Columns);
            Console.ResetColor();
            return false;
        }

        return true;
    }
    // Make a move on the board by using the fillBoardCell method from GameBoard class where the validation is done
    public bool makeMove(int playedColumn)
    {
        bool isValidMove = Gameboard.fillBoardCell(Players[Turn], playedColumn);

        if (!isValidMove)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("This move is not valid! Try again with another column!");
            Console.ResetColor();
            return false;
        }

        Gameboard.printBoard();
        return true;
    }

    // Check if there is a winner and return the winning cells to highlight them
    public string[] checkWin()
    {
        string[] winner = [];
        //Check horizontal winner
        for (int row = 0; row < Gameboard.Rows; row++)
        {
            for (int col = 0; col < Gameboard.Columns - 3; col++)
            {
                if (Gameboard.Board[row, col] != 0 &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row, col + 1]) &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row, col + 2]) &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row, col + 3]))
                {
                    winner = [$"{row}-{col}", $"{row}-{col + 1}", $"{row}-{col + 2}", $"{row}-{col + 3}"];
                    return winner;
                }
            }
        }

        //Check vertical winner
        for (int row = 0; row < Gameboard.Rows - 3; row++)
        {
            for (int col = 0; col < Gameboard.Columns; col++)
            {
                if (Gameboard.Board[row, col] != 0 &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row + 1, col]) &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row + 2, col]) &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row + 3, col]))
                {
                    winner = [$"{row}-{col}", $"{row + 1}-{col}", $"{row + 2}-{col}", $"{row + 3}-{col}"];
                    return winner;
                }
            }
        }

        //Check diagonal (Top right to bottom left) winner
        for (int row = 0; row < Gameboard.Rows - 3; row++)
        {
            for (int col = 3; col < Gameboard.Columns; col++)
            {
                if (Gameboard.Board[row, col] != 0 &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row + 1, col - 1]) &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row + 2, col - 2]) &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row + 3, col - 3]))
                {
                    winner = [$"{row}-{col}", $"{row + 1}-{col - 1}", $"{row + 2}-{col - 2}", $"{row + 3}-{col - 3}"];
                    return winner;
                }
            }
        }

        //Check diagonal (Top left to bottom right) winner
        for (int row = 0; row < Gameboard.Rows - 3; row++)
        {
            for (int col = 0; col < Gameboard.Columns - 3; col++)
            {
                if (Gameboard.Board[row, col] != 0 &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row + 1, col + 1]) &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row + 2, col + 2]) &&
                    Gameboard.Board[row, col].Equals(Gameboard.Board[row + 3, col + 3]))
                {
                    winner = [$"{row}-{col}", $"{row + 1}-{col + 1}", $"{row + 2}-{col + 2}", $"{row + 3}-{col + 3}"];
                    return winner;
                }
            }
        }

        return winner;
    }

    public void gameReset()
    {
        Gameboard.clearBoard();
    }

    public void showGameOverMessage()
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.Write("\n\n  ");
        Console.Write("Game Over");
        Console.WriteLine("  ");
        Console.ResetColor();
        Console.Write("\nThe winner is: ");
        Players[Turn].introducePlayer();
    }
}
