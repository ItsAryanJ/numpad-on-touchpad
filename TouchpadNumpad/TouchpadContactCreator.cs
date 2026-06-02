internal class TouchpadContactCreator
{
    public int? ContactId { get; set; }

    public int? X { get; set; }

    public int? Y { get; set; }

    public int? MaxX { get; set; }

    public int? MaxY { get; set; }

    public byte EqualityDeltaPercent { get; set; }

    public bool TryCreate(out TouchpadContact? contact)
    {
        if (ContactId.HasValue && X.HasValue && Y.HasValue && MaxX.HasValue && MaxY.HasValue)
        {
            contact = new TouchpadContact(ContactId.Value, X.Value, Y.Value, MaxX.Value, MaxY.Value, EqualityDeltaPercent);
            return true;
        }
        contact = null;
        return false;
    }

    public void Clear()
    {
        ContactId = null;
        X = null;
        Y = null;
        MaxX = null;
        MaxY = null;
    }
}
