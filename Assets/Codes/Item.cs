using System.Collections.Generic;

// 物品通过配置模板来生成, 配置模板是一组类实例
// 生成的物品,并不携带 位置信息( 比如怪身上? 地上啥坐标? 背包哪格子? )

/// <summary>
/// 物品词条
/// </summary>
public struct Stat {
    public StatTypes type;
    public float value;
}

/// <summary>
/// 物品词条配置
/// </summary>
public struct StatConfig {
    public StatTypes type;
    public float valueFrom, valueTo;
}

/// <summary>
/// 物品基础配置模板
/// </summary>
public struct ItemConfig {
    public ItemQualities cQuality;              // 品质
    public ItemTypes cType;                     // 佩戴部位
    public int cResId;                          // 资源 id
    public bool cAllowMultiple;                 // 是否允许堆叠( 数量 > 1 )
    StatConfig[] allowedStats;                  // 允许存在的词条列表
}

public class ItemConfigManager {

    public List<Item> generateResult = new();
    public void Generate() {

    }
}

/// <summary>
/// 物品实例数据
/// </summary>
public class Item {
    public int id;                              // 全局自增 id, 生成时填充
    public double quantity;                     // 数量

    // 词条容器
    Stat[] stats;
}

/// <summary>
/// 掉落到地上的物品( 当从地面拾取时，将转为 BagItem 智能的放入背包 )
/// </summary>
public class FloorItem : Item {
    // todo: 地面坐标 啥的?
}


//[StructLayout(LayoutKind.Explicit)]
//public struct Stat {
//    [FieldOffset(0)]
//    public StatTypes type;

//    [FieldOffset(4)]
//    public float f1;
//    [FieldOffset(8)]
//    public float f2;
//    [FieldOffset(12)]
//    public float f3;

//    [FieldOffset(4)]
//    public int i1;
//    [FieldOffset(8)]
//    public int i2;
//    [FieldOffset(12)]
//    public int i3;

//    [FieldOffset(8)]
//    public double d1;
//    [FieldOffset(8)]
//    public long l1;
//}
