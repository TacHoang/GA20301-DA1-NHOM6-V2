using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;

public class GameSession
{
    private int score = 0;

    private int maxHealth = 100;
    private int health = 100;

    // ===== SCORE =====

    public void AddToScore(int amount)
    {
        if (amount <= 0) return;

        score += amount;
    }

    public int GetScore()
    {
        return score;
    }

    // ===== HEALTH =====

    public int GetHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        health -= damage;

        if (health < 0)
            health = 0;
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        health += amount;

        if (health > maxHealth)
            health = maxHealth;
    }
}

// ======================================================
// GAME SESSION TESTS
// ======================================================
[TestFixture]
public class GameSessionTestsPlayMode
{
    [Test]
    public void GetScore_InitialValue_ShouldBeZero()
    {
        var gs = new GameSession();
        Assert.AreEqual(0, gs.GetScore());
    }

    [Test]
    public void AddToScore_PositiveAmount_ShouldIncreaseScore()
    {
        var gs = new GameSession();
        gs.AddToScore(100);
        Assert.AreEqual(100, gs.GetScore());
    }

    [Test]
    public void AddToScore_MultipleAmounts_ShouldAccumulate()
    {
        var gs = new GameSession();
        gs.AddToScore(50);
        gs.AddToScore(75);
        gs.AddToScore(25);
        Assert.AreEqual(150, gs.GetScore());
    }

    [Test]
    public void AddToScore_NegativeAmount_ShouldNotChange()
    {
        var gs = new GameSession();
        gs.AddToScore(100);
        gs.AddToScore(-50);
        Assert.AreEqual(100, gs.GetScore());
    }

    [Test]
    public void AddToScore_ZeroAmount_ShouldNotChange()
    {
        var gs = new GameSession();
        gs.AddToScore(100);
        gs.AddToScore(0);
        Assert.AreEqual(100, gs.GetScore());
    }

    [Test]
    public void GetHealth_InitialValue_ShouldBe100()
    {
        var gs = new GameSession();
        Assert.AreEqual(100, gs.GetHealth());
    }

    [Test]
    public void TakeDamage_PositiveAmount_ShouldDecreaseHealth()
    {
        var gs = new GameSession();
        gs.TakeDamage(30);
        Assert.AreEqual(70, gs.GetHealth());
    }

    [Test]
    public void TakeDamage_ExceedsHealth_ShouldNotGoNegative()
    {
        var gs = new GameSession();
        gs.TakeDamage(150);
        Assert.AreEqual(0, gs.GetHealth());
    }

    [Test]
    public void TakeDamage_NegativeAmount_ShouldNotChange()
    {
        var gs = new GameSession();
        gs.TakeDamage(-50);
        Assert.AreEqual(100, gs.GetHealth());
    }

    [Test]
    public void TakeDamage_ZeroAmount_ShouldNotChange()
    {
        var gs = new GameSession();
        gs.TakeDamage(0);
        Assert.AreEqual(100, gs.GetHealth());
    }

    [Test]
    public void TakeDamage_MultipleTimes_ShouldAccumulate()
    {
        var gs = new GameSession();
        gs.TakeDamage(20);
        gs.TakeDamage(30);
        gs.TakeDamage(10);
        Assert.AreEqual(40, gs.GetHealth());
    }

    [Test]
    public void Heal_PositiveAmount_ShouldIncreaseHealth()
    {
        var gs = new GameSession();
        gs.TakeDamage(50);
        gs.Heal(30);
        Assert.AreEqual(80, gs.GetHealth());
    }

    [Test]
    public void Heal_ExceedsMaxHealth_ShouldNotExceedMax()
    {
        var gs = new GameSession();
        gs.TakeDamage(50);
        gs.Heal(100);
        Assert.AreEqual(100, gs.GetHealth());
    }

    [Test]
    public void Heal_NegativeAmount_ShouldNotChange()
    {
        var gs = new GameSession();
        gs.TakeDamage(50);
        gs.Heal(-20);
        Assert.AreEqual(50, gs.GetHealth());
    }

    [Test]
    public void Heal_ZeroAmount_ShouldNotChange()
    {
        var gs = new GameSession();
        gs.TakeDamage(50);
        gs.Heal(0);
        Assert.AreEqual(50, gs.GetHealth());
    }

    [Test]
    public void Heal_ToFull_ShouldRestoreAllHealth()
    {
        var gs = new GameSession();
        gs.TakeDamage(100);
        Assert.AreEqual(0, gs.GetHealth());
        gs.Heal(100);
        Assert.AreEqual(100, gs.GetHealth());
    }
}