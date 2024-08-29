using System.Runtime.InteropServices;

// ... todo: 抄英雄旧忆

/// <summary>
/// 品质，对应 Res.sprites_bg 下标
/// </summary>
public enum ItemQualities {
    Grey, Green, Blue, Purple, Brown, Red
}

/// <summary>
/// 佩戴部位分类
/// </summary>
public enum ItemTypes {
    Hat, Amulet, Ring, Arm, Armor, Belt, Pants, Glove, Boots, Weapon1, Weapon2, Relic, Material
}

/// <summary>
/// 物品基础配置
/// </summary>
public class ItemConfig {
    public ItemQualities cQuality;               // 品质
    public ItemTypes cType;                      // 佩戴部位
    public int cResId;                           // 资源 id
    public bool cAllowMultiple;                  // 是否允许堆叠
    // ...
}

/// <summary>
/// 物品实例数据
/// </summary>
public class Item : ItemConfig {
    // todo: 自增 id ?
    public double quantity;                     // 数量( todo: 显示为文字? )

    // 词条容器?
    // ...
}


/// <summary>
/// 物品词条分类 ( 抄的，不一定抄完整了，不一定用的完, 备注里提到的 数据上限，仅供参考 )
/// </summary>
public enum StatsTypes {
    /// <summary>
    /// How much damage you can take before you die.
    /// </summary>
    Health,

    /// <summary>
    /// This is a resource that is drained when you use skills.
    /// </summary>
    Mana,

    /// <summary>
    /// This is your base damage, elemental bonuses are applied on top of this number.
    /// </summary>
    Damage,

    /// <summary>
    /// This is how fast you will be able to attack it affects the animation speed of your attacks,
    /// so skills with low cooldown can be performed faster. 
    /// It also allows you to move sooner after attacking, as the animation completes faster.
    /// </summary>
    AttackSpeed,

    /// <summary>
    /// This stat increases the effective range, radius, and distance used by all skills.
    /// </summary>
    Reach,

    /// <summary>
    /// Reduces damage take from all sources, scales multiplicatively rather than additively.
    /// </summary>
    DamageReduction,

    /// <summary>
    /// Evasion gives a chance to completely avoid damage from most direct hits.
    /// </summary>
    Evasion,

    /// <summary>
    /// This indicates how high your chance is to score a Critical Hit, dealing extra damage.
    /// </summary>
    CriticalHitChance,

    /// <summary>
    /// This indicates how much damage your Critical Hits will do in comparison to regular strikes.
    /// </summary>
    CriticalHitDamage,

    /// <summary>
    /// This is how fast you can run.
    /// </summary>
    MovementSpeed,

    // Elemental Damage stats( Physical, Fire, Frost, etc. )
    // These stats show how much extra damage you do with skills of certain elements.
    PhysicalDamage,
    FireDamage,
    HolyDamage,
    PoisonDamage,
    LightningDamage,
    FrostDamage,
    ThornsDamage,

    /// <summary>
    /// How much Health you restore per second.
    /// </summary>
    HealthRegeneration,

    /// <summary>
    /// How much Mana you restore per second.
    /// </summary>
    ManaRegeneration,

    /// <summary>
    /// How effective your Healing and Mana potions are. Increases this number and they will heal you more!
    /// </summary>
    PotionEffectiveness,

    /// <summary>
    /// How high your chance is per strike to knock away foes.
    /// </summary>
    KnockbackChance,

    // Resistances( Physical, Fire, Frost, etc. )
    // These stats show how much less damage you take from all sources of the matching elements.
    // Note that all resistances are capped at 80%.
    PhysicalResistance,
    FireResistance,
    HolyResistance,
    PoisonResistance,
    LightningResistance,
    FrostResistance,

    /// <summary>
    /// How much time is removed from your skill cooldowns.
    /// Caps at 90% Scales multiplicatively rather than additively.
    /// </summary>
    CooldownReduction,

    /// <summary>
    /// How much of the Mana cost is removed from your skill usage.
    /// Scales multiplicatively rather than additively.
    /// </summary>
    ManaCostReduction,
}

/// <summary>
/// 物品词条
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct Stats {
    [FieldOffset(0)]
    public StatsTypes type;

    [FieldOffset(4)]
    public float f1;
    [FieldOffset(8)]
    public float f2;
    [FieldOffset(12)]
    public float f3;

    [FieldOffset(4)]
    public int i1;
    [FieldOffset(8)]
    public int i2;
    [FieldOffset(12)]
    public int i3;

    [FieldOffset(8)]
    public double d1;
    [FieldOffset(8)]
    public long l1;
}

/// <summary>
/// 掉落到地上的物品( 当从地面拾取时，将转为 BagItem 智能的放入背包 )
/// </summary>
public class FloorItem : Item {
    // todo: 地面坐标 啥的?
}
