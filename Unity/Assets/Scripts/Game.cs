﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public Controller[] controllers = new Controller[2];
    [HideInInspector]
    public Button[,] buttons = new Button[3, 3];
    [HideInInspector]
    public char[,] characters = new char[,] { { '0', '0', '0' }, { '0', '0', '0' }, { '0', '0', '0' } };
    public Text endgameText;

    private Controller current;
    private int turn = 0;

    private void Awake() {
        // Get all available buttons, so we can change the text later on
        for (int y = 0; y < 3; y++) {
            Transform row = GameObject.Find(string.Format("Row:{0:00}", y)).transform;
            for (int x = 0; x < 3; x++) {
                buttons[x, y] = row.Find(string.Format("Tile:{0:00}", x)).GetComponent<Button>();
            }
        }

        foreach (Controller c in controllers) {
            c.Initialize(this);
        }

        current = controllers[0];
        current.OnEnableTurn();
    }

    public IEnumerator WaitForSeconds(float f, System.Action action) {
        yield return new WaitForSeconds(f);
        action.Invoke();
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlaceCharacter(Point2 p, Controller controller) {
        ClickButton(p, controller.character, controller.color);

        characters[p.x, p.y] = controller.character;
        Debug.Log(p + " " + controller.character);

        // Horizontal & Vertical
        if (IsHorizontalWin(p.y) || IsVerticalWin(p.x)) {
            EndGame(controller.winText, controller.color);
            return;
        }

        // Diagonal
        if (IsDiagonalWin(new Point2(0, 0), new Point2(2, 2)) || IsDiagonalWin(new Point2(0, 2), new Point2(2, 0))) {
            EndGame(controller.winText, controller.color);
            return;
        }

        turn++;
        if (turn > 8) {
            EndGame("It's a draw!", new Color(1, 218f / 255f, 122f / 255f, 1));
            return;
        }

        SetNextTurn();
    }

    private bool IsHorizontalWin(int row) {
        return characters[0, row] == characters[1, row] && characters[2, row] == characters[1, row] && characters[1, row] != '0';
    }

    private bool IsVerticalWin(int colum) {
        return characters[colum, 0] == characters[colum, 1] && characters[colum, 2] == characters[colum, 1] && characters[colum, 1] != '0';
    }

    private bool IsDiagonalWin(Point2 a, Point2 b) {
        return characters[a.x, a.y] == characters[1, 1] && characters[b.x, b.y] == characters[1, 1] && characters[1, 1] != '0';
    }

    private void SetNextTurn() {
        current.OnDisableTurn();
        current = controllers[turn % 2];
        current.OnEnableTurn();
    }

    private void ClickButton(Point2 p, char c, Color color) {
        buttons[p.x, p.y].interactable = false;
        Text text = buttons[p.x, p.y].GetComponentInChildren<Text>();
        text.text = c.ToString();
        text.color = color;
    }

    private void EndGame(string text, Color color) {
        foreach(Controller c in controllers) {
            c.OnDisableTurn();
        }

        endgameText.text = text;
        endgameText.color = color;
        endgameText.gameObject.SetActive(true);
    }
}