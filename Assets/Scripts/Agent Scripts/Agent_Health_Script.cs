using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Health_Script : MonoBehaviour
{
    public float resistance;
    public float defaultResistance;

    private SpriteRenderer spriteRenderer;

    public float oddsOfInfection = 0.1f;
    public float oddsOfSymptomatic = 0.7f;
    public float oddsOfCritical = 0.2f;
    public float oddsOfDead = 0.7f;

    private enum HealthState
    {
        Healthy,
        Immune,
        Resistant,
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
        resistance = 1f + Random.Range(-0.3f, 0.3f);
        defaultResistance = resistance;

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

        if (other.CompareTag("Agent"))
        {
            //Collision with eachother.
        }
    }

//--States----------------------------------------------------
    private void ChangeState(HealthState newState)
    {
        StopAllCoroutines();
        Debug.Log("Stopping Coroutines");
        currentHealth = newState;
        switch (currentHealth)
        {
            case HealthState.Healthy:
                ChangeColor("#00FF00");
                SetInfectionRadiusCollider(false, 1.5f);
                break;

            case HealthState.Immune:
                ChangeColor("#FFFFFF");
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
                ChangeColor("0000FF");
                SetInfectionRadiusCollider(true, 2.5f);
                break;

            case HealthState.Dead:
                ChangeColor("#000000");
                break;
        }
    }

    //--Change Color of Agent----------------------------------------------------------
    void ChangeColor(string hexColor)
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            // Change color of the parent object (spriteRenderer)
            spriteRenderer.color = newColor;

            // Find the child object "Triangle" and change its color
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
        float infectionRoll = Random.Range(0, 99);
        if (infectionRoll < (oddsOfInfection * resistance * 100) && currentHealth != HealthState.Immune)
        {
            ChangeState(HealthState.Infected);
        }
    }

//--Infection Stage behaviour--------------------------------------------------------------------
    IEnumerator InfectedStage()
    {
        int i = 10;
        while (true)
        {
            int escalationRoll = Random.Range(0, 99);
            if (escalationRoll < i * resistance)
            {
                int fateRoll = Random.Range(0, 99);
                if (fateRoll < oddsOfSymptomatic * resistance * 100)
                {
                    Debug.Log("Infection Successfully Escalated");
                    ChangeState(HealthState.Symptomatic);
                }
                else
                {
                    Debug.Log("Infection Failed To Escalate");
                    ChangeState(HealthState.Healthy);
                    
                }
            }
            Debug.Log("Failed to Escalate. i = " + i);
            i += 10;
            yield return new WaitForSeconds(5);
        }
    }

//--Symptomatic Stage Behaviour------------------------------------------------------------------------------
    IEnumerator SymptomaticStage()
    {
        while (true)
        {
            int immunityRoll = Random.Range(0, 99);
            if(immunityRoll == 99)
            {
                ChangeState (HealthState.Immune);
            }
            else
            {
                int fateRoll = Random.Range(0, 99);
                if(fateRoll < oddsOfCritical * resistance * 100)
                {
                    ChangeState(HealthState.Critical);
                }
                else
                {
                    ChangeState(HealthState.Healthy);
                }
            }

            yield return new WaitForSeconds(5);
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

    
}
