using UnityEngine;

public class BossBattleManager : MonoBehaviour
{
    public BossHealth boss1;
    public BossHealth boss2;

    public GameObject gameFinishedPanel;

    private bool bossMusicPlaying = false;
    private bool boss1Triggered = false;
    private bool boss2Triggered = false;

    private bool boss1Dead = false;
    private bool boss2Dead = false;

    void Update()
    {
        if ((boss1Triggered || boss2Triggered) && !bossMusicPlaying)
        {
            Debug.Log("Boss music triggered.");
            MusicManager.Instance.PlayMusic("BossFight");
            bossMusicPlaying = true;

            Debug.Log($"bossMusicPlaying set to {bossMusicPlaying}");
        }
    }

    public void NotifyBossDeath(BossHealth boss) 
    {
        if (boss == boss1 && boss1.IsDead())
        {
            boss1Dead = true;
            Debug.Log("Boss 1 is dead.");
        }
        else if (boss == boss2 && boss2.IsDead())
        {
            boss2Dead = true;
            Debug.Log("Boss 2 is dead.");
        }

        if (boss1Dead && boss2Dead)
        {
            OnVictory();
        }
    }

    public void TriggerBoss(BossHealth boss)
    {
        if (boss == boss1)
        {
            boss1Triggered = true;
            Debug.Log("Boss 1 has been triggered.");
        }
        if (boss == boss2)
        {
            boss2Triggered = true;
            Debug.Log("Boss 2 has been triggered.");
        }
    }

    void OnVictory()
    {
        Debug.Log("Both bosses defeated. Triggering victory!");

        if (bossMusicPlaying)
        {
            Debug.Log("Stopping boss music. Playing victory music.");
            MusicManager.Instance.PlayMusic("End", 0.5f);
            bossMusicPlaying = false;
        }

        if (gameFinishedPanel != null)
        {
            Debug.Log("Activating game finished panel.");
            gameFinishedPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Game finished panel is not assigned!");
        }

        enabled = false;
    }
}
