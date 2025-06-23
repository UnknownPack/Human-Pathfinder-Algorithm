using UnityEngine;

public class AntEntity : MonoBehaviour
{
    public AntScriptableObject antConfig;
    private float health;
    private float attack;
    private float speed;
    private AlliedFaction faction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
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
    public void DamageAnt(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //ant would die here
            Debug.Log("IM DEAD");
        }

        
    }

    
}
