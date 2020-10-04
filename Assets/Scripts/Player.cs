public class Player
{
    public string Name { get; set; }
    public PlayerType PlayerType { get; set; }
    public CellState CellState { get; set; }

    public Player(string name, PlayerType playerType, CellState cellState)
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
