// File: AttackZone.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackZone : MonoBehaviour
{
    private Boss2 boss;

    void Start()
    {
        boss = GetComponentInParent<Boss2>();
        if (boss == null)
            Debug.LogError($"[AttackZone] boss null! Hierarchy: {GetPath(transform)}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) boss.SetAttackZone(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) boss.SetAttackZone(false);
    }

    string GetPath(Transform t)
    {
        string p = t.name;
        while (t.parent != null) { t = t.parent; p = t.name + "/" + p; }
        return p;
    }
}
