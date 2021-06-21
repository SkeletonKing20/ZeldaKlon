using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIListener : MonoBehaviour
{
    public Text killText;
    int killCount;
    private void Update()
    {
        Enemy.OnEnemyDeath += increaseCounter;
    }

    void increaseCounter()
    {
        killCount++;
        Debug.Log(killCount);
        killText.text = "killCount: " + killCount;
    }
}
