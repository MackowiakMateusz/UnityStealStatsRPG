using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class MainCharacter : MonoBehaviour
{
    
    public SpriteRenderer bodySpriteRenderer;
    public float sumOfResistancesAndDamages;
    public float numberOfTouchesOnUI;
    float physicalColorGreenRatio;
    float fireColorRedRatio;
    float iceColorBlueRatio;
    public float AngleRad;
    public float AngleDeg;
    public SaveSystem savesystem;
    public SaveSystem.SaveGameClass saveGame;
    public Rigidbody2D rigidbody2d;
    public GameObject arrow;
    public Camera camera;
    public Vector3 mouseposition;
    public Light2D light2d; // set manually
    // Start is called before the first frame update
    void Start()
    {
        
        bodySpriteRenderer = GetComponent<SpriteRenderer>();
        savesystem = GameObject.Find("EventSystem").GetComponent<SaveSystem>();
        saveGame = GameObject.Find("EventSystem").GetComponent<SaveSystem>().savegame;
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(saveGame.stats.xPosition, saveGame.stats.yPosition, saveGame.stats.zPosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //angle
        AngleRad = Mathf.Atan2(mouseposition.y - transform.position.y, mouseposition.x - transform.position.x);
        AngleDeg = (180 / Mathf.PI) * AngleRad;
        //end of angle
        arrow.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        camera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z-10);
        arrow.transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90);
        numberOfTouchesOnUI = 0;
        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (EventSystem.current.IsPointerOverGameObject(id))
            {
                numberOfTouchesOnUI = numberOfTouchesOnUI + 1;
            }
        }
        if (Input.touchCount-numberOfTouchesOnUI > 0)
        {
            arrow.SetActive(true);
            Touch touch = Input.GetTouch(Input.touchCount-1);
            mouseposition = Camera.main.ScreenToWorldPoint(touch.position);
            
            rigidbody2d.MovePosition(new Vector2(transform.position.x, transform.position.y) + new Vector2(Mathf.Cos(AngleDeg * Mathf.Deg2Rad) *saveGame.stats.speed, Mathf.Sin(AngleDeg * Mathf.Deg2Rad) * saveGame.stats.speed)*Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            arrow.SetActive(false);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    void Update()
    {
        
        updateSightRange();//updates sight range
        updateAttack();//updates attack
        sumOfResistancesAndDamages = saveGame.stats.iceDamage + saveGame.stats.iceDamageResistance +
            saveGame.stats.fireDamage + saveGame.stats.fireDamageResistance +
            saveGame.stats.physicalDamage + saveGame.stats.physicalDamageResistance;
        physicalColorGreenRatio = (saveGame.stats.physicalDamage + saveGame.stats.physicalDamageResistance) / sumOfResistancesAndDamages;
        fireColorRedRatio = (saveGame.stats.fireDamage + saveGame.stats.fireDamageResistance) / sumOfResistancesAndDamages;
        iceColorBlueRatio = (saveGame.stats.iceDamage + saveGame.stats.iceDamageResistance) / sumOfResistancesAndDamages;
        bodySpriteRenderer.color = new Color(fireColorRedRatio, physicalColorGreenRatio, iceColorBlueRatio,0.8f);
    }
    public void updateSightRange()
    {
        if(saveGame.stats.inDark==true)// dark sight
        {
            light2d.pointLightInnerRadius = saveGame.stats.darkSightRange - 1;
            light2d.pointLightOuterRadius = saveGame.stats.darkSightRange;
            // show enemy health bars if in range of sight
            foreach (GameObject instantiatedEnemy in savesystem.instantiatedEnemies)
            {
                if (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(transform.position.x, transform.position.y)) <= saveGame.stats.darkSightRange)
                {
                    instantiatedEnemy.GetComponent<Enemy>().amIInSight = true;
                    //radar indicator turns off
                    Destroy(instantiatedEnemy.GetComponent<Enemy>().myRadar);
                    instantiatedEnemy.GetComponent<Enemy>().myRadar = null;
                    //radar indicator turns off
                    instantiatedEnemy.transform.GetChild(0).gameObject.SetActive(true);
                    instantiatedEnemy.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    instantiatedEnemy.GetComponent<Enemy>().amIInSight = false;
                    //radar indicator turns on
                    if (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(transform.position.x, transform.position.y)) <= saveGame.stats.radarRange)
                    {
                        if (instantiatedEnemy.GetComponent<Enemy>().myRadar == null)
                        {
                            instantiatedEnemy.GetComponent<Enemy>().myRadar = Instantiate(savesystem.radarIndicatorPrefab, instantiatedEnemy.transform.position, Quaternion.Euler(0, 0, 0), instantiatedEnemy.transform);
                        }
                    }
                    else
                    {
                        Destroy(instantiatedEnemy.GetComponent<Enemy>().myRadar);
                        instantiatedEnemy.GetComponent<Enemy>().myRadar = null;
                    }
                    //radar indicator turns on
                    instantiatedEnemy.transform.GetChild(0).gameObject.SetActive(false);
                    instantiatedEnemy.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
        else//day sight
        {
            light2d.pointLightInnerRadius = saveGame.stats.daySightRange - 1;
            light2d.pointLightOuterRadius = saveGame.stats.daySightRange;
            // show enemy health bars if in range of sight
            foreach (GameObject instantiatedEnemy in savesystem.instantiatedEnemies)
            {
                if (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(transform.position.x, transform.position.y)) <= saveGame.stats.daySightRange)
                {
                    instantiatedEnemy.GetComponent<Enemy>().amIInSight = true;
                    //radar indicator turns off
                    Destroy(instantiatedEnemy.GetComponent<Enemy>().myRadar);
                    instantiatedEnemy.GetComponent<Enemy>().myRadar = null;
                    //radar indicator turns off
                    instantiatedEnemy.transform.GetChild(0).gameObject.SetActive(true);
                    instantiatedEnemy.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    instantiatedEnemy.GetComponent<Enemy>().amIInSight = false;
                    //radar indicator turns on
                    if (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(transform.position.x, transform.position.y)) <= saveGame.stats.radarRange)
                    {
                        if(instantiatedEnemy.GetComponent<Enemy>().myRadar==null)
                        {
                            instantiatedEnemy.GetComponent<Enemy>().myRadar = Instantiate(savesystem.radarIndicatorPrefab, instantiatedEnemy.transform.position, Quaternion.Euler(0, 0, 0), instantiatedEnemy.transform);
                        }
                    }
                    else
                    {
                        Destroy(instantiatedEnemy.GetComponent<Enemy>().myRadar);
                        instantiatedEnemy.GetComponent<Enemy>().myRadar = null;
                    }
                    //radar indicator turns on
                    instantiatedEnemy.transform.GetChild(0).gameObject.SetActive(false);
                    instantiatedEnemy.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }

    }
    public void updateAttack()
    {
        Enemy enemy;
        foreach (GameObject instantiatedEnemy in savesystem.instantiatedEnemies)
        {
            if (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(transform.position.x, transform.position.y)) <= saveGame.stats.attackRange)
            {
                
                enemy = instantiatedEnemy.GetComponent<Enemy>();
                if(saveGame.stats.iceDamage >= enemy.stats.iceDamageResistance)
                {
                    enemy.stats.health = enemy.stats.health - (saveGame.stats.iceDamage - enemy.stats.iceDamageResistance) * Time.deltaTime;
                }
                if (saveGame.stats.fireDamage >= enemy.stats.fireDamageResistance)
                {
                    enemy.stats.health = enemy.stats.health - (saveGame.stats.fireDamage - enemy.stats.fireDamageResistance) * Time.deltaTime;
                }
                if (saveGame.stats.physicalDamage >= enemy.stats.physicalDamageResistance)
                {
                    enemy.stats.health = enemy.stats.health - (saveGame.stats.physicalDamage - enemy.stats.physicalDamageResistance) * Time.deltaTime;
                }
                
                



            }
        }
    }
}
