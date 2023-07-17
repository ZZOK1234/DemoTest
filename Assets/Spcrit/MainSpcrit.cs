using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSpcrit : MonoBehaviour {
    private const int NOPLAYER = 0;
    private const int PLAYER1 = 1;
    private const int PLAYER2 = 2;

    private int gameTurn = PLAYER1;
    private int totalMoves = 0;
    private int totalPlayer = 1;
    private int [,] chessBoard = new int [3, 3];

    private static int buttonWidth = 100;
    private static int buttonHeight = 100;
    private static int firstButtonX = (Screen.width - 3 * MainSpcrit.buttonWidth) / 2;
    private static int firstButtonY = Screen.height - 3 * MainSpcrit.buttonHeight - 10;
    
    public Texture2D backgroundImage;
    public GUISkin gameSkin;

    void InitGameWithTotalPlayer(int playersNum) {
        gameTurn = PLAYER1;
        totalMoves = 0;
        totalPlayer = playersNum;
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                chessBoard[i, j] = NOPLAYER;
            }
        }
    }

    void ComputerMove() {
        System.Random ran = new System.Random();
        int randomNum = ran.Next(0, 8);
        int xIndex, yIndex;
        do {
            xIndex = randomNum / 3;
            yIndex = randomNum % 3;
            randomNum = (randomNum + 1) % 8;
        } while (chessBoard[yIndex, xIndex] != NOPLAYER);
        chessBoard[yIndex, xIndex] = gameTurn;
        gameTurn = (gameTurn == PLAYER1 ? PLAYER2 : PLAYER1);
        totalMoves++;
        GetGameButtonText(xIndex, yIndex);
    }

    int CheckWinner() {
        for (int i = 0; i < 3; ++i) {
            if (chessBoard[i, 0] != NOPLAYER && 
                chessBoard[i, 0] == chessBoard[i, 1] && 
                chessBoard[i, 1] == chessBoard[i, 2]) {
                gameTurn = NOPLAYER;
                return chessBoard[i, 0];
            }
            if (chessBoard[0, i] != NOPLAYER && 
                chessBoard[0, i] == chessBoard[1, i] && 
                chessBoard[1, i] == chessBoard[2, i]) {
                gameTurn = NOPLAYER;
                return chessBoard[0, i];
            }
        }
        if (chessBoard[1, 1] != NOPLAYER) {
            if ((chessBoard[0, 0] == chessBoard[1, 1] && 
                chessBoard[1, 1] == chessBoard[2, 2]) || 
                (chessBoard[0, 2] == chessBoard[1, 1] && 
                chessBoard[1, 1] == chessBoard[2, 0])) {
                gameTurn = NOPLAYER;
                return chessBoard[1, 1];
            }
        }
        return NOPLAYER;
    }
    void AddBackground()
    {
        GUIStyle backgroundStyle = new GUIStyle();
        backgroundStyle.normal.background = backgroundImage;
        GUI.Label(new Rect(0, 0, 710, 388), "", backgroundStyle);
    }

    void AddTitle() {
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 35;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.black;
        GUI.Label(new Rect(Screen.width / 2 - 150, 20, 300, 50), "测试用井字棋Demo", titleStyle);
    }

    void AddTip() {
        GUI.skin = gameSkin;
        string text = GetTipText();
        GUI.Label(new Rect(Screen.width / 2 - 320, 70, 650, 50), text);
    }

    void AddButton() {
        GUI.skin = gameSkin;
        AddGameButton();
        AddResetButton();
    }

    void AddGameButton() {
        for (int xIndex = 0; xIndex < 3; ++xIndex) {
            for (int yIndex = 0; yIndex < 3; ++yIndex) {

                int buttonX = firstButtonX + xIndex * buttonWidth;
                int buttonY = firstButtonY + yIndex * buttonHeight;
                string text = GetGameButtonText(xIndex, yIndex);
                if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), text)) {
                    SetGameButtonFunction(xIndex, yIndex);
                }
            }
        }
    }

    void AddResetButton() {
        GUIStyle resetStyle = new GUIStyle("button");
        resetStyle.fontSize = 20;
        if (GUI.Button(new Rect(firstButtonX + 3 * buttonWidth + 50, Screen.height - 70, 80, 50), "Reset", resetStyle)) {
            InitGameWithTotalPlayer(1);
        }
    }
    
    string GetTipText() {
        int winner = CheckWinner();
        switch (winner) {
            case NOPLAYER:
                if (totalMoves == 0) {
                    return "请点击方块开始游戏";
                } else if (totalMoves == 9) {
                    return "请重新开始";
                } else {
                    if (totalPlayer == 1) {
                        return "游戏进行中...";
                    }
                    return "";
                }
            case PLAYER1:
                return "玩家获胜";
            case PLAYER2:
                return "PC获胜";
            default:
                return "";
        }
    }

    string GetGameButtonText(int xIndex, int yIndex) {
        int buttonX = firstButtonX + xIndex * buttonWidth * 5;
        int buttonY = firstButtonY + yIndex * buttonHeight * 5;
        int Player = chessBoard[yIndex, xIndex];
        switch (Player) {
            case NOPLAYER:
                return "";
            case PLAYER1:
                return "√";
            case PLAYER2:
                return "X";
            default:
                return "";
        }
    }

    void SetGameButtonFunction(int xIndex, int yIndex) {
        int Player = chessBoard[yIndex, xIndex];
        switch (Player) {
            case NOPLAYER:
                chessBoard[yIndex, xIndex] = gameTurn;
                gameTurn = (gameTurn == PLAYER1 ? PLAYER2 : PLAYER1);
                totalMoves++;
                if (totalPlayer == 1 && totalMoves < 9 && CheckWinner() == NOPLAYER) {
                    ComputerMove();
                }
                break;
            case PLAYER1:
                break;
            case PLAYER2:
                break;
        }
    }

    void PlayGameSystem() {
        AddBackground();  
        AddTitle();  
        AddTip();  
        AddButton();  
    }

    void OnGUI() {
        PlayGameSystem();
    }
}
