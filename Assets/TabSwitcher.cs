using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSwitcher : MonoBehaviour
{
    
    public GameObject[] Tabs;
    public void SwichTab(int tabID) {
        foreach (var tab in Tabs) {
            tab.SetActive(false);
        }
        Tabs[tabID].SetActive(true);
    }
}
