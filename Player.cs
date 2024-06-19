namespace Connect_4;

public class Player
{
    public string Name { get; private set; }
    public int Value { get; private set; }
    public ConsoleColor Color { get; set; }


    public Player(string _name, ConsoleColor _color, int _value)
    {
        Name = _name;
        Color = _color;
        Value = _value;
    }

    public void introducePlayer()
    {
        Console.ForegroundColor = this.Color;
        Console.Write(this.Name);
        Console.ResetColor();
    }
}
