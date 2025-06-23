using System;
using System.Collections;
using UnityEngine;

public class AntFightEvent : MonoBehaviour
{
    public AntEntity ants1, ants2;

    public Coroutine[] battles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKeyUp(KeyCode.E))
        {
            battles = new Coroutine[2];
            if (ants1.getSpeed() == ants2.getSpeed())
            {
                Debug.Log("Both ants have same speed, randomising turns");
              //  StartCoroutine(sameSpeed(ant1, ant2));
            }
            else
            {
                Debug.Log("Ants have different speed");
                battles[0] = StartCoroutine(antBattle(ants1, ants2));
                battles[1] = StartCoroutine(antBattle(ants2, ants1));
            }

        }*/
    }

    public void StartFight(AntEntity ant1, AntEntity ant2)
    {
        battles = new Coroutine[2];
        if (ant1.getSpeed() == ant2.getSpeed())
        {
            Debug.Log("Both ants have same speed, randomising turns");
            StartCoroutine(sameSpeed(ant1, ant2));
        }
        else
        {
            Debug.Log("Ants have different speed");
            battles[0] = StartCoroutine(antBattle(ant1, ant2));
            battles[1] = StartCoroutine(antBattle(ant1, ant2));
        }

    }
    public IEnumerator sameSpeed(AntEntity ant1, AntEntity ant2)
    {
        while (ant1.getHealth() > 0 && ant2.getHealth() > 0)
        {
            yield return new WaitForSeconds(1 / ant2.getSpeed());
            float random = (int)Mathf.Floor(UnityEngine.Random.Range(0, 2));
            if (ant1.entityEngagedInBattle != ant2)
            {
                if (ant2.entityEngagedInBattle == ant1)
                {
                    StartCoroutine(antBattleSame(ant2,ant1));
                }
            }
            else
            if (random == 0)
            {
                StartCoroutine(antBattleSame(ant1, ant2));
            }
            else
            {
                StartCoroutine(antBattleSame(ant2, ant1));
            }
            yield return null;
            if (random == 0)
            {
                StartCoroutine(antBattleSame(ant2, ant1));
            }
            else
            {
                StartCoroutine(antBattleSame(ant1, ant2));
            }
            yield return null;
        }
        yield return null;
    }

    public IEnumerator antBattle(AntEntity ant1, AntEntity ant2)
    {
       while (ant1.getHealth() > 0 && ant2.getHealth() > 0)
        {
            yield return new WaitForSeconds(1 / ant1.getSpeed());
            if (ant1.entityEngagedInBattle != ant2)
            {
                Debug.Log("We aint fighting this one");
            }
            else if (ant2.DamageAnt(ant1.getAttack()))
            {
                Debug.Log("Ant has been defeated" + ant2.gameObject.name);
                ant1.inBattle = false;
                ant2.inBattle = false;
                ant1.entityEngagedInBattle = null;
                endBattles();
                
                }
            yield return null;
        }
        yield return null;
    }

    public IEnumerator antBattleSame(AntEntity ant1, AntEntity ant2)
    {
        
        if (ant2.DamageAnt(ant1.getAttack()))
        {
            Debug.Log("Ant has been defeated" + ant2.gameObject.name);
            endBattles();
        }
        yield return null;
    }

    public void endBattles()
    {
        StopAllCoroutines();
        // if(ants1.)

    }


    

}
