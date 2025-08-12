using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Npc : MonoBehaviour
{
    [Header("Thông tin NPC")]
    public string npcName;
    public string[] dialogues;             // Hội thoại lần đầu
    public string[] postQuestDialogues;    // Hội thoại sau khi hoàn thành

    [Header("UI")]
    public Text dialogueTextUI;
    public GameObject dialoguePanel;

    [Header("Thiết lập nhiệm vụ")]
    public Transform player;
    public Transform teleportTarget;
    public GameObject portal;
    public float watchDistance = 5f;

    [Header("Quản lý kỹ năng")]
    public SkillManager skillManager;

    private int dialogueIndex = 0;
    private int postDialogueIndex = 0;
    private bool isTalking = false;
    private bool questGiven = false;
    private bool questCompleted = false;
    private bool showingPostQuest = false;

    void Start()
    {
        if (skillManager == null)
        {
            skillManager = FindObjectOfType<SkillManager>();
        }
    }

    void Update()
    {
        if (isTalking && Input.GetMouseButtonDown(0))
        {
            if (showingPostQuest)
            {
                ContinuePostQuestDialogue();
            }
            else
            {
                ContinueDialogue();
            }
        }

        if (Vector2.Distance(transform.position, player.position) < watchDistance)
        {
            transform.localScale = new Vector3(player.position.x < transform.position.x ? 1 : -1, 1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        Time.timeScale = 0;
        dialoguePanel.SetActive(true);
        skillManager?.LockAllSkills();

        if (!questGiven)
        {
            dialogueIndex = 0;
            ShowDialogue(dialogues[dialogueIndex]);
        }
        else if (!questCompleted)
        {
            ShowDialogue(npcName + ": Bạn chưa hoàn thành nhiệm vụ.");
        }
        else
        {
            postDialogueIndex = 0;
            ShowDialogue(postQuestDialogues[postDialogueIndex]);
            showingPostQuest = true;
        }
    }

    void ContinueDialogue()
    {
        dialogueIndex++;
        if (dialogueIndex < dialogues.Length)
        {
            ShowDialogue(dialogues[dialogueIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    void ContinuePostQuestDialogue()
    {
        postDialogueIndex++;
        if (postDialogueIndex < postQuestDialogues.Length)
        {
            ShowDialogue(postQuestDialogues[postDialogueIndex]);
        }
        else
        {
            ShowFinalMessage(); // ✅ Hiển thị câu kết thúc cố định
        }
    }

    void ShowFinalMessage()
    {
        ShowDialogue(" Bạn đã hoàn thành nhiệm vụ, cổng dịch chuyển qua màn đã mở!");
        StartCoroutine(WaitAndEndPostQuestDialogue());
    }

    IEnumerator WaitAndEndPostQuestDialogue()
    {
        yield return new WaitForSecondsRealtime(1f); // ✅ Dùng thời gian thực
        EndPostQuestDialogue();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueTextUI.text = "";
        isTalking = false;
        Time.timeScale = 1;
        skillManager?.UnlockAllSkills();

        if (!questGiven)
        {
            GiveQuest();
            TeleportToTarget();
        }
    }

    void EndPostQuestDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueTextUI.text = "";
        isTalking = false;
        showingPostQuest = false;
        Time.timeScale = 1;
        skillManager?.UnlockAllSkills();
        ActivatePortal();
    }

    void ShowDialogue(string text)
    {
        dialogueTextUI.text = npcName + ": " + text;
    }

    void GiveQuest()
    {
        questGiven = true;
        Debug.Log("📜 Nhiệm vụ đã giao!");
        FindObjectOfType<QuestManager>()?.StartQuestDisplay();
    }

    public void MarkQuestComplete()
    {
        questCompleted = true;
        Debug.Log("✅ Nhiệm vụ hoàn thành!");
    }

    void TeleportToTarget()
    {
        if (teleportTarget != null)
        {
            transform.position = teleportTarget.position;
        }
    }

    void ActivatePortal()
    {
        if (portal != null)
        {
            portal.SetActive(true);
            Debug.Log("🚪 Cổng dịch chuyển mở!");
        }
    }
}