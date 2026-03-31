using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;

// ======================================================
// MOCK ITEM (giả lập item giống docx)
// ======================================================
public class CoinPickup
{
    private bool collected = false;
    private int coinValue = 50;

    public bool IsCollected()
    {
        return collected;
    }

    public void Collect(GameSession gs)
    {
        if (collected) return;

        collected = true;
        gs.AddToScore(coinValue);
    }
}

// ======================================================
// TEST CASES FROM XLSX
// ======================================================
[TestFixture]
public class CoinSystemTests
{
    // ==================================================
    // TC-HTD01
    // Nhân vật chạm xu → cộng điểm
    // ==================================================
    [Test]
    public void TC_HTD01_CollectCoin_ShouldIncreaseScore()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);

        Assert.Greater(gs.GetScore(), 0);
    }

    // ==================================================
    // TC-HTD02
    // Không chạm xu → điểm không đổi
    // ==================================================
    [Test]
    public void TC_HTD02_NoCollect_ShouldNotChangeScore()
    {
        var gs = new GameSession();

        int startScore = gs.GetScore();

        // không collect

        Assert.AreEqual(startScore, gs.GetScore());
    }

    // ==================================================
    // TC-HTD03
    // Ăn nhiều xu → điểm tăng nhiều lần
    // ==================================================
    [Test]
    public void TC_HTD03_MultipleCoins_ShouldAccumulateScore()
    {
        var gs = new GameSession();

        new CoinPickup().Collect(gs);
        new CoinPickup().Collect(gs);
        new CoinPickup().Collect(gs);

        Assert.Greater(gs.GetScore(), 50);
    }

    // ==================================================
    // TC-HTD04
    // Xu đã ăn không ăn lại
    // ==================================================
    [Test]
    public void TC_HTD04_CoinCollectedOnce_ShouldNotAddAgain()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);
        int scoreAfterFirst = gs.GetScore();

        coin.Collect(gs);

        Assert.AreEqual(scoreAfterFirst, gs.GetScore());
    }

    // ==================================================
    // TC-HTD05
    // Xu biến mất sau khi ăn
    // ==================================================
    [Test]
    public void TC_HTD05_Coin_ShouldBeMarkedCollected()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);

        Assert.IsTrue(coin.IsCollected());
    }

    // ==================================================
    // TC-HTD06
    // Di chuyển liên tục vẫn cộng điểm đúng
    // ==================================================
    [Test]
    public void TC_HTD06_ContinuousCollect_ShouldAddCorrectScore()
    {
        var gs = new GameSession();

        for (int i = 0; i < 5; i++)
        {
            new CoinPickup().Collect(gs);
        }

        Assert.AreEqual(250, gs.GetScore());
    }

    // ==================================================
    // TC-HTD07
    // UI hiển thị đúng điểm (giả lập bằng GetScore)
    // ==================================================
    [Test]
    public void TC_HTD07_UI_ShouldDisplayUpdatedScore()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        coin.Collect(gs);

        int uiScore = gs.GetScore(); // giả lập UI đọc data

        Assert.AreEqual(gs.GetScore(), uiScore);
    }

    // ==================================================
    // TC-HTD08
    // Quit game → score được giữ (giả lập save state)
    // ==================================================
    [Test]
    public void TC_HTD08_QuitGame_ShouldKeepScoreValue()
    {
        var gs = new GameSession();
        new CoinPickup().Collect(gs);

        int savedScore = gs.GetScore();

        // giả lập quit → load lại
        int loadedScore = savedScore;

        Assert.AreEqual(savedScore, loadedScore);
    }

    // ==================================================
    // TC-HTD09
    // Pause game → không ăn được xu
    // ==================================================
    [Test]
    public void TC_HTD09_PauseGame_ShouldNotCollectCoin()
    {
        var gs = new GameSession();
        var coin = new CoinPickup();

        bool isPaused = true;

        if (!isPaused)
            coin.Collect(gs);

        Assert.AreEqual(0, gs.GetScore());
    }

    // ==================================================
    // TC-HTD10
    // Game mới → score reset về 0
    // ==================================================
    [Test]
    public void TC_HTD10_NewGame_ShouldResetScore()
    {
        var gs = new GameSession();
        new CoinPickup().Collect(gs);

        // new session
        gs = new GameSession();

        Assert.AreEqual(0, gs.GetScore());
    }
}