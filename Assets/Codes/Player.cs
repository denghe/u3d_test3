using System.Collections.Generic;
using UnityEngine;

public class Player {
    public static Player instance = new();

    public double health;
    public double damage;
    public double defence;
    public double speed;

    public Bag bagChar = new();
    public Bag bagInventory = new();    // todo: array

}
