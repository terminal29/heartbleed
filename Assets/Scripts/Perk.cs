using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    public Material offMaterial;
    public Material onMaterial;

    public Image perkImage;
    public void SetEnabled(bool enabled)
    {
        if (enabled && perkImage.material != onMaterial)
        {
            perkImage.material = onMaterial;
        }

        if (!enabled && perkImage.material != offMaterial)
        {
            perkImage.material = offMaterial;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        perkImage.material = offMaterial;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
