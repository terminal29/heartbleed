using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RespawnUI : MonoBehaviour
{

    public Image pointerLeft;
    public Image pointerRight;

    public Text respawnText;
    public Action onRespawnSelected;

    public Text dieText;
    public Action onDieSelected;

    public Text quitText;
    public Action onQuitSelected;

    public Text restartText;
    public Action onRestartSelected;

    private int selectedIndex = 0;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    Text getElementForIndex(int index)
    {
        if (index == 0)
            return respawnText;
        if (index == 1)
            return dieText;
        if (index == 2)
            return restartText;
        if (index == 3)
            return quitText;
        return null;
    }

    Action getActionForIndex(int index)
    {
        if (index == 0)
            return onRespawnSelected;
        if (index == 1)
            return onDieSelected;
        if (index == 2)
            return onRestartSelected;
        if (index == 3)
            return onQuitSelected;
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = selectedIndex == 0 ? 0 : selectedIndex - 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = selectedIndex == 3 ? 3 : selectedIndex + 1;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            getActionForIndex(selectedIndex)?.Invoke();
        }

        Vector3 leftPos = pointerLeft.rectTransform.localPosition;
        leftPos.y = Mathf.Lerp(leftPos.y, getElementForIndex(selectedIndex).rectTransform.localPosition.y, Time.deltaTime * 10);
        pointerLeft.rectTransform.localPosition = leftPos;

        Vector3 rightPos = pointerRight.rectTransform.localPosition;
        rightPos.y = Mathf.Lerp(rightPos.y, getElementForIndex(selectedIndex).rectTransform.localPosition.y, Time.deltaTime * 10);
        pointerRight.rectTransform.localPosition = rightPos;
    }
}
