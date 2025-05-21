using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private AudioSource _audioSource; // AudioSource component
    [SerializeField] private AudioClip _gameOver;     // game over sound
    [SerializeField] private AudioClip _reStart;     // restart sound

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Time.timeScale = 1.0f;
    }

    public void GameOver()
    {
        _gameOverCanvas.SetActive(true);
        _audioSource.Stop();
        _audioSource.PlayOneShot(_gameOver);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        // play the restart SFX
        _audioSource.PlayOneShot(_reStart);

        // wait in real time so it works even if timescale is 0
        yield return new WaitForSecondsRealtime(_reStart.length);

        // now reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
