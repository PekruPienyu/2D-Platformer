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
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private Button leftKeyButton;
    [SerializeField] private Button rightKeyButton;
    [SerializeField] private Button jumpKeyButton;
    [SerializeField] private Button sprintKeyButton;
    [SerializeField] private TMP_Text coinCountText;
    [SerializeField] private TMP_Text playerLiveText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;

    private bool isTakingInput;

    public enum PlayerAction
    {
        left,
        right,
        jump,
        sprint,
    }

    private PlayerAction currentPlayerAction;

    private void Start()
    {
        ResetAllButtonText();
        coinCountText.text = "X " + Player.instance.GetCurrentCoinCount();
        playerLiveText.text = "X " + Player.instance.GetCurrentLiveCount();
        scoreText.text = "" + Player.instance.GetCurrentScore();
        timeText.text = "" + Player.instance.GetCurrentRemainingTime();

        Player.instance.coinAddEvent += UpdateCoinCount;
        Player.instance.timeDecreaseEvent += UpdateRemainingtime;
        Player.instance.scoreAddEvent += UpdateScore;
        Player.instance.liveCountUpdateEvent += UpdateLiveCount;
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

    public void UpdateCoinCount()
    {
        coinCountText.text = "X " + Player.instance.GetCurrentCoinCount();
    }

    public void UpdateRemainingtime()
    {
        timeText.text = "" + Player.instance.GetCurrentRemainingTime();
    }

    public void UpdateScore()
    {
        scoreText.text = "" + Player.instance.GetCurrentScore();
    }

    public void UpdateLiveCount()
    {
        playerLiveText.text = "X " + Player.instance.GetCurrentLiveCount();
    }

    private void ResetAllButtonText()
    {
        leftKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.left.ToString();
        rightKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.right.ToString();
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

    public void GetKeyInputForJump()
    {
        currentPlayerAction = PlayerAction.jump;
        if (!isTakingInput)
        {
            isTakingInput = true;
        }
    }

    public void GetKeyInputForSprint()
    {
        currentPlayerAction = PlayerAction.sprint;
        if (!isTakingInput)
        {
            isTakingInput = true;
        }
    }

    public void OnBackButtonPressed()
    {
        settingsPanel.SetActive(false);
        backgroundPanel.SetActive(true);
        playerController.ResumeGame();
    }

    public void OnSettingsButtonPressed()
    {
        settingsPanel.SetActive(true);
        backgroundPanel.SetActive(false);
        playerController.PauseGame();
    }

    public void OnResetToDefaultButtonPressed()
    {
        playerControllerKeys.ResetToDefaultKeys();
        ResetAllButtonText();
    }
}
