public enum Direction {Up, Down, Left, Right, None};

public static class DirectionExtensions
{
    public static Direction Flip(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.None => Direction.None,
            _ => direction
        };
    }
}