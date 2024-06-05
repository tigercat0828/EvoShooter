using System;
using TMPro;
using UnityEngine;

public class EvolutionLevelManager : MonoBehaviour {

    public static EvolutionLevelManager manager;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI GenerationText;

    [SerializeField] public int ArenaNum { get; private set; }


    public Transform[] EntityGroup;
    Gene[] selectedGenes;
    public float CheckPeriod = 10f;
    public float CheckTimer;

    public Transform AgentGroup;
    public GameObject AgentPrefab;
    
    private void Awake() {
        manager = this;
        GameSettings.LoadSettings();
    }
    private void Start() {
        Globals.instance.ResetAllStatus();
        ArenaNum = EntityGroup.Length;
        //selectedGenes = new Gene[ArenaNum];
        SpawnInitialAgents();
    }
    public void Update() {
        CheckTimer += Time.deltaTime;
        if (CheckTimer > CheckPeriod) {
            bool allClosed = CheckAllArenaAreClosed();
            if (allClosed) {
                NextGeneration();
            }
        }
    }

    private void NextGeneration() {
        
    }

    private bool CheckAllArenaAreClosed() {
        for (int i = 0; i < ArenaNum; i++) {
            if (!Globals.instance.ArenaClosed[i]) {
                return false;
            }
        }
        return true;
    }

    public void SpawnInitialAgents() {
        for (int i = 0; i < EntityGroup.Length; i++) {
            Agent agent = Instantiate(AgentPrefab, EntityGroup[i].position, Quaternion.identity, AgentGroup).GetComponent<Agent>();
            agent.SetSlot(i);
            agent.IsInEvoScene = true;
            agent.name = $"Agent ({i})";
            //Gene gene = Gene.GenRandomGene();
            //agent.SetGene(gene);
        }
    }
  
}


