using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EvolutionLevelManager : MonoBehaviour {

    public static EvolutionLevelManager manager;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI GenerationText;


    [SerializeField] private float GenTimer = 0;
    public int CurrentGeneration = 0;
    public float GenerationPeriod = 300;  // 300 seconds

    // Parameter
    public  int POPULATIONS = 16;
    public  float MUTATION_RATE = 0.05f;
    public  int MAX_GENERATIONS = 3;
    // Environment Selection : Generation Model
    //       Local Selection : Truncation top 50%
    
    
    
    public float CheckPeriod = 10f;
    float CheckTimer;
    
    // keep reference
    public List<Agent> AgentList = new();
    private List<Gene> AgentGenes = new();
    
    // for position and parent-child hierarchy
    public Transform AgentGroup;
    public GameObject AgentPrefab;
    public Transform[] EntityGroup;


    public bool IsEvoEnding = false;

    private void Awake() {
        manager = this;
        GameSettings.LoadSettings();
        
    }
    private void Start() {
        Globals.instance.ResetAllStatus();
        Globals.instance.StatFile = new(MAX_GENERATIONS);
        IsEvoEnding = false ;
        SpawnInitialAgents();
        
    }
    public void Update() {

      
        if (!IsEvoEnding) {
            if (CurrentGeneration == MAX_GENERATIONS) {
                IsEvoEnding = true;
                Debug.Log($"gen : {CurrentGeneration}");
                Globals.instance.StatFile.WriteToCSV("outputData.csv");
                Debug.Log("File write done");
            }
            CheckTimer += Time.deltaTime;
            GenTimer += Time.deltaTime;

            if (CheckTimer > CheckPeriod) {

                bool allClosed = CheckAllArenaAreClosed();
                if (allClosed || GenTimer > GenerationPeriod) {
                    NextGeneration();
                    GenTimer = 0;
                }
                CheckTimer = 0;
            }
        }
        else {
            Debug.Log("Evo ending");
        }
    }
    struct AgentInfo {
        public int index;
        public int fitness;

        public AgentInfo(int index, int fitness) {
            this.index = index;
            this.fitness = fitness;
        }
    }
    private void NextGeneration() {
        Debug.Log($"New Generation {CurrentGeneration}");
        Globals.instance.State = EvoGameState.Calculating;
        CurrentGeneration++;
        // all entity go to hell 
        foreach (var agent in AgentList) {
            if(agent != null) agent.TakeDamage(1000);
        }
        DestroyGameObjectsWithTag("Enemy");
        DestroyGameObjectsWithTag("EnemyBullet");
        DestroyGameObjectsWithTag("Bullet");


        List<AgentInfo> infos = new();
        // record gene of agents
        for (int i = 0; i < POPULATIONS; i++) {
             infos.Add(new(i, Globals.instance.scores[i]));
        }
        infos = infos.OrderByDescending(ainfo => ainfo.fitness).ToList();
        // record statstics info
        double mean = infos.Average(i => i.fitness);
        Globals.instance.StatFile.AverageFitness.Add((float)mean);
        Globals.instance.StatFile.BestFitness.Add(infos.First().fitness);
        Globals.instance.StatFile.WorstFitness.Add(infos.Last().fitness);

        double stdv = Math.Sqrt(infos.Sum(x => Math.Pow(x.fitness - mean, 2)) / POPULATIONS);
        Globals.instance.StatFile.StdvFitness.Add((float)stdv);

        int gnebestIndex = infos.First().index;
        Globals.instance.StatFile.GenBestAgent.Add(AgentGenes[gnebestIndex]);
        



        List<Gene> selectedGenes = new();
        // Truncation by 50 %
        for (int i = 0; i < POPULATIONS/2; i++) {
            int index = infos[i].index;
            
            selectedGenes.Add(AgentGenes[index]);
            selectedGenes.Add(AgentGenes[index]);
        }
        Globals.instance.ResetAllStatus();
        AgentList.Clear();
        AgentGenes.Clear();

        // inherit
        List<Gene> newGenGenes = new();
        
        for (int i = 0; i < POPULATIONS; i += 2) {
            (Gene a, Gene b) = Gene.CrossOver(selectedGenes[i], selectedGenes[i + 1]);
            newGenGenes.Add(a);
            newGenGenes.Add(b);
        }
        for (int i = 0; i < newGenGenes.Count; i++) {
            if (UnityEngine.Random.Range(0f, 1f) < MUTATION_RATE) {
                newGenGenes[i] = Gene.Mutate(newGenGenes[i]);
            }
        }
        for (int i = 0; i < 16; i++) {
            Agent agent = Instantiate(AgentPrefab, EntityGroup[i].position, Quaternion.identity, AgentGroup).GetComponent<Agent>();
            agent.SetSlot(i);
            agent.IsInEvoScene = true;
            agent.name = $"Agent ({i})";
            agent.SetGene(newGenGenes[i]);
            AgentList.Add(agent);
            AgentGenes.Add(new(newGenGenes[i]));
        }


        //// random search 
        //for (int i = 0; i < 8; i++) {
        //    Agent agent = Instantiate(AgentPrefab, EntityGroup[i].position, Quaternion.identity, AgentGroup).GetComponent<Agent>();
        //    agent.SetSlot(i);
        //    agent.IsInEvoScene = true;
        //    agent.name = $"Agent ({i})";
        //    agent.SetGene(selectedGenes[i]);
        //    AgentList.Add(agent);
        //    AgentGenes.Add(new(selectedGenes[i]));
        //}

        //for (int i = 8; i < 16; i++) {
        //    Agent agent = Instantiate(AgentPrefab, EntityGroup[i].position, Quaternion.identity, AgentGroup).GetComponent<Agent>();
        //    agent.SetSlot(i);
        //    agent.IsInEvoScene = true;
        //    agent.name = $"Agent ({i})";
        //    Gene gene = Gene.GenRandomGene();
        //    agent.SetGene(gene);
        //    AgentList.Add(agent);
        //    AgentGenes.Add(gene);
        //}


        selectedGenes.Clear();
        Globals.instance.State = EvoGameState.Evolving;
        GenTimer = 0;

    }

    private bool CheckAllArenaAreClosed() {
        for (int i = 0; i < POPULATIONS; i++) {
            if (!Globals.instance.ArenaClosed[i]) {
                return false;
            }
        }
        return true;
    }

    public void SpawnInitialAgents() {
        CurrentGeneration = 1;
        for (int i = 0; i < EntityGroup.Length; i++) {
            Agent agent = Instantiate(AgentPrefab, EntityGroup[i].position, Quaternion.identity, AgentGroup).GetComponent<Agent>();
            agent.SetSlot(i);
            agent.IsInEvoScene = true;
            agent.name = $"Agent ({i})";
            Gene gene = Gene.GenRandomGene();
            agent.SetGene(gene);
            AgentList.Add(agent);
            AgentGenes.Add(new(gene));
        }
    }
    void DestroyGameObjectsWithTag(string tag) {
        // Find all GameObjects with the specified tag
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

        // Loop through each GameObject and destroy them
        foreach (GameObject obj in objectsWithTag) {
            Destroy(obj);
        }
    }
}


