using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Health_Script : MonoBehaviour
{
    public float resistance;
    public float defaultResistance;

    private SpriteRenderer spriteRenderer;

    public float oddsOfInfection = 0.1f;

    private enum HealthState
    {
        Healthy,
        Immune,
        Compromised,
        Infected,
        Symptomatic,
        Dead
    }
    private HealthState currentHealth = HealthState.Healthy;
    // Start is called before the first frame update
    void Start()
    {
        resistance = 1f + Random.Range(-0.3f, 0.3f);
        defaultResistance = resistance;

        spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeHealth(HealthState.Healthy);
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

    //Change Color of Agent
    void ChangeColor(string hexColor)
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            spriteRenderer.color = newColor;
        }
    }

    void InfectedContact()
    {
        float infectionRoll = Random.Range(0, 99);
        if (infectionRoll < (oddsOfInfection * resistance * 100))
        {
            ChangeHealth(HealthState.Infected);
        }
    }

    bool SetInfectionRadiusCollider(bool isEnabled, float newRadius)
    {
        // Find the child named Infection_Radius
        Transform infectionRadiusTransform = transform.Find("Infection_Radius");
        if (infectionRadiusTransform != null)
        {
            // Get the CircleCollider2D component on Infection_Radius
            CircleCollider2D circleCollider = infectionRadiusTransform.GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                // Enable or disable the collider
                circleCollider.enabled = isEnabled;

                // If a valid new radius is provided, adjust the radius
                if (newRadius >= 0)
                {
                    circleCollider.radius = newRadius;
                }

                // Return true if the operation was successful
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

        // Return false if the operation failed
        return false;
    }

    private void ChangeHealth(HealthState newState)
    {
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
                ChangeColor("#FFFF00");
                break;

            case HealthState.Infected:
                ChangeColor("#FF8000");
                SetInfectionRadiusCollider(true, 1.5f);
                break;

            case HealthState.Symptomatic:
                ChangeColor("#0000FF");
                //SetInfectionRadiusCollider(true, 2.5f);
                break;

            case HealthState.Dead:
                ChangeColor("#000000");
                break;
        }
    }
}
