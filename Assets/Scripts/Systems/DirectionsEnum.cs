using UnityEngine;

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

    public static Vector2 ToDirectionVector(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.None => Vector2.zero,
            _ => Vector2.zero
        };
    }
}