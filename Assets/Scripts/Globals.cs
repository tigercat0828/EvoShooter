using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public static class Globals
{
    private const int MAX_SLOT_NUM = 16;
    private  static int[] scores;
    static Globals() {
        scores = new int[MAX_SLOT_NUM];
    }
    public static void ResetScores() {
        Array.Fill(scores, 0);
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
