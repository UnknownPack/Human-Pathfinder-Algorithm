using TMPro;
using UnityEngine;

public class AntEntity : MonoBehaviour
{
    public AntScriptableObject antConfig;
    public SpriteRenderer sprite;
    private float health;
    private float attack;
    private float speed;
    private AlliedFaction faction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetStats();
    }
    private void SetStats()
    {
        health = antConfig.health;
        attack = antConfig.attack;
        speed = antConfig.speed;
    }

    void Update() //Temp update to test some stuff
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            DamageAnt(1);
            Debug.Log("Damaged ant for 1, this is its current health " + health);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            SetStats();
        }

    }


    public float getSpeed()
    {
        //float newSpeed = speed * faction.speedMult
        return speed;
    }
    public float getHealth()
    {
        //float newHealth = health * faction.healthMult
        return health;
    }
    public float getAttack()
    {
        //float newAttack = attack * faction.attackMult
        return attack;
    }
    public BaseFaction getFaction()
    {
        return faction;
    }


    public bool DamageAnt(float damage)
    {
        health -= damage;
        sprite.color = new Color(1, 0, 0);
        Invoke("changeColourBack", 0.1f);
        if (health <= 0)
        {
            //ant would die here
            Debug.Log("IM DEAD");
            return true;
        }
        else
        {
            return false;
        }



    }
    public void changeColourBack()
    {
        sprite.color = new Color(1, 1, 1);
    }

    
}
