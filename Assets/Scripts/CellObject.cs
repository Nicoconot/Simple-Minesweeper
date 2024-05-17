using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CellObject : MonoBehaviour, IPointerClickHandler
{
    public Cell cell;

    [SerializeField] private GameObject hiddenObj, revealedObj, bombObj, flagObj;

    [SerializeField] private TextMeshProUGUI numbText;

    private UnityAction<Cell> OnReveal, OnStart;
    private UnityAction OnGameOver, OnFlag;

    private bool isRevealed, isMarked;


    public void Setup(Cell cellToSet, UnityAction<Cell> actionOnReveal, UnityAction actionOnGameOver, UnityAction actionOnFlag, UnityAction<Cell> actionOnStart)
    {
        cell = cellToSet;

        cell.holderObject = gameObject;

        cell.OnChanged += Refresh;

        OnReveal = actionOnReveal;
        OnGameOver = actionOnGameOver;
        OnFlag = actionOnFlag;
        OnStart = actionOnStart;
        
        Refresh();
    }

    private void OnDisable()
    {
        if (cell != null) cell.OnChanged -= Refresh;
    }

    private void Refresh()
    {
        numbText.text = cell.Number > 0 ? cell.Number.ToString() : "";

        numbText.color = Colors.NumberColors[Mathf.Clamp(cell.Number - 1, 0, 7)];
        
        bombObj.SetActive(cell.HasBomb);
        numbText.gameObject.SetActive(!cell.HasBomb);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Click();
        else if (eventData.button == PointerEventData.InputButton.Right)
            Mark();
    }

    public void Click()
    {
        if (isMarked) return;
        hiddenObj.SetActive(false);
        revealedObj.SetActive(true);

        if (cell.Number == 0 && !isRevealed)
        {
            isRevealed = true;
            if(BoardGenerator.hasStarted) OnReveal.Invoke(cell);
        }
        if(cell.HasBomb) OnGameOver.Invoke();

        isRevealed = true;
        
        if(!BoardGenerator.hasStarted) OnStart.Invoke(cell);
    }

    public void Mark()
    {
        flagObj.SetActive(!flagObj.activeSelf);

        isMarked = flagObj.activeSelf;

        if (isMarked)
        {
            BoardGenerator.correctlyMarkedBombs.Add(cell);
            OnFlag.Invoke();
        }
        else BoardGenerator.correctlyMarkedBombs.Remove(cell);
    }
}

[System.Serializable]
public class Cell
{
    public int posX, posY;

    public GameObject holderObject;

    public int Number { get; private set; }

    public bool HasBomb { get; private set; }

    public UnityAction OnChanged;

    public Cell(int x, int y)
    {
        posX = x; 
        posY = y;
    }

    public void AddBomb()
    {
        HasBomb = true;
        
        OnChanged.Invoke();
    }

    public void SetNumber(int num)
    {
        Number = num;

        OnChanged.Invoke();
    }
}
