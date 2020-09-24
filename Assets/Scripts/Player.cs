using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class Player : MonoBehaviour {
    public string Name;
    
    public int Exp;
    
    public int Hp;
    public int Shield;
    public GameObject healthBar1;
    public GameObject shieldBar1;
    public GameObject player;

    LevelSystem levelSystem;
    ShieldBarScript shieldBar;
    HealthBar healthBar;
    

    public void Start() {
        healthBar1 = GameObject.Find("HealthBarColor");
        shieldBar1 = GameObject.Find("ShieldBarColor");
        player = GameObject.Find("Player");

        levelSystem = player.GetComponent<LevelSystem>();
        shieldBar = shieldBar1.GetComponent<ShieldBarScript>();
        healthBar = healthBar1.GetComponent<HealthBar>();

    }
    public void Update() {

        if (Input.GetKeyDown(KeyCode.S)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();


    }
    
    public void Save() {
       
        Exp = levelSystem.xp;

        Hp = healthBar.currentHealth;
        Shield = shieldBar.currentShield;
      

        JSONObject playerJson = new JSONObject();
        playerJson.Add("Name", Name);
       
        playerJson.Add("Exp", Exp);
        playerJson.Add("HP", Hp);
        playerJson.Add("Shield", Shield);

        JSONArray position = new JSONArray();
        position.Add(transform.position.x);
        position.Add(transform.position.y);
        position.Add(transform.position.z);
        playerJson.Add("Position", position);

        string path = Application.persistentDataPath + "PlayerSave.json";
        File.WriteAllText(path, playerJson.ToString());

    }

    void Load() {
        string path = Application.persistentDataPath + "PlayerSave.json";
        string jsonString = File.ReadAllText(path);
        JSONObject playerJson = (JSONObject)JSON.Parse(jsonString);
        Name = playerJson["Name"];
        
        Exp = playerJson["Exp"];
        Hp = playerJson["HP"];
        Shield = playerJson["Shield"];

        transform.position = new Vector3(
            playerJson["Position"].AsArray[0],
            playerJson["Position"].AsArray[1],
            playerJson["Position"].AsArray[2]
            );

        
        healthBar.currentHealth = Hp;
        shieldBar.currentShield = Shield;
        levelSystem.xp = Exp;
      
       


    }
}
