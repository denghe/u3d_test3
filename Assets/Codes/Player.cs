using System.Collections.Generic;
using UnityEngine;

public class Player {
    public static Player instance = new();  // 单机先这样

    // 常见基础词条结存变量
    public double health;
    public double damage;
    public double defence;
    public double speed;
    // todo: 扩展词条结存变量容器?

    public Bag bagChar = new();
    public Bag bagInventory = new();    // todo: array

    // todo: 词条计算函数, 传入目标
}
