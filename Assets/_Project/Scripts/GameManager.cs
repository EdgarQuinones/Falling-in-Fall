
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text bestTimeText;

    public Button playAgainButton;
    public Button quitButton;

    private float startTime;
    private bool finished = false;
    private bool paused = false;
    private float bestTime;

    void Start()
    {
        Time.timeScale = 1f;

        startTime = Time.time;
        bestTime = PlayerPrefs.GetFloat("BestTime", -1f);

        playAgainButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        UpdateBestTime();
    }


    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed || finished)
            return;

        if (paused)
            ResumeGame();
        else
            PauseGame();
    }


    void Update()
    {
        if (finished || paused)
            return;

        float currentTime = Time.time - startTime;
        timeText.text = "Time: " + currentTime.ToString("F3");
    }

    public void PauseGame()
    {
        if (finished || paused)
            return;

        paused = true;
        Time.timeScale = 0f;

        playAgainButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        if (finished || !paused)
            return;

        paused = false;
        Time.timeScale = 1f;

        playAgainButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }
    public void FinishGame()
    {
        if (finished)
            return;

        finished = true;
        paused = false;

        float finalTime = Time.time - startTime;
        timeText.text = "Time: " + finalTime.ToString("F3");

        if (bestTime < 0 || finalTime < bestTime)
        {
            bestTime = finalTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save();
        }

        UpdateBestTime();

        playAgainButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }

    void UpdateBestTime()
    {
        bestTimeText.text = bestTime < 0
            ? "Best: ---.---"
            : "Best: " + bestTime.ToString("F3");
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
