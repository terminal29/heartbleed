using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkDisplay : MonoBehaviour
{
    public Perk biggerLight;
    public Perk directFire;
    public Perk quickReload;
    public Perk doubleFire;
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Dictionary<GameManager.PerkType, bool> enabledPerks = manager.GetPerks();
        biggerLight.SetEnabled(enabledPerks[GameManager.PerkType.BigLight]);
        directFire.SetEnabled(enabledPerks[GameManager.PerkType.DirectFire]);
        quickReload.SetEnabled(enabledPerks[GameManager.PerkType.QuickReload]);
        doubleFire.SetEnabled(enabledPerks[GameManager.PerkType.DoubleFire]);
    }
}
