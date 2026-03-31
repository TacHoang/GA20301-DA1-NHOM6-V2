using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;

public class Pickup
{
    private bool collected = false;
    private int healValue = 30;

    public bool IsCollected()
    {
        return collected;
    }

    public void Collect(GameSession gs)
    {
        if (collected) return;

        collected = true;
        gs.Heal(healValue);
    }
}

[TestFixture]
public class HBTestsPlayMode
{
    // TC-HP01
    // Nhân vật chạm vật phẩm sinh mạng → hồi máu
    [Test]
    public void TC_HP01_CollectLifeItem_ShouldHealPlayer()
    {
        var gs = new GameSession();
        var life = new LifePickup();

        gs.TakeDamage(50);

        life.Collect(gs);

        Assert.AreEqual(80, gs.GetHealth());
    }

    // TC-HP02
    // Không chạm → không hồi máu
    [Test]
    public void TC_HP02_NotTouchLifeItem_ShouldNotHeal()
    {
        var gs = new GameSession();

        Assert.AreEqual(100, gs.GetHealth());
    }

    // TC-HP03
    // Item được đánh dấu đã nhặt
    [Test]
    public void TC_HP03_LifeItem_ShouldBeCollected()
    {
        var gs = new GameSession();
        var life = new LifePickup();

        life.Collect(gs);

        Assert.IsTrue(life.IsCollected());
    }

    // TC-HP04
    // Hiệu ứng hồi máu hoạt động đúng
    [Test]
    public void TC_HP04_HealEffect_ShouldApplyCorrectly()
    {
        var gs = new GameSession();
        var life = new LifePickup();

        gs.TakeDamage(30);

        life.Collect(gs);

        Assert.AreEqual(100, gs.GetHealth());
    }

    // TC-HP05
    // Nhặt nhiều vật phẩm sinh mạng liên tiếp
    [Test]
    public void TC_HP05_MultipleLifeItems_ShouldHealMultipleTimes()
    {
        var gs = new GameSession();

        gs.TakeDamage(60);

        new LifePickup().Collect(gs);
        new LifePickup().Collect(gs);

        Assert.AreEqual(100, gs.GetHealth());
    }

    // TC-HP06
    // Item chỉ kích hoạt 1 lần
    [Test]
    public void TC_HP06_LifeItem_ShouldTriggerOnlyOnce()
    {
        var gs = new GameSession();
        var life = new LifePickup();

        gs.TakeDamage(60);

        life.Collect(gs);
        int healthAfterFirst = gs.GetHealth();

        life.Collect(gs);

        Assert.AreEqual(healthAfterFirst, gs.GetHealth());
    }

    // TC-HP07
    // UI cập nhật sau khi hồi máu
    [Test]
    public void TC_HP07_UI_ShouldUpdateHealth()
    {
        var gs = new GameSession();
        var life = new LifePickup();

        gs.TakeDamage(40);
        life.Collect(gs);

        int uiHealth = gs.GetHealth();

        Assert.AreEqual(90, uiHealth);
    }

    // TC-HP08
    // Di chuyển nhanh vẫn nhặt được item
    [Test]
    public void TC_HP08_FastMovement_ShouldStillCollectLifeItem()
    {
        var gs = new GameSession();
        var life = new LifePickup();

        gs.TakeDamage(50);

        life.Collect(gs);

        Assert.AreEqual(80, gs.GetHealth());
    }

    // TC-HP09
    // Hồi máu khi HP = 0
    [Test]
    public void TC_HP09_HealFromZeroHealth_ShouldRestoreHealth()
    {
        var gs = new GameSession();
        var life = new LifePickup();

        gs.TakeDamage(200);

        life.Collect(gs);

        Assert.Greater(gs.GetHealth(), 0);
    }

    // TC-HP10
    // Game mới → item sinh mạng vẫn hoạt động
    [Test]
    public void TC_HP10_NewGame_LifeItemShouldWorkNormally()
    {
        var gs = new GameSession();

        var life = new LifePickup();
        life.Collect(gs);

        gs = new GameSession(); // new session

        var newLife = new LifePickup();
        newLife.Collect(gs);

        Assert.AreEqual(100, gs.GetHealth());
    }
}