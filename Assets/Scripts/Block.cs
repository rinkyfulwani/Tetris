using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float previousTime;
    private float blockFallTime;

    void Start() {
        // initiate 
        InputButtons.currentInstance.SetCurrentBlock(gameObject, this);
        blockFallTime = GameManager.currentInstance.GetGameSpeed();

        // check if game over
        if (!IsValidMove()) {
            GameManager.currentInstance.SetIsGameOver(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //print("Time.time ==" + Time.time + "and previousTime ==" + previousTime);
        if(Time.time-previousTime > blockFallTime)
        {
            transform.position += Vector3.down;
            if (!IsValidMove())
            {
                transform.position += Vector3.up;
                //delete the layers if needed
                Grid.Instance.RemoveGridLayer();
                enabled = false;
                //create a new block if game is not over
                if (!GameManager.currentInstance.GetIsGameOver()) {
                    Grid.Instance.GenerateNewBlocks();
                }
            } else {
                //update the grid
                Grid.Instance.UpdateGrid(this);
            }
            previousTime = Time.time;
        }

        // check for user interaction
        HandleUserInput();

        // get current game speed;
        blockFallTime = GameManager.currentInstance.GetGameSpeed();

    }

    // handles user input if the user wants to move or rotate the block
    private void HandleUserInput() {
        bool isMoveOn = InputButtons.currentInstance.GetIsMoveOn();
        if (Input.GetKeyDown(KeyCode.R)) {
            InputButtons.currentInstance.SwitchInputButton();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            GameManager.currentInstance.SetHighSpeed();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!GameManager.currentInstance.GetIsGamePaused()) {
                GameManager.currentInstance.PauseGame();
            } else {
                GameManager.currentInstance.ResumeGame();
            }

        }

        if (Input.GetKeyDown(KeyCode.UpArrow) | Input.GetKeyDown(KeyCode.W) ) {
            if (!isMoveOn) {
                ExecuteRotation(new Vector3(90, 0, 0));
            } else {
                ExecuteMove(Vector3.forward);
            }

        } else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            if (!isMoveOn) {
                ExecuteRotation(new Vector3(0, 0, 90));
            } else {
                ExecuteMove(Vector3.left);
            }


        } else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            if (!isMoveOn) {
                ExecuteRotation(new Vector3(-90, 0, 0));
            } else {
                ExecuteMove(Vector3.back);
            }

                
        } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            if (!isMoveOn) {
                ExecuteRotation(new Vector3(0, 0, -90));
            } else {
                ExecuteMove(Vector3.right);
            }

        }

    }

    public Vector3 GetCenterPoint() {
        
        float xSum = 0f;
        float ySum = 0f;
        float zSum = 0f;
        int numBlocks = 0;

        foreach(Transform childCube in transform) {
            numBlocks = numBlocks + 1;

            xSum = xSum + childCube.position.x;
            ySum = ySum + childCube.position.y;
            zSum = zSum + childCube.position.z;
        }

        xSum = xSum / numBlocks;
        ySum = ySum / numBlocks;
        zSum = zSum / numBlocks;

        Vector3 centerPos = new Vector3(xSum, ySum, zSum);

        return centerPos;
    }

    // rotate the block if valid move
    public void ExecuteRotation(Vector3 rotation) {
        transform.Rotate(rotation,  Space.World);
        if (IsValidMove()) {
            Grid.Instance.UpdateGrid(this);
        } else {
            transform.Rotate(-rotation, Space.World);
        }
    }

    // input will always be 1 in any direction
    public void ExecuteMove(Vector3 moveDirection) {

        transform.position  += moveDirection;
        
        // check valid move
        if (IsValidMove()) {
            // update the grid with the block
            Grid.Instance.UpdateGrid(this);
        } else {
            transform.position = transform.position - moveDirection;
        }
    }

    private bool IsValidMove()
    {
        foreach(Transform childCube in transform)
        {
            Vector3 position = Grid.Instance.VectorRound(childCube.position);
            if(!Grid.Instance.IsInsideGrid(position))
            {
                return false;
            }
        }
        foreach (Transform childCube in transform)
        {
            Vector3 roundedPosition = Grid.Instance.VectorRound(childCube.position);
            Transform childTransform = Grid.Instance.GetTransformOnGridPosition(roundedPosition);
            if(childTransform != null && childTransform.parent != transform)
            {
                return false;
            }
        }
        return true;
    }

}
