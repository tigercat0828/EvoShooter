
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatsticFile {
    public int Generations;
    // record each generation
    public List<float> AverageFitness = new();
    public List<int> BestFitness = new();
    public List<int> WorstFitness = new();
    public List<float> StdvFitness = new();
    public Gene LastestAgent;
    public Gene BestAgent;
    public List<Gene> GenBestAgent = new();
    public StatsticFile(int gen) {
        Generations = gen;
    }
    public void WriteToCSV(string filename) {
        filename = Application.dataPath + "/output.csv";

        using (StreamWriter sw = new(filename, false)) {
            sw.WriteLine("Generation, AverageFitness, BestFitness, WorstFitness, StdFitness");
            
            for (int i = 0; i < Generations; i++) {
                sw.WriteLine($"{i},{AverageFitness[i]},{BestFitness[i]},{WorstFitness[i]},{StdvFitness[i]}");
            }

            for (int i = 0; i < Generations; i++) {
                sw.WriteLine(BestFitness[i]+","+ GenBestAgent[i].ToString());
            }
            //sw.WriteLine("LastestAgentGene");
            //sw.WriteLine("BestAgentGene");
        }

    }
}
