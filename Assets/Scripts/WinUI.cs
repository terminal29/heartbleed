using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinUI : MonoBehaviour
{
    GameManager manager;

    public Action onRestartSelected;
    public Action onQuitSelected;
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
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void RestartPressed()
    {
        onRestartSelected?.Invoke();
    }

    public void QuitPressed()
    {
        onQuitSelected?.Invoke();
    }
}
