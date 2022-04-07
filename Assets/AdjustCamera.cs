using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCamera : MonoBehaviour
{
    public SaveSystem savesystem;
    public Camera camera;
    public float tilesAboveSightRange;
    // Start is called before the first frame update
    void Start()
    {
        savesystem = GameObject.Find("EventSystem").GetComponent<SaveSystem>();
        camera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(savesystem.savegame.stats.inDark==false)
        {
            camera.orthographicSize = savesystem.savegame.stats.daySightRange + tilesAboveSightRange;
        }
        else
        {
            camera.orthographicSize = savesystem.savegame.stats.darkSightRange + tilesAboveSightRange;
        }
        
    }
    public void zoomIn(float value)
    {
        tilesAboveSightRange = tilesAboveSightRange - value;
    }
    public void zoomOut(float value)
    {
        tilesAboveSightRange = tilesAboveSightRange + value;
    }
}
