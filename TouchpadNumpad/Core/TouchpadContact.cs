using System;

public struct TouchpadContact : IEquatable<TouchpadContact>
{
    private int _equalityDeltaX;

    private int _equalityDeltaY;

    public int ContactId { get; }

    public int X { get; }

    public int Y { get; }

    public int MaxX { get; }

    public int MaxY { get; }

    public byte EqualityDeltaPercent { get; }

    public static bool operator ==(TouchpadContact a, TouchpadContact b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(TouchpadContact a, TouchpadContact b)
    {
        return !a.Equals(b);
    }

    public TouchpadContact(int contactId, int x, int y, int maxX, int maxY, byte equalityDeltaPercent)
    {
        ContactId = contactId;
        X = x;
        Y = y;
        MaxX = maxX;
        MaxY = maxY;
        EqualityDeltaPercent = equalityDeltaPercent;
        _equalityDeltaX = ((equalityDeltaPercent != 0) ? (MaxX * equalityDeltaPercent / 100) : 0);
        _equalityDeltaY = ((equalityDeltaPercent != 0) ? (MaxY * equalityDeltaPercent / 100) : 0);
    }

    public TouchpadContact ChangeEqualityDeltaPercent(byte equalityDeltaPercent)
    {
        return new TouchpadContact(ContactId, X, Y, MaxX, MaxY, equalityDeltaPercent);
    }

    public bool Equals(TouchpadContact other)
    {
        if (Math.Abs(X - other.X) <= _equalityDeltaX && Math.Abs(Y - other.Y) <= _equalityDeltaY && MaxX == other.MaxX)
        {
            return MaxY == other.MaxY;
        }
        return false;
    }

    public override bool Equals(object? obj)
    {
        if (obj is TouchpadContact other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (ContactId, X, Y, MaxX, MaxY).GetHashCode();
    }

    public override string ToString()
    {
        return $"ContactId: {ContactId} | X: {X}, Y: {Y}";
    }
}
