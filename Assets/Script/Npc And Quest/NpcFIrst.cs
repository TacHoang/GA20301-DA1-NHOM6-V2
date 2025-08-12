using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NpcFirst : MonoBehaviour
{
    [Header("Thông tin NPC")]
    public string npcName;
    public string[] dialogues;

    [Header("UI")]
    public Text dialogueTextUI;
    public GameObject dialoguePanel;

    [Header("Thiết lập")]
    public Transform player;
    public GameObject portal;
    public float watchDistance = 5f;

    private int dialogueIndex = 0;
    private bool isTalking = false;
    private bool hasTalked = false;

    void Update()
    {
        if (isTalking && Input.GetMouseButtonDown(0))
        {
            ContinueDialogue();
        }

        if (Vector2.Distance(transform.position, player.position) < watchDistance)
        {
            transform.localScale = new Vector3(player.position.x < transform.position.x ? 1 : -1, 1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTalked)
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        hasTalked = true;
        Time.timeScale = 0;
        dialoguePanel.SetActive(true);
        dialogueIndex = 0;
        ShowDialogue(dialogues[dialogueIndex]);
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
            StartCoroutine(EndDialogueAfterDelay());
        }
    }

    void ShowDialogue(string text)
    {
        dialogueTextUI.text = npcName + ": " + text;
    }

    IEnumerator EndDialogueAfterDelay()
    {
        ShowDialogue("Hãy qua cổng dịch chuyển để bắt đầu. Chúc bạn may mắn!");
        yield return new WaitForSecondsRealtime(1f);
        EndDialogue();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueTextUI.text = "";
        isTalking = false;
        Time.timeScale = 1;
        ActivatePortal();
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