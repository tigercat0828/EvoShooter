using System;
using UnityEngine;


public class Globals : MonoBehaviour 
{
    private const int MAX_SLOT_NUM = 16;
    public int[] scores = new int[MAX_SLOT_NUM];
    public bool[] ArenaClosed = new bool[MAX_SLOT_NUM];

    
    public static Globals instance;

    public void ResetAllStatus() { 
        // reset score
        Array.Fill(scores, 0);
        // reset arena active status
        Array.Fill(ArenaClosed, false);
    }
    public void Awake() {
        instance = this;
        ResetAllStatus();
    }
    public int GetScore(int slot) {
        return scores[slot];
    }
    public  void AddScore(int slot, int score) {
        scores[slot] += score;
    }
    public  void Minus(int slot, int score) {
        scores[slot] -= score;
    }
}
