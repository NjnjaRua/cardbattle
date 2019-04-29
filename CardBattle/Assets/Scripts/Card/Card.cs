using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Create Card")]
public class Card : ScriptableObject {

    public string name;
    public string description;

    public Sprite image;

    public long attackPoint;
    public int lifePoint;

    public void PrintInfo()
    {
        Debug.Log(name + ": " + description + " The card haves: " + attackPoint + " ; " + lifePoint);
    }
}
