using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RespawnUI : MonoBehaviour
{

    public Font selectedFont;
    public Font unSelectedFont;

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

    public Text lightPerkUnderline;

    public Text speedReloadPerkUnderline;

    public Text doubleFirePerkUnderline;

    public Text directFirePerkUnderline;

    public Text extraHeartUnderlineText;


    GameManager manager;


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

    public void RestartHovered(bool isHovered)
    {
        if (isHovered)
        {
            restartText.font = selectedFont;
        }
        else
        {
            restartText.font = unSelectedFont;
        }
    }

    public void DiePressed()
    {
        onDieSelected?.Invoke();
    }

    public void DieHovered(bool isHovered)
    {
        if (isHovered)
        {
            dieText.font = selectedFont;
        }
        else
        {
            dieText.font = unSelectedFont;
        }
    }

    public void RespawnPressed()
    {
        onRespawnSelected?.Invoke();
    }

    public void RespawnHovered(bool isHovered)
    {
        if (isHovered)
        {
            respawnText.font = selectedFont;
        }
        else
        {
            respawnText.font = unSelectedFont;
        }
    }

    public void QuitPressed()
    {
        onQuitSelected?.Invoke();
    }

    public void QuitHovered(bool isHovered)
    {
        if (isHovered)
        {
            quitText.font = selectedFont;
        }
        else
        {
            quitText.font = unSelectedFont;
        }
    }

    public void DirectFireHovered(bool isHovered)
    {
        if (isHovered)
        {
            if (!manager.GetPerks()[GameManager.PerkType.DirectFire]) directFirePerkUnderline.font = selectedFont;
        }
        else
        {
            directFirePerkUnderline.font = unSelectedFont;
        }
    }

    public void DirectFirePressed()
    {
        if (manager.GetPerks()[GameManager.PerkType.DirectFire])
            return;
        if (manager.coins >= 50)
        {
            manager.coins -= 50;
            manager.SetPerkStatus(GameManager.PerkType.DirectFire, true);
            directFirePerkUnderline.font = unSelectedFont;
        }
    }

    public void SpeedReloadHovered(bool isHovered)
    {
        if (isHovered)
        {
            if (!manager.GetPerks()[GameManager.PerkType.QuickReload]) speedReloadPerkUnderline.font = selectedFont;
        }
        else
        {
            speedReloadPerkUnderline.font = unSelectedFont;
        }
    }

    public void SpeedReloadPressed()
    {
        if (manager.GetPerks()[GameManager.PerkType.QuickReload])
            return;
        if (manager.coins >= 40)
        {
            manager.coins -= 40;
            manager.SetPerkStatus(GameManager.PerkType.QuickReload, true);
            speedReloadPerkUnderline.font = unSelectedFont;
        }
    }

    public void BigLightHovered(bool isHovered)
    {
        if (isHovered)
        {
            if (!manager.GetPerks()[GameManager.PerkType.BigLight]) lightPerkUnderline.font = selectedFont;
        }
        else
        {
            lightPerkUnderline.font = unSelectedFont;
        }
    }

    public void BigLightPressed()
    {
        if (manager.GetPerks()[GameManager.PerkType.BigLight])
            return;
        if (manager.coins >= 10)
        {
            manager.coins -= 10;
            manager.SetPerkStatus(GameManager.PerkType.BigLight, true);
            lightPerkUnderline.font = unSelectedFont;
        }
    }

    public void DoubleFireHovered(bool isHovered)
    {
        if (isHovered)
        {
            if (!manager.GetPerks()[GameManager.PerkType.DoubleFire]) doubleFirePerkUnderline.font = selectedFont;
        }
        else
        {
            doubleFirePerkUnderline.font = unSelectedFont;
        }
    }

    public void DoubleFirePressed()
    {
        if (manager.GetPerks()[GameManager.PerkType.DoubleFire])
            return;
        if (manager.coins >= 30)
        {
            manager.coins -= 30;
            manager.SetPerkStatus(GameManager.PerkType.DoubleFire, true);
            doubleFirePerkUnderline.font = unSelectedFont;
        }
    }

    public void ExtraHeartHovered(bool isHovered)
    {
        if (isHovered)
        {
            if (manager.GetHeart().GetHealth() != manager.GetHeart().GetMaxHealth()) extraHeartUnderlineText.font = selectedFont;
        }
        else
        {
            extraHeartUnderlineText.font = unSelectedFont;
        }
    }

    public void ExtraHeartPressed()
    {
        if (manager.GetHeart().GetHealth() == manager.GetHeart().GetMaxHealth())
            return;
        if (manager.coins >= 50)
        {
            manager.coins -= 50;
            manager.GetHeart().SetHealth(manager.GetHeart().GetHealth() + 1);
            if (manager.GetHeart().GetHealth() == manager.GetHeart().GetMaxHealth())
                doubleFirePerkUnderline.font = unSelectedFont;
        }
    }



}
