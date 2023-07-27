using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public Player_Controller playerController;
    public PlayerControllerKeys_SO playerControllerKeys;
    public GameObject settingsPanel;
    public GameObject backgroundPanel;
    public Button leftKeyButton;
    public Button rightKeyButton;
    public Button jumpKeyButton;
    public Button sprintKeyButton;

    public bool isTakingInput;

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
        leftKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.left.ToString();
        rightKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.right.ToString();
        jumpKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.jump.ToString();
        sprintKeyButton.GetComponentInChildren<TMP_Text>().text = playerControllerKeys.sprint.ToString();
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

    public void OnSaveButtonPressed()
    {
        ChangePlayerKeyLeft();
        ChangePlayerKeyRight();
        ChangePlayerKeyJump();
        ChangePlayerKeySprint();
    }

    public void OnBackButtonPressed()
    {
        settingsPanel.SetActive(false);
        backgroundPanel.SetActive(true);
        playerController.pauseGame = false;
    }

    public void OnSettingsButtonPressed()
    {
        settingsPanel.SetActive(true);
        backgroundPanel.SetActive(false);
        playerController.pauseGame = true;
    }

    public void ChangePlayerKeyLeft()
    {
        if (leftKeyButton.GetComponentInChildren<TMP_Text>().text.Length > 1) return;
        char keyCode = leftKeyButton.GetComponentInChildren<TMP_Text>().text[0];
        playerController.ChangePlayerKeyLeft(keyCode);
        leftKeyButton.GetComponentInChildren<TMP_Text>().text = keyCode.ToString().ToUpper();
    }

    public void ChangePlayerKeyRight()
    {
        if (rightKeyButton.GetComponentInChildren<TMP_Text>().text.Length > 1) return;
        char keyCode = rightKeyButton.GetComponentInChildren<TMP_Text>().text[0];
        playerController.ChangePlayerKeyRight(keyCode);
        rightKeyButton.GetComponentInChildren<TMP_Text>().text = keyCode.ToString().ToUpper();
    }

    public void ChangePlayerKeyJump()
    {
        if (jumpKeyButton.GetComponentInChildren<TMP_Text>().text.Length > 1) return;
        char keyCode = jumpKeyButton.GetComponentInChildren<TMP_Text>().text[0];
        playerController.ChangePlayerKeyJump(keyCode);
        jumpKeyButton.GetComponentInChildren<TMP_Text>().text = keyCode.ToString().ToUpper();
    }

    public void ChangePlayerKeySprint()
    {
        if (sprintKeyButton.GetComponentInChildren<TMP_Text>().text.Length > 1) return;
        char keyCode = sprintKeyButton.GetComponentInChildren<TMP_Text>().text[0];
        playerController.ChangePlayerKeySprint(keyCode);
        sprintKeyButton.GetComponentInChildren<TMP_Text>().text = keyCode.ToString().ToUpper();
    }
}
