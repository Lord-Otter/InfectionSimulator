using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spawn_Script : MonoBehaviour
{
    public int totalPopulation;
    public float spawnDelay;
    public float counterUpdateRate;

    public GameObject Agent;

    //UI
    private int population = 1;
    public int aliveCounter;
    public int immuneCounter;
    public int healthyCounter;
    public int infectedCounter;
    public int symptomaticCounter;
    public int criticalCounter;
    public int deadCounter;

    private string startPopulationText;
    public GameObject startPopulation;
    private TextMeshProUGUI populationComponent;

    private string aliveText;
    public GameObject alive;
    private TextMeshProUGUI aliveComponent;

    private string immuneText;
    public GameObject immune;
    private TextMeshProUGUI immuneComponent;
    
    private string healthyText;
    public GameObject healthy;
    private TextMeshProUGUI healthyComponent;
    
    private string infectedText;
    public GameObject infected;
    private TextMeshProUGUI infectedComponent;
    
    private string symptomaticText;
    public GameObject symptomatic;
    private TextMeshProUGUI symptomaticComponent;
    
    private string criticalText;
    public GameObject critical;
    private TextMeshProUGUI criticalComponent;
    
    private string deadText;
    public GameObject dead;
    private TextMeshProUGUI deadComponent;

    // Start is called before the first frame update
    void Start()
    {
        populationComponent = startPopulation.GetComponent<TextMeshProUGUI>();
        aliveComponent = alive.GetComponent<TextMeshProUGUI>();
        immuneComponent = immune.GetComponent<TextMeshProUGUI>();
        healthyComponent = healthy.GetComponent<TextMeshProUGUI>();
        infectedComponent = infected.GetComponent<TextMeshProUGUI>();
        symptomaticComponent = symptomatic.GetComponent<TextMeshProUGUI>();
        criticalComponent = critical.GetComponent<TextMeshProUGUI>();
        deadComponent = dead.GetComponent<TextMeshProUGUI>();

        StartCoroutine(UpdateTexts());
        StartCoroutine(SpawnAgent());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator UpdateTexts()
    {
        populationComponent.text = "Starting Population: " + totalPopulation.ToString();

        while (true)
        {
            float alivePercent = ((aliveCounter * 1000) / totalPopulation) / 10f;
            aliveComponent.text = aliveCounter.ToString() + " (" + alivePercent + "%)";

            if (aliveCounter != 0)
            {
                float immunePercent = ((immuneCounter * 1000) / aliveCounter) / 10f;
                immuneComponent.text = immuneCounter.ToString() + " (" + immunePercent + "%)";
            }

            if (aliveCounter != 0)
            {
                float healthyPercent = ((healthyCounter * 1000) / aliveCounter) / 10f;
                healthyComponent.text = healthyCounter.ToString() + " (" + healthyPercent + "%)";
            }

            if (aliveCounter != 0)
            {
                float infectedPercent = ((infectedCounter * 1000) / aliveCounter) / 10f;
                infectedComponent.text = infectedCounter.ToString() + " (" + infectedPercent + "%)";
            }

            if (aliveCounter != 0)
            {
                float symptomaticPercent = ((symptomaticCounter * 1000) / aliveCounter) / 10f;
                symptomaticComponent.text = symptomaticCounter.ToString() + " (" + symptomaticPercent + "%)";
            }

            if (aliveCounter != 0)
            {
                float criticalPercent = ((criticalCounter * 1000) / aliveCounter) / 10f;
                criticalComponent.text = criticalCounter.ToString() + " (" + criticalPercent + "%)";
            }

            float deadPercent = ((deadCounter * 1000) / totalPopulation) / 10f;
            deadComponent.text = deadCounter.ToString() + " (" + deadPercent + "%)";

            yield return new WaitForSeconds(counterUpdateRate);
        }
    }

    IEnumerator SpawnAgent()
    {
        for (int i = 1; i <= totalPopulation; i++)
        {
            population++;
            aliveCounter++;
            UpdateTexts();
            Instantiate(Agent);
            yield return new WaitForSeconds(spawnDelay);
        }
        population--;
        StopCoroutine(SpawnAgent());
        Debug.Log("Stopping Spawner");
    }
}
