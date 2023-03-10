using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string debugStartMessage;

    public GameObject player1;
    public GameObject player2;

    public GameObject player1Indicator;
    public GameObject player2Indicator;

    public int heightOfBoard = 6;
    public int lengthOfBoard = 7;

    
    public GameObject[] spawnLoc;

    GameObject fallingPiece;

    bool player1Turn = true;

    int[,] boardState; 

    void Start()
    {
        Debug.Log(debugStartMessage);

        boardState = new int[lengthOfBoard, heightOfBoard];

        player1Indicator.SetActive(false);
        player2Indicator.SetActive(false);

    }

    public void HoverColumn(int column)
    {
        if (boardState[column, heightOfBoard - 1] == 0 && (fallingPiece == null || fallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
        {
            if (player1Turn)
            {
                player1Indicator.SetActive(true);
                player1Indicator.transform.position = spawnLoc[column].transform.position;
            }
            else
            {
                player2Indicator.SetActive(true);
                player2Indicator.transform.position = spawnLoc[column].transform.position;
            }
        }
    }
    
    public void SelectColumn(int column)
    {
       if (fallingPiece == null || fallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero)
       {
            Debug.Log("GameManager Column " + column);
            TakeTurn(column);
       }
    }
    
    void TakeTurn(int column)
    {
        if (UpdateBoardState(column))
        {
            player1Indicator.SetActive(false);
            player2Indicator.SetActive(false);

            if (player1Turn)
            {
                fallingPiece = Instantiate(player1, spawnLoc[column].transform.position, Quaternion.identity);
                fallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
                player1Turn = false;

                if (DidWin(1))
                {
                    Debug.LogWarning("Player 1 has won!");
                }
            }
            else
            {
                fallingPiece = Instantiate(player2, spawnLoc[column].transform.position, Quaternion.identity);
                fallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
                player1Turn = true;
            
                if (DidWin(2))
                {
                    Debug.LogWarning("Player 2 has won!");
                }
            }
        }
    }
    
    
    bool UpdateBoardState(int column)
    {
        for (int row = 0; row < heightOfBoard; row++)
        {
            if (boardState[column, row] == 0)
            {
                if (player1Turn)
                {
                    boardState[column, row] = 1;
                }
                else
                {
                    boardState[column, row] = 2; 
                }
                Debug.Log("Piece being spawned at (" + column + ", " + row + ")");
                return true;
            }
        }
        Debug.LogWarning("isFull");
        return false;
    }
    
    bool DidWin(int playerNum)
    { //Horizontal
        for (int x = 0; x < lengthOfBoard - 3; x++)
        {
            for (int y = 0; y < heightOfBoard; y++)
            {
                if (boardState[x,y] == playerNum && boardState[x + 1, y] == playerNum && boardState[x + 3, y] == playerNum)
                {
                    return true;
                }
            }
        }
        //Vertical 
        for (int x = 0; x < lengthOfBoard; x++)
        { 
            for (int y = 0; y < heightOfBoard - 3; y++)
            {
                if (boardState[x, y] == playerNum && boardState[x, y + 1] == playerNum && boardState[x, y + 2 ] == playerNum && boardState[x, y + 3] == playerNum) 
                {
                    return true;
                }
            }
        }
        // y = x
        
        for (int x = 0; x < lengthOfBoard - 3;  x++)
        {
            for (int y = 0; y < heightOfBoard - 3; y++)
            {
                if (boardState[x, y] == playerNum && boardState[x+ 1, y + 1] == playerNum && boardState[x + 2, y + 2] == playerNum && boardState[x + 3, y + 3] == playerNum)
                {
                    return true;
                }
            }
        }
        // y = -x

        for (int x = 0; x < lengthOfBoard - 3; x++)
        {
            for (int y = 0; y < heightOfBoard - 3; y++)
            {
                if (boardState[x, y + 3] == playerNum && boardState[x + 1, y + 2] == playerNum && boardState[x + 2, y + 1] == playerNum && boardState[x + 3, y] == playerNum)
                {
                    return true;
                }
            }
        }
        return false;
    }
}