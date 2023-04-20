using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButtons : MonoBehaviour
{

    public static InputButtons currentInstance;

    // store the canvases
    public GameObject[] rotationCanvasArr;
    public GameObject movementCanvas;
    public GameObject upButton;
    public GameObject downButton;
    public GameObject leftButton;
    public GameObject rightButton;

    // save the active/current block
    private GameObject currentBlock;
    private Block currentTetris;
    private bool isMoveOn = true;

    // singleton
    private void Awake()
    {
        currentInstance = this;
    }

    private void Start()
    {
        SetInput();
    }

    // position the canvas to the current block 
    private void Update() {
        PositionCanvas();
    }

    // position the canvas to the current block
    private void PositionCanvas() {

        if (currentBlock == null) {
            return;
        } else {
            Vector3 blockPosition = currentTetris.GetCenterPoint();
            transform.position = blockPosition;
            AlignMoveButtons(blockPosition, 3, 3, 3, 3);
        }
    }

    private void AlignMoveButtons(Vector3 blockPosition, int upDiff, int downDiff, int rightDiff, int leftDiff) {
        // change position of button
        Vector3 buttonPosition = blockPosition;
        // upButton
        buttonPosition.z = blockPosition.z + upDiff;
        upButton.transform.position = buttonPosition;
        // downButton
        buttonPosition.z = blockPosition.z - downDiff;
        downButton.transform.position = buttonPosition;
        // right button
        buttonPosition.z = blockPosition.z;
        buttonPosition.x = blockPosition.x + rightDiff;
        rightButton.transform.position = buttonPosition;
        // left button
        buttonPosition.x = blockPosition.x - leftDiff;
        leftButton.transform.position = buttonPosition;
    }

    // initiate the current block. args: gameobject of the block and the actual block instance
    public void SetCurrentBlock(GameObject block, Block blockScriptInstance) {
        currentBlock = block;
        currentTetris = blockScriptInstance;
    }

    public void MoveBlockButton(string direction)
    {
        if (currentBlock != null)
        {
            switch (direction)
            {
                case "left": 
                    currentTetris.ExecuteMove(Vector3.left);
                    break;
                case "right":
                    currentTetris.ExecuteMove(Vector3.right);
                    break;
                case "up":
                    currentTetris.ExecuteMove(Vector3.forward);
                    break;
                case "down":
                    currentTetris.ExecuteMove(Vector3.back);
                    break;

            }
        }
    }

    public void SpeedButton() {
        GameManager.currentInstance.SetHighSpeed();
    }

    public void RotateBlockButton(string rotation)
    {
        if (currentBlock != null)
        {
            
            switch (rotation)
            {
                //X Rotation 
                case "posX":
                    currentTetris.ExecuteRotation(new Vector3(90, 0, 0));
                    break;
                case "negX":
                    currentTetris.ExecuteRotation(new Vector3(-90, 0, 0));
                    break;
                //Y Rotation 
                case "posY":
                    currentTetris.ExecuteRotation(new Vector3(0, 90, 0));
                    break;
                case "negY":
                    currentTetris.ExecuteRotation(new Vector3(0, -90, 0));
                    break;
                //Z rotation
                case "posZ":
                    currentTetris.ExecuteRotation(new Vector3(0, 0, 90));
                    break;
                case "negZ":
                    currentTetris.ExecuteRotation(new Vector3(0, 0, -90));
                    break;
            }

        }
    }

    public void SwitchInputButton()
    {
        isMoveOn = !isMoveOn;
        SetInput();
    }

    public bool GetIsMoveOn() {
        return isMoveOn;
    }

    private void SetInput()
    {
        movementCanvas.SetActive(isMoveOn);
        foreach (GameObject rotationButton in rotationCanvasArr)
        {
            rotationButton.SetActive(!isMoveOn);
        }
    }

}
