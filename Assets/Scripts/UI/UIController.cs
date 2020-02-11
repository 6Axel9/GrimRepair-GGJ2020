using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_menu;
    [SerializeField]
    private GameObject m_game;
    [SerializeField]
    private GameObject m_rules;
    [SerializeField]
    private GameObject m_over;
    [SerializeField]
    private Camera m_cameraUp;
    [SerializeField]
    private Camera m_cameraDown;
    [SerializeField]
    private PlayerBehaviour m_firstPlayer;
    [SerializeField]
    private PlayerBehaviour m_secondPlayer;
    [SerializeField]
    private TMP_Text m_winnerScoreText;
    [SerializeField]
    private TMP_Text m_counterText;
    [SerializeField]
    private TMP_Text m_timerText;
    [SerializeField]
    private Image m_winner;

    [SerializeField]
    private float m_gameTime = 60f;

    private float m_elapsedTime;
    private bool m_isPlaying;

    private int m_counter = 3;
    public int Counter
    {
        set
        {
            m_counter = value;
            m_counterText.text = m_counter.ToString();
        }
    }

    private void Update()
    {
        if (!m_isPlaying)
            return;

        m_elapsedTime += Time.deltaTime;
        m_timerText.text = ((int)(m_gameTime - m_elapsedTime)).ToString();

        if (m_elapsedTime > m_gameTime)
        {
            m_elapsedTime = 0f;
            GameOver();
        }
    }

    public void CountDown(Action callback)
    {
        if (m_counter == 0)
        {
            callback.Invoke();
            return;
        }

        Counter = m_counter - 1;
        
    }

    public void ToogleRules()
    {
        m_rules.SetActive(m_rules.activeSelf ? false : true);
    }

    public void VersusMode()
    {
        m_cameraUp.gameObject.SetActive(true);
        m_cameraUp.rect = new Rect(0f, 0.5f, 1f, 0.5f);

        m_cameraDown.gameObject.SetActive(true);
        m_cameraDown.rect = new Rect(0f, 0.0f, 1f, 0.5f);

        m_menu.SetActive(false);
        m_game.SetActive(true);

        void callback()
        {
            m_counterText.gameObject.SetActive(false);
            m_firstPlayer.Mapping.Controls = PlayerType.Manual;
            m_secondPlayer.Mapping.Controls = PlayerType.Manual;
            m_firstPlayer.Agent.enabled = true;
            m_secondPlayer.Agent.enabled = true;

            m_isPlaying = true;
        }

        gameObject.LeanDelayedCall(1f, () => CountDown(callback)).setRepeat(4);
    }

    public void SingleMode()
    {
        m_cameraUp.gameObject.SetActive(true);
        m_cameraUp.rect = new Rect(0f, 0f, 1f, 1f);

        m_menu.SetActive(false);
        m_game.SetActive(true);

        void callback()
        {
            m_counterText.gameObject.SetActive(false);
            m_firstPlayer.Mapping.Controls = PlayerType.Manual;
            m_secondPlayer.Mapping.Controls = PlayerType.Auto;
            m_firstPlayer.Agent.enabled = true;
            m_secondPlayer.Agent.enabled = true;

            m_isPlaying = true;
        }

        gameObject.LeanDelayedCall(1f, () => CountDown(callback)).setRepeat(4);
    }

    public void GameOver()
    {
        m_cameraUp.gameObject.SetActive(true);
        m_cameraUp.rect = new Rect(0f, 0f, 1f, 1f);

        if (m_firstPlayer.Score > m_secondPlayer.Score)
        {
            m_winnerScoreText.text = m_firstPlayer.Score.ToString();
            m_winner.sprite = m_firstPlayer.Mapping.WinnerIco;
        }
        else
        {
            m_winnerScoreText.text = m_secondPlayer.Score.ToString();
            m_winner.sprite = m_secondPlayer.Mapping.WinnerIco;
        }

        m_game.SetActive(false);
        m_over.SetActive(true);
        m_isPlaying = false;
    }

    public void ResetToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
