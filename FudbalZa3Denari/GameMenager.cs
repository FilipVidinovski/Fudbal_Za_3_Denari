using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

public class GameManager
{
    private const float FieldLeft = 140f;
    private const float FieldTop = 110f;
    private const float FieldRight = 1140f;
    private const float FieldBottom = 610f;
    private const float ScoringDistance = 100f;
    private const float CollisionMargin = 10f;

    private Coin coin1;
    private Coin coin2;
    private Coin coin3;

    private List<Coin> coins = new List<Coin>();
    private List<PictureBox> obsticles = new List<PictureBox>();

    public enum TurnState
    {
        PlayerOne,
        PlayerTwo,
        GameOver
    }

    public TurnState CurrentTurn { get; private set; }

    private int flicksRemaining;
    private int flicksMade;

    private Coin movingCoin;
    private bool moveInProgress;

    private bool firstTurn = true;
    private bool invalidShot;
    private bool turnWasForcedToEnd;

    private HashSet<Coin> coinsHitThisMove = new HashSet<Coin>();

    private int playerOneScore;
    private int playerTwoScore;

    private bool scoreAlreadyAwarded;

    public GameManager(Coin coin1, Coin coin2, Coin coin3, List<PictureBox> obsticles)
    {
        this.coin1 = coin1;
        this.coin2 = coin2;
        this.coin3 = coin3;

        coins.Add(coin1);
        coins.Add(coin2);
        coins.Add(coin3);

        if (obsticles != null)
        {
            this.obsticles = obsticles;
        }

        CurrentTurn = TurnState.PlayerOne;
        flicksRemaining = 2;
        flicksMade = 0;
    }


    public bool GameOver
    {
        get { return CurrentTurn == TurnState.GameOver; }
    }

    public int PlayerOneScore
    {
        get { return playerOneScore; }
    }

    public int PlayerTwoScore
    {
        get { return playerTwoScore; }
    }

    public int FlicksRemaining
    {
        get
        { return flicksRemaining; }
    }

    public int FlicksMade
    {
        get { return flicksMade; }
    }


    public bool AnyCoinMoving()
    {
        foreach (Coin coin in coins)
        {
            if (coin != null && coin.IsMoving())
            {
                return true;
            }
        }

        return false;
    }

    public void HandleMouseClick(MouseButtons button, Point mousePosition)
    {
        if (button == MouseButtons.Right)
        {
            coin1.CancelAiming();
            coin2.CancelAiming();
            coin3.CancelAiming();

            return;
        }

        if (button != MouseButtons.Left)
        { 
            return; 
        }

        if (GameOver)
        { 
            return; 
        }

        if (AnyCoinMoving())
        { 
            return; 
        }

        if (flicksRemaining <= 0)
        { 
            return; 
        }

        Coin aimingCoin = null;

        foreach (Coin coin in coins)
        {
            if (coin.IsAiming)
            {
                aimingCoin = coin;
                break;
            }
        }

        if (aimingCoin != null)
        {
            aimingCoin.FlingTowards(mousePosition);
            BeginFlick(aimingCoin, mousePosition);

            return;
        }

        foreach (Coin coin in coins)
        {
            if (coin.MouseOver(mousePosition))
            {
                coin.StartAiming();
                break;
            }
        }
    }



    private void BeginFlick(Coin coin, Point target)
    {
        movingCoin = coin;
        moveInProgress = true;
        turnWasForcedToEnd = false;

        coinsHitThisMove.Clear();

        flicksMade++;
        flicksRemaining--;

        bool mustPassBetween = true;

        if (firstTurn && flicksMade == 1)
        {
            mustPassBetween = false;
        }

        if (mustPassBetween)
        {
            invalidShot =!ShotPassesBetweenOtherCoins(coin, target);
        }
        else
        {
            invalidShot = false;
        }

        firstTurn = false;
    }

    public void Update()
    {
        if (GameOver)
        {
            return;
        }

        foreach (Coin coin in coins)
        {
            coin.Update();
        }

        foreach (Coin coin in coins)
        {
            foreach (PictureBox obstacle in obsticles)
            {
                CheckCoinBoxCollision(coin, obstacle.Bounds);
            }
        }

        for (int i = 0; i < coins.Count; i++)
        {
            for (int j = i + 1; j < coins.Count; j++)
            {
                CheckCoinCollision(coins[i], coins[j]);
            }
        }

        if (moveInProgress && !AnyCoinMoving())
        {
            FinishFlick();
            CheckForScore();
        }
    }


    private void FinishFlick()
    {
        moveInProgress = false;
        movingCoin = null;

        if (GameOver)
        {
            return;
        }

        if (turnWasForcedToEnd)
        {
            turnWasForcedToEnd = false;
            invalidShot = false;

            EndTurn();

            return;
        }

        if (invalidShot)
        {
            invalidShot = false;

            EndTurn();

            return;
        }

        invalidShot = false;

        if (CurrentTurn == TurnState.PlayerOne)
        {
            if (flicksRemaining == 0)
            {
                StartPlayerTwo();
            }

            return;
        }

        if (CurrentTurn == TurnState.PlayerTwo)
        {
            if (flicksRemaining == 0)
            {
                StartPlayerOne();
            }
        }
    }

    private void EndTurn()
    {
        coin1.CancelAiming();
        coin2.CancelAiming();
        coin3.CancelAiming();

        if (CurrentTurn == TurnState.PlayerOne)
        {
            StartPlayerTwo();
        }
        else
        {
            StartPlayerOne();
        }
    }

    private void StartPlayerTwo()
    {
        CurrentTurn = TurnState.PlayerTwo;

        flicksRemaining = 3;
        flicksMade = 0;

        movingCoin = null;
        moveInProgress = false;
        invalidShot = false;
        turnWasForcedToEnd = false;

        coinsHitThisMove.Clear();
    }

    private void StartPlayerOne()
    {
        CurrentTurn = TurnState.PlayerOne;

        flicksRemaining = 3;
        flicksMade = 0;

        movingCoin = null;
        moveInProgress = false;
        invalidShot = false;
        turnWasForcedToEnd = false;

        coinsHitThisMove.Clear();
    }

    private void CheckCoinCollision(Coin moving,Coin other)
    {
        Vector2 difference = other.Position - moving.Position;
        float distance = difference.Length();
        float collisionDistance = Coin.Radius * 2f;

        if (distance > collisionDistance)
        {
            return;
        }

        if (moveInProgress)
        {
            turnWasForcedToEnd = true;
        }

        float xDifference = Math.Abs(moving.Position.X - other.Position.X);
        float yDifference = Math.Abs(moving.Position.Y - other.Position.Y);

        if (yDifference <= CollisionMargin)
        {
            SwapHorizontalVelocity(moving,other);
        }
        else if (xDifference <= CollisionMargin)
        {
            SwapVerticalVelocity(moving,other);
        }
        else
        {
            SwapBothVelocity(moving,other);
        }

        if (distance > 0.001f)
        {
            Vector2 direction = difference / distance;
            float overlap = collisionDistance - distance;

            moving.SetPosition(moving.Position - direction * (overlap / 2f));
            other.SetPosition(other.Position + direction * (overlap / 2f));
        }
    }


    private void SwapHorizontalVelocity(Coin moving, Coin other)
    {
        float temp = moving.Velocity.X;

        moving.SetVelocity(new Vector2(other.Velocity.X, moving.Velocity.Y));

        other.SetVelocity(new Vector2(temp, other.Velocity.Y));
    }

    private void SwapVerticalVelocity(Coin moving,Coin other)
    {
        float temp = moving.Velocity.Y;

        moving.SetVelocity(new Vector2(moving.Velocity.X,other.Velocity.Y));

        other.SetVelocity(new Vector2(other.Velocity.X,temp));
    }

    private void SwapBothVelocity(Coin moving,Coin other)
    {
        Vector2 temp = moving.Velocity;

        moving.SetVelocity(other.Velocity);

        other.SetVelocity(temp);
    }

    private void CheckCoinBoxCollision(Coin coin,Rectangle box)
    {
        Rectangle coinRect = new Rectangle((int)(coin.Position.X - Coin.Radius), (int)(coin.Position.Y - Coin.Radius), (int)(Coin.Radius * 2), (int)(Coin.Radius * 2));

        if (!coinRect.IntersectsWith(box))
        {
            return;
        }

        float fromLeft = coinRect.Right - box.Left;
        float fromRight = box.Right - coinRect.Left;
        float fromTop = coinRect.Bottom - box.Top;
        float fromBottom = box.Bottom - coinRect.Top;

        float smallest = Math.Min(Math.Min(fromLeft, fromRight),Math.Min(fromTop, fromBottom));

        if (smallest == fromLeft)
        {
            coin.SetVelocity(new Vector2(-Math.Abs(coin.Velocity.X), coin.Velocity.Y));
        }
        else if (smallest == fromRight)
        {
            coin.SetVelocity(new Vector2(Math.Abs(coin.Velocity.X), coin.Velocity.Y));
        }
        else if (smallest == fromTop)
        {
            coin.SetVelocity(new Vector2(coin.Velocity.X, -Math.Abs(coin.Velocity.Y)));
        }
        else
        {
            coin.SetVelocity(new Vector2(coin.Velocity.X, Math.Abs(coin.Velocity.Y)));
        }
    }

    private bool ShotPassesBetweenOtherCoins(Coin shotCoin,Point target)
    {
        List<Coin> otherCoins = new List<Coin>();

        foreach (Coin coin in coins)
        {
            if (coin != shotCoin)
            {
                otherCoins.Add(coin);
            }
        }

        if (otherCoins.Count != 2)
        {
            return false;
        }

        Vector2 start = shotCoin.Position;
        Vector2 end = new Vector2(target.X,target.Y);
        Vector2 shotDirection = end - start;

        if (shotDirection.LengthSquared() < 0.0001f)
        {
            return false;
        }

        shotDirection = Vector2.Normalize(shotDirection);

        Vector2 perpendicular = new Vector2(-shotDirection.Y,shotDirection.X);

        Vector2 relativeA = otherCoins[0].Position - start;
        Vector2 relativeB = otherCoins[1].Position - start;

        float sideA = Vector2.Dot(relativeA,perpendicular);
        float sideB = Vector2.Dot(relativeB,perpendicular);

        if (sideA == 0f || sideB == 0f)
        {
            return false;
        }

        if ((sideA > 0 && sideB > 0) || (sideA < 0 && sideB < 0))
        {
            return false;
        }

        float projectionA = Vector2.Dot(relativeA,shotDirection);
        float projectionB = Vector2.Dot(relativeB,shotDirection);

        if (projectionA < 0 || projectionB < 0)
        {
            return false;
        }

        return true;
    }

    public void CheckForScore()
    {
        if (GameOver)
        {
            return;
        }

        if (scoreAlreadyAwarded)
        {
            return;
        }

        bool playerOneScores = true;

        foreach (Coin coin in coins)
        {
            if (coin.Position.Y < FieldBottom - ScoringDistance)
            {
                playerOneScores = false;
                break;
            }
        }

        bool playerTwoScores = true;

        foreach (Coin coin in coins)
        {
            if (coin.Position.Y > FieldTop + ScoringDistance)
            {
                playerTwoScores = false;
                break;
            }
        }

        if (playerOneScores)
        {
            playerOneScore++;

            scoreAlreadyAwarded = true;

            if (playerOneScore >= 3)
            {
                CurrentTurn = TurnState.GameOver;
                return;
            }

            ResetRound(TurnState.PlayerTwo);
            return;
        }

        if (playerTwoScores)
        {
            playerTwoScore++;

            scoreAlreadyAwarded = true;

            if (playerTwoScore >= 3)
            {
                CurrentTurn = TurnState.GameOver;
                return;
            }

            ResetRound(TurnState.PlayerOne);
        }
    }



    private void ResetRound(TurnState startingPlayer)
    {
        coin1.SetPosition(new Vector2(640f, 290f));
        coin2.SetPosition(new Vector2(640f, 350f));
        coin3.SetPosition(new Vector2(640f, 410f));

        coin1.SetVelocity(Vector2.Zero);
        coin2.SetVelocity(Vector2.Zero);
        coin3.SetVelocity(Vector2.Zero);

        CurrentTurn = startingPlayer;

        flicksRemaining = 3;
        flicksMade = 0;

        movingCoin = null;
        moveInProgress = false;

        firstTurn = true;
        invalidShot = false;
        turnWasForcedToEnd = false;

        coinsHitThisMove.Clear();

        scoreAlreadyAwarded = false;

        coin1.CancelAiming();
        coin2.CancelAiming();
        coin3.CancelAiming();
    }
    public void ResetGame()
    {
        playerOneScore = 0;
        playerTwoScore = 0;

        scoreAlreadyAwarded = false;

        ResetRound(TurnState.PlayerOne);
    }

}
