namespace Connect_4;

class Program
{
    
    public static void Main()
    {

        Console.WriteLine("WELCOME TO CONNECT 4 !");

        GameBoard gameBoard = new GameBoard(6, 7);

        Game game = new Game(gameBoard);

        game.startGame();

    }

}