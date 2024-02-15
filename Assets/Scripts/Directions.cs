using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ocho direcciones
public enum Direction
{
    None,
    Up,
    UpRight,
    Right,
    RightDown,
    Down,
    DownLeft,
    Left,
    LeftUp,
}

public enum Axis
{
    None,
    Horizontal,
    Vertical,
    DiagonalDR,
    DiagonalUR
}

public static class Directions
{
    public static Direction[] directions4 = { Direction.Up, Direction.Right, Direction.Down, Direction.Left };

    public static float diagonal = Mathf.Sqrt(2)/2;

    public static Direction GetRandom4Direction()
    {
        return directions4[Random.Range(0, 4)];
    }

    public static Axis GetOppositeAxis(Axis a)
    {
        switch(a)
        {
            case Axis.DiagonalDR:
                return Axis.DiagonalUR;

            case Axis.Horizontal:
                return Axis.Vertical;

            case Axis.Vertical:
                return Axis.Horizontal;

            case Axis.DiagonalUR:
                return Axis.DiagonalDR;

            default:
                return Axis.None;
        }
    }

    public static Axis Direction2Axis(Direction d)
    {
        Axis a = Axis.None;

        switch(d)
        {
            case Direction.Down:
            case Direction.Up:
                a = Axis.Vertical;
                break;

            case Direction.Right:
            case Direction.Left:
                a = Axis.Horizontal;
                break;

            case Direction.DownLeft:
            case Direction.UpRight:
                a = Axis.DiagonalUR;
                break;

            case Direction.RightDown:
            case Direction.LeftUp:
                a = Axis.DiagonalDR;
                break;
        }

        return a;
    }

    public static Direction Vector2Direction(Vector2 d)
    {
    if (d.x == 0 && d.y == 0)
        {
            return Direction.None;
        }

        float angle;
        if (d.x < 0)
        {
            angle = 360 - Mathf.Atan2(d.x, d.y) * Mathf.Rad2Deg * -1;
        }
        else
        {
            angle = Mathf.Atan2(d.x, d.y) * Mathf.Rad2Deg;
        }

        if (337.5 <= angle || angle < 22.5)
        {
            return Direction.Up;
        }

        if (22.5 <= angle && angle < 67.5)
        {
            return Direction.UpRight;
        }

        if (67.5 <= angle && angle < 112.5)
        {
            return Direction.Right;
        }

        if (112.5 <= angle && angle < 157.5)
        {
            return Direction.RightDown;
        }

        if (157.5 <= angle && angle < 202.5)
        {
            return Direction.Down;
        }

        if (202.5 <= angle && angle < 247.5)
        {
            return Direction.DownLeft;
        }

        if (247.5 <= angle && angle < 292.5)
        {
            return Direction.Left;
        }

        if (292.5 <= angle && angle < 337.5)
        {
            return Direction.LeftUp;
        }

        return Direction.None;
    }

    public static Direction GetOppositeDirection(Direction d)
    {
        switch (d)
        {
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.RightDown: return Direction.LeftUp;
            case Direction.Right: return Direction.Left;
            case Direction.UpRight: return Direction.DownLeft;
            case Direction.DownLeft: return Direction.UpRight;
            case Direction.Up: return Direction.Down;
            case Direction.LeftUp: return Direction.RightDown;
            default: return Direction.None;
        }
    }

    public static Vector2Int Direction2VectorI(Direction d)
    {
        Vector2Int v = Vector2Int.zero;

        switch (d)
        {
            case Direction.Up:
                v.y = 1;
                break;

            case Direction.Right:
                v.x = 1;
                break;
            case Direction.Down:
                v.y = -1;
                break;
            case Direction.Left:
                v.x = -1;
                break;
        }

        return v;
    }

    public static Vector2 Direction2Vector(Direction d)
    {

        Vector2 dir = new Vector2(0f, 0f);

        switch (d)
        {
            case Direction.Up:
                dir.y = 1;
                break;

            case Direction.UpRight:
                dir.x = diagonal;
                dir.y = diagonal;
                break;

            case Direction.Right:
                dir.x = 1;
                break;

            case Direction.RightDown:
                dir.x = diagonal;
                dir.y = -diagonal;
                break;

            case Direction.Down:
                dir.y = -1f;
                break;

            case Direction.DownLeft:
                dir.x = -diagonal;
                dir.y = -diagonal;
                break;

            case Direction.Left:
                dir.x = -1f;
                break;

            case Direction.LeftUp:
                dir.x = -diagonal;
                dir.y = diagonal;
                break;
        }

        return dir;
    }
}
