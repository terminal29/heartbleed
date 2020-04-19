using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    public Text coinCount;
    public Image coinImage;
    private GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        coinCount.text = string.Format("{0}", manager.coins);
        Vector3 eu = coinImage.rectTransform.localRotation.eulerAngles;
        eu.y = -70 + 140 * Mathf.Sin(Time.time / 2);
        coinImage.rectTransform.localRotation = Quaternion.Euler(eu);
    }
}
