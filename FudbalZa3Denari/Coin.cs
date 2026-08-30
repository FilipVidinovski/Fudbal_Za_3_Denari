using System;
using System.Drawing;
using System.Numerics;

public class Coin
{
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }

    public bool IsAiming { get; private set; }

    public const float Radius = 15f;

    public Image Image { get; private set; }

    private const float FieldLeft = 140f;
    private const float FieldTop = 110f;
    private const float FieldRight = 1140f;
    private const float FieldBottom = 610f;

    public Coin(float x, float y, Image image)
    {
        Position = new Vector2(x, y);
        Velocity = Vector2.Zero;
        Image = image;
    }

    public bool IsMoving()
    {
        return Velocity.Length() >= 0.05f;
    }

    public void Update()
    {
        Velocity *= 0.99f;

        if (Velocity.Length() < 0.05f)
        {
            Velocity = new Vector2(0, 0);
        }

        float nextX = Position.X + Velocity.X;
        float nextY = Position.Y + Velocity.Y;

        if (nextX - Radius < FieldLeft)
        {
            Velocity = new Vector2(-Velocity.X, Velocity.Y);
            nextX = FieldLeft + Radius;
        }

        if (nextX + Radius > FieldRight)
        {
            Velocity = new Vector2(-Velocity.X, Velocity.Y);
            nextX = FieldRight - Radius;
        }

        if (nextY - Radius < FieldTop)
        {
            Velocity = new Vector2(Velocity.X, -Velocity.Y);
            nextY = FieldTop + Radius;
        }

        if (nextY + Radius > FieldBottom)
        {
            Velocity = new Vector2(Velocity.X, -Velocity.Y);
            nextY = FieldBottom - Radius;
        }

        Position = new Vector2(nextX, nextY);
    }

    public void SetVelocity(Vector2 velocity)
    {
        Velocity = velocity;
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }

    public void Stop()
    {
        Velocity = Vector2.Zero;
    }

    public bool MouseOver(Point mousePosition)
    {
        float dx = mousePosition.X - Position.X;
        float dy = mousePosition.Y - Position.Y;

        float distanceSquared = dx * dx + dy * dy;

        return distanceSquared <= Radius * Radius;
    }

    public void StartAiming()
    {
        IsAiming = true;
    }

    public void FlingTowards(Point target)
    {
        Vector2 direction = new Vector2(target.X - Position.X,target.Y - Position.Y);
        float distance = direction.Length();

        if (distance < 0.1f)
        {
            IsAiming = false;
            return;
        }

        direction = Vector2.Normalize(direction);

        const float MaxFlingDistance = 80f;
        const float MaxFlingSpeed = 8f;

        float flingSpeed = (distance / MaxFlingDistance) * MaxFlingSpeed;

        if (flingSpeed > MaxFlingSpeed)
        {
            flingSpeed = MaxFlingSpeed;
        }

        Velocity = direction * flingSpeed;

        IsAiming = false;
    }

    public void CancelAiming()
    {
        IsAiming = false;
    }
}
