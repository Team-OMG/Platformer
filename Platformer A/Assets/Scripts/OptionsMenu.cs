using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public Toggle fullscreenTog, vsyncTog;

    public ResItem[] resolutions;

    private int selectedResolution;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyGraphics()
    {
        //fullscreen
        Screen.fullScreen = fullscreenTog.isOn;

        //vsync
        if (vsyncTog.isOn)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }

    [System.Serializable]
    public class ResItem
    {
        public int horizontal, vertical;
    }
}
