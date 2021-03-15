using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour, IInteractables
{
    public string text = "";
    public void Interact()
    {
        Debug.Log(text);
    }
}
