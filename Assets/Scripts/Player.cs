public class Player
{
    public string Name { get; set; }
    public PlayerType PlayerType { get; set; }
    public State CellState { get; set; }

    public Player(string name, PlayerType playerType, State cellState)
    {
        Name = name;
        PlayerType = playerType;
        CellState = cellState;
    }

    public override string ToString()
    {
        return $"{Name} [{CellState}] ";
    }
}
