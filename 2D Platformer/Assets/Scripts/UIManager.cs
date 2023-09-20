using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Player_Controller playerController;
    [SerializeField] private PlayerControllerKeys_SO playerControllerKeys;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private Button leftKeyButton;
    [SerializeField] private Button rightKeyButton;
    [SerializeField] private Button downKeyButton;
    [SerializeField] private Button jumpKeyButton;
    [SerializeField] private Button sprintKeyButton;
    [SerializeField] private TMP_Text coinCountText;
    [SerializeField] private TMP_Text playerLiveText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text worldTitle;

    private bool isTakingInput;

    public static UIManager instance;

    public enum PlayerAction
    {
        left,
        right,
        down,
        jump,
        sprint,
    }

    private PlayerAction currentPlayerAction;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ResetAllButtonText();
        coinCountText.text = "X " + MainManager.instance.GetCurrentCoinCount();
        playerLiveText.text = "X " + MainManager.instance.GetCurrentLiveCount();
        scoreText.text = "" + MainManager.instance.GetCurrentScore();
        timeText.text = "" + MainManager.instance.GetCurrentRemainingTime();

        MainManager.instance.coinAddEvent += UpdateCoinCount;
        MainManager.instance.timeDecreaseEvent += UpdateRemainingtime;
        MainManager.instance.scoreAddEvent += UpdateScore;
        MainManager.instance.liveCountUpdateEvent += UpdateLiveCount;
    }

    private void Update()
    {
        if(isTakingInput && Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    switch (currentPlayerAction)
                    {
                        case PlayerAction.left:
                            playerControllerKeys.left = keyCode;
                            leftKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.left.ToString();
                            break;
                        case PlayerAction.right:
                            playerControllerKeys.right = keyCode;
                            rightKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.right.ToString();
                            break;
                        case PlayerAction.down:
                            playerControllerKeys.right = keyCode;
                            downKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.down.ToString();
                            break;
                        case PlayerAction.jump:
                            playerControllerKeys.jump = keyCode;
                            jumpKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.jump.ToString();
                            break;
                        case PlayerAction.sprint:
                            playerControllerKeys.sprint = keyCode;
                            sprintKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.sprint.ToString();
                            break;
                    }
                    isTakingInput = false;
                    break;
                }
            }
        }
    }

    public void UpdateWorldTitle(string title)
    {
        worldTitle.text = "World "+title;
    }

    public void UpdateCoinCount()
    {
        coinCountText.text = "X " + MainManager.instance.GetCurrentCoinCount();
    }

    public void UpdateRemainingtime()
    {
        timeText.text = "" + MainManager.instance.GetCurrentRemainingTime();
    }

    public void UpdateScore()
    {
        scoreText.text = "" + MainManager.instance.GetCurrentScore();
    }

    public void UpdateLiveCount()
    {
        playerLiveText.text = "X " + MainManager.instance.GetCurrentLiveCount();
    }

    public void UpdateUI()
    {
        UpdateLiveCount();
        UpdateScore();
        UpdateCoinCount();
        UpdateRemainingtime();
    }

    private void ResetAllButtonText()
    {
        leftKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.left.ToString();
        rightKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.right.ToString();
        downKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.down.ToString();
        jumpKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.jump.ToString();
        sprintKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.sprint.ToString();
    }

    public void GetKeyInputForLeft()
    {
        currentPlayerAction = PlayerAction.left;
        isTakingInput = true;
    }

    public void GetKeyInputForRight()
    {
        currentPlayerAction = PlayerAction.right;
        isTakingInput = true;
    }

    public void GetKeyInputForDown()
    {
        currentPlayerAction = PlayerAction.down;
        isTakingInput = true;
    }

    public void GetKeyInputForJump()
    {
        currentPlayerAction = PlayerAction.jump;
        isTakingInput = true;
    }

    public void GetKeyInputForSprint()
    {
        currentPlayerAction = PlayerAction.sprint;
        isTakingInput = true;
    }

    public void LoadMainMenuPanel()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        backgroundPanel.SetActive(false);
    }

    public void OnStartButtonPressed()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        backgroundPanel.SetActive(true);
        SceneLoader.instance.LoadNextScene();
    }

    public void OnContinueButtonPressed()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        backgroundPanel.SetActive(true);
        SceneLoader.instance.LoadSavedScene();
    }

    public void OnBackButtonPressed()
    {
        settingsPanel.SetActive(false);
        backgroundPanel.SetActive(true);
        MainManager.instance.ResumeGame();
    }

    public void OnSettingsButtonPressed()
    {
        settingsPanel.SetActive(true);
        backgroundPanel.SetActive(false);
        MainManager.instance.PauseGame();
    }

    public void OnResetToDefaultButtonPressed()
    {
        playerControllerKeys.ResetToDefaultKeys();
        ResetAllButtonText();
    }
}
