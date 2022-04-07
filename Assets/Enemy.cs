using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject myRadar;
    public bool amIInSight;
    [System.Serializable]
    public class EnemyStats
    {
        public int IdOfSpawnerFromWhichItWasSpawned;
        public string name;
        public float xpValue;
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
        public bool triggered;
        public int id;
    }
    public EnemyStats stats;
    [System.Serializable]
    public class LootedStats
    {
        public float xpValue;
        public float attackRange;
        public bool inDark;
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
    public LootedStats lootedStats;
    //public GameObject healthBarObject;
    public Image healthBarImage;
    // Start is called before the first frame update
    void Start()
    {
        //assign gameobejct manually
        //healthBarObject = transform.Find("HealthBar").gameObject;
        //healthBarImage = healthBarObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBarImage.fillAmount = stats.health / stats.maxHealth;
        // kill/destroy gameobject
        if(stats.health<=0)
        {
            
            SaveSystem savesystem = GameObject.Find("EventSystem").GetComponent<SaveSystem>();
            foreach(GameObject spawner in savesystem.instantiatedSpawners)
            {
                if(spawner.GetComponent<Spawner>().stats.ID==stats.IdOfSpawnerFromWhichItWasSpawned)
                {
                    spawner.GetComponent<Spawner>().stats.numberOfAlreadySpawnedEnemies -= 1;
                }
            }
            // add stats to player
            savesystem.savegame.stats.experience = savesystem.savegame.stats.experience + lootedStats.xpValue;
            savesystem.savegame.stats.attackRange = savesystem.savegame.stats.attackRange + lootedStats.attackRange;
            savesystem.savegame.stats.speed = savesystem.savegame.stats.speed + lootedStats.speed;
            savesystem.savegame.stats.health = savesystem.savegame.stats.health + lootedStats.health;
            savesystem.savegame.stats.maxHealth = savesystem.savegame.stats.maxHealth + lootedStats.maxHealth;
            savesystem.savegame.stats.physicalDamageResistance = savesystem.savegame.stats.physicalDamageResistance + lootedStats.physicalDamageResistance;
            savesystem.savegame.stats.physicalDamage = savesystem.savegame.stats.physicalDamage + lootedStats.physicalDamage;
            savesystem.savegame.stats.fireDamageResistance = savesystem.savegame.stats.fireDamageResistance + lootedStats.fireDamageResistance;
            savesystem.savegame.stats.fireDamage = savesystem.savegame.stats.fireDamage + lootedStats.fireDamage;
            savesystem.savegame.stats.iceDamageResistance = savesystem.savegame.stats.iceDamageResistance + lootedStats.iceDamageResistance;
            savesystem.savegame.stats.iceDamage = savesystem.savegame.stats.iceDamage + lootedStats.iceDamage;
            savesystem.savegame.stats.darkSightRange = savesystem.savegame.stats.darkSightRange + lootedStats.darkSightRange;
            savesystem.savegame.stats.daySightRange = savesystem.savegame.stats.daySightRange + lootedStats.daySightRange;
            savesystem.savegame.stats.radarRange = savesystem.savegame.stats.radarRange + lootedStats.radarRange;
            savesystem.gainedStatsFromEnemyText.text = "Gained:" + "\n";
            if(lootedStats.xpValue!=0)
            {
                savesystem.gainedStatsFromEnemyText.text= savesystem.gainedStatsFromEnemyText.text+ "XP:" +lootedStats.xpValue+ "\n";
            }
            if (lootedStats.attackRange != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Attack Range:" + lootedStats.attackRange + "\n";
            }
            if (lootedStats.speed != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Movement Speed:" + lootedStats.speed + "\n";
            }
            if (lootedStats.maxHealth != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Max Health:" + lootedStats.xpValue + "\n";
            }
            if (lootedStats.physicalDamageResistance != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Physical Damage Resistance:" + lootedStats.physicalDamageResistance + "\n";
            }
            if (lootedStats.physicalDamage != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Physical Damage:" + lootedStats.physicalDamage + "\n";
            }
            if (lootedStats.fireDamageResistance != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Fire Damage Resistance:" + lootedStats.fireDamageResistance + "\n";
            }
            if (lootedStats.fireDamage != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Fire Damage:" + lootedStats.fireDamage + "\n";
            }
            if (lootedStats.iceDamageResistance != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Ice Damage Resistance:" + lootedStats.iceDamageResistance + "\n";
            }
            if (lootedStats.iceDamage != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Ice Damage:" + lootedStats.iceDamage + "\n";
            }
            if (lootedStats.darkSightRange != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Dark Sight Range:" + lootedStats.darkSightRange + "\n";
            }
            if (lootedStats.daySightRange != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Day Sight Range:" + lootedStats.daySightRange + "\n";
            }
            if (lootedStats.radarRange != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "Radar Range:" + lootedStats.radarRange + "\n";
            }
            if (lootedStats.health != 0)
            {
                savesystem.gainedStatsFromEnemyText.text = savesystem.gainedStatsFromEnemyText.text + "AND healed by:" + lootedStats.health + "\n";
            }
            // add stats to player

            foreach (GameObject enemyGameobject in savesystem.instantiatedEnemies)
            {
                if(enemyGameobject==this.gameObject)
                {
                    savesystem.instantiatedEnemies.Remove(enemyGameobject);
                    break;
                }
            }
            foreach (Enemy.EnemyStats enemy in savesystem.savegame.enemies)
            {
                if (enemy == stats)
                {
                    savesystem.savegame.enemies.Remove(enemy);
                    break;
                }
            }
            GameObject.Destroy(this.gameObject);
        }
        // kill/destroy gameobject
    }
}
