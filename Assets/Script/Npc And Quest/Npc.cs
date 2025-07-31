using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    public string npcName;
    public string[] dialogues;
    public Text dialogueTextUI;
    public GameObject dialoguePanel;
    public Transform player;
    public Transform teleportTarget;
    public GameObject portal;
    public float watchDistance = 5f;
    public SkillManager skillManager;

    private int dialogueIndex = 0;
    private bool isTalking = false;
    private bool questGiven = false;
    private bool questCompleted = false;
    private bool showingWarning = false;

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
            if (showingWarning)
            {
                dialogueTextUI.text = "";
                dialoguePanel.SetActive(false);
                isTalking = false;
                showingWarning = false;
                Time.timeScale = 1;
                skillManager?.UnlockAllSkills();
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

        if (!questGiven)
        {
            dialogueIndex = 0;
            dialoguePanel.SetActive(true);
            ShowDialogue(dialogues[dialogueIndex]);
            skillManager?.LockAllSkills();
        }
        else
        {
            dialoguePanel.SetActive(true);

            if (!questCompleted)
            {
                dialogueTextUI.text = npcName + ": Bạn chưa hoàn thành nhiệm vụ.";
            }
            else
            {
                dialogueTextUI.text = npcName + ": Bạn đã hoàn thành nhiệm vụ, cổng dịch chuyển qua màn đã mở!";
                TeleportToTarget();
                ActivatePortal();
            }

            showingWarning = true;
            skillManager?.LockAllSkills();
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

    void ShowDialogue(string text)
    {
        dialogueTextUI.text = npcName + ": " + text;
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

    void GiveQuest()
    {
        questGiven = true;
        Debug.Log("📜 Nhiệm vụ đã giao!");

        QuestManager qm = FindObjectOfType<QuestManager>();
        if (qm != null)
        {
            qm.StartQuestDisplay(); // ✅ Hiển thị text nhiệm vụ
        }
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