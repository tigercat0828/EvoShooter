using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.SocialPlatforms.Impl;
public static class SystemIO
{
    public static readonly string SAVE_FOLDER = Path.Combine(Application.persistentDataPath, "saves");
    public static readonly string FILE_EXTENSION = ".json";

    public static void Initialize() {
        if(!Directory.Exists(SAVE_FOLDER)) Directory.CreateDirectory(SAVE_FOLDER);
    }
    private static void Save(string filename ,string data) {
        string filepath = Path.Combine(SAVE_FOLDER, filename+FILE_EXTENSION);
        File.WriteAllText(filepath, data);
    }
    private static string Load(string filename) {

        string filepath = Path.Combine(SAVE_FOLDER, filename + FILE_EXTENSION);
        if (File.Exists(filepath)) {
            string loadedData = File.ReadAllText(filepath);
            return loadedData;
        }
        else {
            return null;
        }
    }
    public static void SaveHighscore(SaveData data, int score) {

        string loadedData = Load("highscore");
        if (loadedData != null) {
            data = JsonUtility.FromJson<SaveData>(loadedData);
        }
        if (score > data.highScore) {
            data.highScore = score;
            string saveData = JsonUtility.ToJson(data);
            Save("highscore", saveData);
        }
    }



}
