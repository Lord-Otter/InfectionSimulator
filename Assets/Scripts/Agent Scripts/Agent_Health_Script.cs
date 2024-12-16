using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Health_Script : MonoBehaviour
{
    public float infectionResistance;
    private float resistance;
    private float baseResistance;
    public float fateRollCooldown;

    private SpriteRenderer spriteRenderer;

    public float oddsOfInfection = 0.1f;
    public float oddsOfSymptomatic = 0.7f;
    public float oddsOfCritical = 0.2f;
    public float oddsOfDead = 0.7f;

    public Agent_Movement_Script moveScript;

    private enum HealthState
    {
        Healthy,
        Immune,
        Compromised,
        Infected,
        Symptomatic,
        Critical,
        Dead
    }
    private HealthState currentHealth = HealthState.Healthy;
    // Start is called before the first frame update
    void Start()
    {
        moveScript = FindObjectOfType<Agent_Movement_Script>();

        infectionResistance = Mathf.Round(Random.Range(0.7f, 1.3f) * 100f) / 100f;
        resistance = 2 - infectionResistance;
        baseResistance = resistance;

        spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeState(HealthState.Healthy);
        InfectedContact();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Agent_Health_Script otherAgentScript = other.GetComponent<Agent_Health_Script>();
        if (other.CompareTag("Infection"))
        {
            if (currentHealth == HealthState.Healthy || currentHealth == HealthState.Compromised)
            {
                InfectedContact();
            }
        }
    }

//--States----------------------------------------------------
    private void ChangeState(HealthState newState)
    {
        StopAllCoroutines();
        //Debug.Log("Stopping Coroutines");
        currentHealth = newState;
        switch (currentHealth)
        {
            case HealthState.Healthy:
                ChangeColor("#00FF00");
                SetInfectionRadiusCollider(false, 1.5f);
                break;

            case HealthState.Immune:
                ChangeColor("#FFFFFF");
                SetInfectionRadiusCollider(false, 1.5f);
                break;

            case HealthState.Compromised:
                ChangeColor("#80FF00");
                break;

            case HealthState.Infected:
                ChangeColor("#FFFF00");
                SetInfectionRadiusCollider(true, 1.5f);

                StartCoroutine(InfectedStage());
                break;

            case HealthState.Symptomatic:
                ChangeColor("#FF8000");
                SetInfectionRadiusCollider(true, 2.5f);

                StartCoroutine(SymptomaticStage());
                break;

            case HealthState.Critical:
                ChangeColor("#FF0000");
                SetInfectionRadiusCollider(true, 2.5f);

                StartCoroutine(CriticalStage());
                break;

            case HealthState.Dead:
                ChangeColor("#000000");
                SetInfectionRadiusCollider(false, 2.5f);

                StopAllMoveCoroutines();
                break;
        }
    }

//--Change Color of Agent----------------------------------------------------------
    void ChangeColor(string hexColor)
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            spriteRenderer.color = newColor;

            Transform triangleTransform = transform.Find("Triangle");
            if (triangleTransform != null)
            {
                SpriteRenderer triangleRenderer = triangleTransform.GetComponent<SpriteRenderer>();
                if (triangleRenderer != null)
                {
                    triangleRenderer.color = newColor;
                }
            }
        }
    }

//--Collides with infection circle-----------------------------------------
    void InfectedContact()
    {
        float infectionRoll = Random.Range(0, 100);
        if (infectionRoll < (oddsOfInfection * resistance * 100) && currentHealth != HealthState.Immune)
        {
            ChangeState(HealthState.Infected);
        }
    }

//--Infection Stage behaviour--------------------------------------------------------------------
    IEnumerator InfectedStage()
    {
        int i = 0;
        while (true)
        {
            int escalationRoll = Random.Range(0, 10);
            if (escalationRoll < i)
            {
                int fateRoll = Random.Range(0, 100);
                if (fateRoll < oddsOfSymptomatic * resistance * 100)
                {
                    //Debug.Log("Infected Successfully Escalated");
                    ChangeState(HealthState.Symptomatic);
                }
                else
                {
                    //Debug.Log("Infected Was Defeated");
                    ChangeState(HealthState.Healthy);
                    
                }
            }

            //Debug.Log("Infected Failed to Escalate. Odds: " + i + "0%");
            ++i;
            yield return new WaitForSeconds(fateRollCooldown);
        }
    }

//--Symptomatic Stage Behaviour------------------------------------------------------------------------------
    IEnumerator SymptomaticStage()
    {
        int i = 0;
        while (true)
        {
            int escalationRoll = Random.Range(0, 10);
            if (escalationRoll < i) 
            {
                int fateRoll = Random.Range(0, 100);
                if (fateRoll < oddsOfCritical * resistance * 100)
                {
                    //Debug.Log("Symptomatic Sucessfully Escalated");
                    ChangeState(HealthState.Critical);                    
                }
                else
                {
                    //Debug.Log("Symptomatic Was Defeated");
                    ChangeState(HealthState.Healthy);
                }                
            }

            //Debug.Log("Symptomatic Failed to Escalate. Odds: " + i + "0%");
            ++i;
            yield return new WaitForSeconds(fateRollCooldown);
        }
    }

    //--Critical Stage Behaviour-----------------------------------------------------------------------------------
    IEnumerator CriticalStage()
    {
        int immunityRoll = Random.Range(0, 100);
        if (immunityRoll == 0)
        {
            ChangeState(HealthState.Immune);
        }

        int i = 0;
        while (true)
        {
            int escalationRoll = Random.Range(0, 10);
            if (escalationRoll < i)
            {
                int fateRoll = Random.Range(0, 100);
                if (fateRoll < oddsOfDead * resistance * 100)
                {
                    //Debug.Log("Critical Sucessfully Escalated");
                    ChangeState(HealthState.Dead);
                }
                else
                {
                    //Debug.Log("Critical Was Defeated");
                    ChangeState(HealthState.Healthy);
                }
            }

            //Debug.Log("Critical Failed to Escalate. Odds: " + i + "0%");
            ++i;
            yield return new WaitForSeconds(fateRollCooldown);
        }
    }

//--Change infection Circle-----------------------------------------------------------------------------------
    bool SetInfectionRadiusCollider(bool isEnabled, float newRadius)
    {
        Transform infectionRadiusTransform = transform.Find("Infection_Radius");
        if (infectionRadiusTransform != null)
        {
            CircleCollider2D circleCollider = infectionRadiusTransform.GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                circleCollider.enabled = isEnabled;

                if (newRadius >= 0)
                {
                    circleCollider.radius = newRadius;
                }

                return true;
            }
            else
            {
                Debug.LogWarning("CircleCollider2D component not found on Infection_Radius!");
            }
        }
        else
        {
            Debug.LogWarning("Infection_Radius child not found!");
        }

        return false;
    }

//--Stop All Coroutines in Agent_Movement_Script----------------------------------------------------
    public void StopAllMoveCoroutines()
    {
        moveScript.StopAllCoroutines();
    }
}
