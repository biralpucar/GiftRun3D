using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntrySceneHandler : MonoSingleton<EntrySceneHandler>
{
    protected override void Awake()
    {
        base.Awake();

        if (IsStray()) return;

        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        Application.targetFrameRate = 999;
        QualitySettings.vSyncCount = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        yield return new WaitForSeconds(0.25f);

        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int targetLevelIndex = PlayerPrefs.GetInt(GameManager.lastPlayedStageKey, currentLevelIndex + 1);
        SceneManager.LoadScene(targetLevelIndex);
    }
}
