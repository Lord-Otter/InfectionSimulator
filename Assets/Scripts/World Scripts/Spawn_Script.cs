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


    //Start Population
    public GameObject startPopulation;
    private TextMeshProUGUI populationComponent;


    //Alive
    public GameObject alive;
    private TextMeshProUGUI aliveComponent;


    //Immune
    public GameObject immune;
    private TextMeshProUGUI immuneComponent;

    public GameObject immuneBar;
    private RectTransform immuneBarComponent;

    public GameObject immuneBarText;
    private TextMeshProUGUI immuneBarTextComponent;
    

    //Healthy
    public GameObject healthy;
    private TextMeshProUGUI healthyComponent;

    public GameObject healthyBar;
    private RectTransform healthyBarComponent;

    public GameObject healthyBarText;
    private TextMeshProUGUI healthyBarTextComponent;


    //Infected
    public GameObject infected;
    private TextMeshProUGUI infectedComponent;

    public GameObject infectedBar;
    private RectTransform infectedBarComponent;

    public GameObject infectedBarText;
    private TextMeshProUGUI infectedBarTextComponent;


    //Symptomatic
    public GameObject symptomatic;
    private TextMeshProUGUI symptomaticComponent;

    public GameObject symptomaticBar;
    private RectTransform symptomaticBarComponent;

    public GameObject symptomaticBarText;
    private TextMeshProUGUI symptomaticBarTextComponent;


    //Critical
    public GameObject critical;
    private TextMeshProUGUI criticalComponent;

    public GameObject criticalBar;
    private RectTransform criticalBarComponent;

    public GameObject criticalBarText;
    private TextMeshProUGUI criticalBarTextComponent;


    //Dead
    public GameObject dead;
    private TextMeshProUGUI deadComponent;

    public GameObject deadBar;
    private RectTransform deadBarComponent;

    public GameObject deadBarText;
    private TextMeshProUGUI deadBarTextComponent;

    // Start is called before the first frame update
    void Start()
    {
        //Start Population
        populationComponent = startPopulation.GetComponent<TextMeshProUGUI>();


        //Alive
        aliveComponent = alive.GetComponent<TextMeshProUGUI>();


        //Immune
        immuneComponent = immune.GetComponent<TextMeshProUGUI>();
        immuneBarComponent = immuneBar.GetComponent<RectTransform>();
        immuneBarTextComponent = immuneBarText.GetComponent<TextMeshProUGUI>();


        //Healthy
        healthyComponent = healthy.GetComponent<TextMeshProUGUI>();
        healthyBarComponent = healthyBar.GetComponent<RectTransform>();
        healthyBarTextComponent = healthyBarText.GetComponent<TextMeshProUGUI>();


        //Infected
        infectedComponent = infected.GetComponent<TextMeshProUGUI>();
        infectedBarComponent = infectedBar.GetComponent<RectTransform>();
        infectedBarTextComponent = infectedBarText.GetComponent<TextMeshProUGUI>();


        //Symptomatic
        symptomaticComponent = symptomatic.GetComponent<TextMeshProUGUI>();
        symptomaticBarComponent = symptomaticBar.GetComponent<RectTransform>();
        symptomaticBarTextComponent = symptomaticBarText.GetComponent<TextMeshProUGUI>();


        //Critical
        criticalComponent = critical.GetComponent<TextMeshProUGUI>();
        criticalBarComponent = criticalBar.GetComponent<RectTransform>();
        criticalBarTextComponent = criticalBarText.GetComponent<TextMeshProUGUI>();


        //Dead
        deadComponent = dead.GetComponent<TextMeshProUGUI>();
        deadBarComponent = deadBar.GetComponent<RectTransform>();
        deadBarTextComponent = deadBarText.GetComponent<TextMeshProUGUI>();

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
            aliveComponent.text = $"{aliveCounter} ({alivePercent:F1}%)";

            if (aliveCounter != 0)
            {
                float immunePercent = ((immuneCounter * 1000) / aliveCounter) / 10f;
                immuneComponent.text = $"{immuneCounter} ({immunePercent:F1}%)";

                ChangeBarWidth(immuneBarComponent, (float)immuneCounter / (float)population * 1000);
                immuneBarTextComponent.text = immuneCounter.ToString();
            }

            if (aliveCounter != 0)
            {
                float healthyPercent = ((healthyCounter * 1000) / aliveCounter) / 10f;
                healthyComponent.text = $"{healthyCounter} ({healthyPercent:F1}%)";

                ChangeBarWidth(healthyBarComponent, (float)healthyCounter / (float)population * 1000);
                healthyBarTextComponent.text = healthyCounter.ToString();
            }

            if (aliveCounter != 0)
            {
                float infectedPercent = ((infectedCounter * 1000) / aliveCounter) / 10f;
                infectedComponent.text = $"{infectedCounter} ({infectedPercent:F1}%)";

                ChangeBarWidth(infectedBarComponent, (float)infectedCounter / (float)population * 1000);
                infectedBarTextComponent.text = infectedCounter.ToString();
            }

            if (aliveCounter != 0)
            {
                float symptomaticPercent = ((symptomaticCounter * 1000) / aliveCounter) / 10f;
                symptomaticComponent.text = $"{symptomaticCounter} ({symptomaticPercent:F1}%)";

                ChangeBarWidth(symptomaticBarComponent, (float)symptomaticCounter / (float)population * 1000);
                symptomaticBarTextComponent.text = symptomaticCounter.ToString();
            }

            if (aliveCounter != 0)
            {
                float criticalPercent = ((criticalCounter * 1000) / aliveCounter) / 10f;
                criticalComponent.text = $"{criticalCounter} ({criticalPercent:F1}%)";

                ChangeBarWidth(criticalBarComponent, (float)criticalCounter / (float)population * 1000);
                criticalBarTextComponent.text = criticalCounter.ToString();
            }

            float deadPercent = ((deadCounter * 1000) / totalPopulation) / 10f;
            deadComponent.text = $"{deadCounter} ({deadPercent:F1}%)";

            ChangeBarWidth(deadBarComponent, (float)deadCounter / (float)population * 1000);
            deadBarTextComponent.text = deadCounter.ToString();
            

            yield return new WaitForSeconds(counterUpdateRate);
        }
    }

    void ChangeBarWidth(RectTransform rectTransform, float newWidth)
    {
        Vector2 currentSize = rectTransform.sizeDelta;

        rectTransform.sizeDelta = new Vector2(newWidth, currentSize.y);
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
