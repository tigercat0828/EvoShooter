using System;


public static class Globals
{
    private const int MAX_SLOT_NUM = 16;
    private static int[] scores;
    private static bool[] ArenaActives;

    static Globals() {
        scores = new int[MAX_SLOT_NUM];
        ArenaActives = new bool[MAX_SLOT_NUM];
    }
    public static void ResetAllStatus() {
        // reset score
        Array.Fill(scores, 0);
        // reset arena active status
        Array.Fill(ArenaActives, false);
    }

    public static int GetScore(int slot) {
        return scores[slot];
    }
    public static void AddScore(int slot, int score) {
        scores[slot] += score;
    }
    public static void Minus(int slot, int score) {
        scores[slot] -= score;
    }
}
