using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveSystem : MonoBehaviour
{
    public GameObject radarIndicatorPrefab;
    //textMEshPro
    public TextMeshProUGUI playerStatsTextObjectButtonText;
    public GameObject playerStatsTextObject;
    public TextMeshProUGUI gainedStatsTextObjectButtonText;
    public GameObject gainedStatsTextObject;
    public TextMeshProUGUI enemyStatsTextObjectButtonText;
    public GameObject enemyStatsTextObject;

    
    public void togglePlayerStatsButtonFunction()
    {
        if (playerStatsTextObject.active == true)
        {
            playerStatsTextObject.SetActive(false);
            playerStatsTextObjectButtonText.text = playerStatsTextObjectButtonText.text + " (OFF) ";
        }
        else
        {
            playerStatsTextObject.SetActive(true);
            playerStatsTextObjectButtonText.text = playerStatsTextObjectButtonText.text.Substring(0, playerStatsTextObjectButtonText.text.Length - 7);
        }
    }
    public void toggleGainedStatsButtonFunction()
    {
        if (gainedStatsTextObject.active == true)
        {
            gainedStatsTextObject.SetActive(false);
            gainedStatsTextObjectButtonText.text = gainedStatsTextObjectButtonText.text + " (OFF) ";
        }
        else
        {
            gainedStatsTextObject.SetActive(true);
            gainedStatsTextObjectButtonText.text = gainedStatsTextObjectButtonText.text.Substring(0, gainedStatsTextObjectButtonText.text.Length - 7);
        }
    }
    public void toggleEnemyStatsButtonFunction()
    {
        if(enemyStatsTextObject.active==true)
        {
            enemyStatsTextObject.SetActive(false);
            enemyStatsTextObjectButtonText.text = enemyStatsTextObjectButtonText.text + " (OFF) ";

        }
        else
        {
            enemyStatsTextObject.SetActive(true);
            
            enemyStatsTextObjectButtonText.text = enemyStatsTextObjectButtonText.text.Substring(0,enemyStatsTextObjectButtonText.text.Length - 7);
        }
    }
    public TextMeshProUGUI playerStatsText;
    public TextMeshProUGUI enemyStatsText;
    public TextMeshProUGUI gainedStatsFromEnemyText;
    public GameObject closestSeenEnemy;
    public Enemy.EnemyStats closestSeenEnemyStats;
    public float closestEnemyDistanceFromPlayer;
    public GameObject playerGameobject;
    //textMEshPro

    public bool paused;
    public List<GameObject> EnemyPrefabs;
    public List<GameObject> instantiatedEnemies;
    public List<GameObject> instantiatedSpawners;
    [System.Serializable]
    public class SaveGameClass
    {
        public float timeSinceGameStart;
        public Stats stats;
        public List<Enemy.EnemyStats> enemies;
    }
    [System.Serializable]
    public class Stats
    {
        public float experience;
        public float attackRange;
        public bool inDark;
        public float xPosition;
        public float yPosition;
        public float zPosition;
        public float speed;
        public float health;
        public float maxHealth;
        public float physicalDamageResistance;
        public float physicalDamage;
        public float fireDamageResistance;
        public float fireDamage;
        public float iceDamageResistance;
        public float iceDamage;
        public float darkSightRange;
        public float daySightRange;
        public float radarRange;
    }
    public SaveGameClass savegame;
    public void Save()
    {

        savegame.stats.xPosition = GameObject.FindGameObjectWithTag("Player").transform.position.x;
        savegame.stats.yPosition = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        savegame.stats.zPosition = GameObject.FindGameObjectWithTag("Player").transform.position.z;
        foreach(GameObject instantiatedEnemy in instantiatedEnemies)
        {
            instantiatedEnemy.GetComponent<Enemy>().stats.xPosition = instantiatedEnemy.transform.position.x;
            instantiatedEnemy.GetComponent<Enemy>().stats.yPosition = instantiatedEnemy.transform.position.y;
            instantiatedEnemy.GetComponent<Enemy>().stats.zPosition = instantiatedEnemy.transform.position.z;
        }
        PlayerPrefs.SetString("save", Helper.Serialize<SaveGameClass>(savegame));
        Debug.Log(Application.dataPath + "/SaveGame.xml" + " -> SAVED/ZAPISANO");
    }
    public void Load()//Reference here all functions from enemies etc. that will be loaded after Load() is called
    {
        if (PlayerPrefs.HasKey("save"))
        {
            savegame = Helper.Deserialize<SaveGameClass>(PlayerPrefs.GetString("save"));
            Debug.Log(Application.dataPath + "/SaveGame.xml" + " -> LOADED/WCZYTANO");
        }
        else
        {
            Save();
            Debug.Log("No save file yet.");
        }
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(savegame.stats.xPosition, savegame.stats.yPosition, savegame.stats.zPosition);
        //instantiate enemies
        foreach(GameObject instantiatedEnemy in instantiatedEnemies)
        {
            Destroy(instantiatedEnemy);
        }
        instantiatedEnemies = new List<GameObject>();
        GameObject newEnemy;
        foreach (Enemy.EnemyStats enemy in savegame.enemies)
        {
            newEnemy = Instantiate(EnemyPrefabs[enemy.id], new Vector3(enemy.xPosition, enemy.yPosition, enemy.zPosition), Quaternion.Euler(0, 0, 0));
            instantiatedEnemies.Add(newEnemy);
            newEnemy.GetComponent<Enemy>().stats = enemy;
        }
        // end of instantiate enemies
    }
    // Start is called before the first frame update
    void Awake()
    {
        Load();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updatePlayerStatsText();
        updateEnemyStatsText();
        if(paused==false)
        {
           savegame.timeSinceGameStart = savegame.timeSinceGameStart + Time.deltaTime;
        }
    }

    void OnApplicationPause(bool pause)
    {
        Save();
    }

    void OnApplicationQuit()
    {
        Save();
    }
    public void updatePlayerStatsText()
    {
        playerStatsText.text =
            "Health: " + savegame.stats.health + "/" + savegame.stats.maxHealth + "\n" +
            "Attack Range: " + savegame.stats.attackRange+"\n"+
            "Movement Speed: " + savegame.stats.speed + "\n" +
            "Physical Damage: " + savegame.stats.physicalDamage + "\n" +
            "Physical Damage Resistance: " + savegame.stats.physicalDamageResistance + "\n" +
            "Fire Damage: " + savegame.stats.fireDamage + "\n" +
            "Fire Damage Resistance: " + savegame.stats.fireDamageResistance + "\n" +
            "Ice Damage: " + savegame.stats.iceDamage + "\n" +
            "Ice Damage Resistance: " + savegame.stats.iceDamageResistance + "\n" +
            "Sight Range In Day: " + savegame.stats.daySightRange + "\n" +
            "Sight Range In Dark: " + savegame.stats.darkSightRange + "\n" +
            "Radar Range: " + savegame.stats.radarRange+ "\n" +
            "Experience: " + savegame.stats.experience;
}
    public void updateEnemyStatsText()
    {
        closestEnemyDistanceFromPlayer = 1000000;
        if (savegame.stats.inDark==true)
        {
            
            foreach (GameObject instantiatedEnemy in instantiatedEnemies)
            {
                if (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(playerGameobject.transform.position.x, playerGameobject.transform.position.y)) <= savegame.stats.darkSightRange)
                {
                    if(closestEnemyDistanceFromPlayer>= (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(playerGameobject.transform.position.x, playerGameobject.transform.position.y))))
                    {
                        closestEnemyDistanceFromPlayer = (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(playerGameobject.transform.position.x, playerGameobject.transform.position.y)));
                        closestSeenEnemy = instantiatedEnemy;
                    }
                }
            }
        }
        else
        {
            foreach (GameObject instantiatedEnemy in instantiatedEnemies)
            {
                if (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(playerGameobject.transform.position.x, playerGameobject.transform.position.y)) <= savegame.stats.daySightRange)
                {
                    if (closestEnemyDistanceFromPlayer >= (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(playerGameobject.transform.position.x, playerGameobject.transform.position.y))))
                    {
                        closestEnemyDistanceFromPlayer = (Vector2.Distance(new Vector2(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y), new Vector2(playerGameobject.transform.position.x, playerGameobject.transform.position.y)));
                        closestSeenEnemy = instantiatedEnemy;
                    }
                }
            }
        }
        if(closestSeenEnemy!=null)
        {
            closestSeenEnemyStats = closestSeenEnemy.GetComponent<Enemy>().stats;
            if(closestSeenEnemyStats.health<=0)
            {
                enemyStatsText.text = closestSeenEnemyStats.name+ " is dead.";
            }
            else
            {
                enemyStatsText.text =
                    "Name: " + closestSeenEnemyStats.name + "\n" +
                "Health: " + closestSeenEnemyStats.health + "/" + closestSeenEnemyStats.maxHealth + "\n" +
                "Attack Range: " + closestSeenEnemyStats.attackRange + "\n" +
                "Movement Speed: " + closestSeenEnemyStats.speed + "\n" +
                "Physical Damage: " + closestSeenEnemyStats.physicalDamage + "\n" +
                "Physical Damage Resistance: " + closestSeenEnemyStats.physicalDamageResistance + "\n" +
                "Fire Damage: " + closestSeenEnemyStats.fireDamage + "\n" +
                "Fire Damage Resistance: " + closestSeenEnemyStats.fireDamageResistance + "\n" +
                "Ice Damage: " + closestSeenEnemyStats.iceDamage + "\n" +
                "Ice Damage Resistance: " + closestSeenEnemyStats.iceDamageResistance + "\n" +
                "Sight Range In Day: " + closestSeenEnemyStats.daySightRange + "\n" +
                "Sight Range In Dark: " + closestSeenEnemyStats.darkSightRange + "\n"+
                "XpValue: " + closestSeenEnemyStats.xpValue + "\n";
            }
        }
    }
}
