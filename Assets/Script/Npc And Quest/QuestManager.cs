using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public Text questTextUI;
    public int requiredKills = 60;
    public int currentKills = 0;

    void Start()
    {
        questTextUI.gameObject.SetActive(false); // Ẩn ban đầu
    }

    public void StartQuestDisplay()
    {
        questTextUI.gameObject.SetActive(true); // ✅ Hiển thị khi nhiệm vụ bắt đầu
        UpdateQuestText();
    }

    public void AddKill()
    {
        currentKills++;
        UpdateQuestText();

        if (currentKills >= requiredKills)
        {
            Debug.Log("✅ Nhiệm vụ hoàn thành!");
            FindObjectOfType<Npc>().MarkQuestComplete();
        }
    }

    void UpdateQuestText()
    {
        questTextUI.text = $"Nhiệm vụ: Tiêu diệt {requiredKills} quái vật\n{currentKills}/{requiredKills}";
    }
}