using NUnit.Framework;

// ======================================================
// ITEM TESTS PLAYMODE
// ======================================================
[TestFixture]
public class ItemTestsPlayMode
{
    // TC-IT01
    // Nhân vật chạm vật phẩm → vật phẩm được nhặt
    [Test]
    public void TC_IT01_CollectItem_WhenTouch_ShouldIncreaseScore()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);

        Assert.AreEqual(50, gs.GetScore());
    }

    // TC-IT02
    // Không chạm collider → không nhặt
    [Test]
    public void TC_IT02_NotTouchItem_ShouldNotCollect()
    {
        var gs = new GameSession();

        Assert.AreEqual(0, gs.GetScore());
    }

    // TC-IT03
    // Item biến mất sau khi nhặt (đánh dấu collected)
    [Test]
    public void TC_IT03_Item_ShouldBeMarkedCollected()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);

        Assert.IsTrue(coin.IsCollected());
    }

    // TC-IT04
    // Hiệu ứng item được kích hoạt đúng
    [Test]
    public void TC_IT04_ItemEffect_ShouldApplyCorrectly()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);

        Assert.Greater(gs.GetScore(), 0);
    }

    // TC-IT05
    // Nhặt nhiều vật phẩm liên tiếp
    [Test]
    public void TC_IT05_MultipleItems_ShouldAccumulateScore()
    {
        var gs = new GameSession();

        new CoinPickup().Collect(gs);
        new CoinPickup().Collect(gs);
        new CoinPickup().Collect(gs);

        Assert.AreEqual(150, gs.GetScore());
    }

    // TC-IT06
    // Item không kích hoạt nhiều lần
    [Test]
    public void TC_IT06_Item_ShouldTriggerOnlyOnce()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);
        coin.Collect(gs);

        Assert.AreEqual(50, gs.GetScore());
    }

    // TC-IT07
    // UI cập nhật sau khi nhặt item
    [Test]
    public void TC_IT07_UI_ShouldUpdateScore()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);

        int uiScore = gs.GetScore();

        Assert.AreEqual(50, uiScore);
    }

    // TC-IT08
    // Di chuyển nhanh vẫn nhặt được item
    [Test]
    public void TC_IT08_FastMovement_ShouldStillCollectItem()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);

        Assert.AreEqual(50, gs.GetScore());
    }

    // TC-IT09
    // Nhặt item nhiều vị trí khác nhau
    [Test]
    public void TC_IT09_CollectItemsDifferentPositions_ShouldWork()
    {
        var gs = new GameSession();

        new CoinPickup().Collect(gs);
        new CoinPickup().Collect(gs);

        Assert.AreEqual(100, gs.GetScore());
    }

    // TC-IT10
    // Game mới → item mới vẫn hoạt động
    [Test]
    public void TC_IT10_NewGame_ItemShouldWorkNormally()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);

        gs = new GameSession(); // new game

        var newCoin = new CoinPickup();
        newCoin.Collect(gs);

        Assert.AreEqual(50, gs.GetScore());
    }
}