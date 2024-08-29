using System.Runtime.InteropServices;

// ... todo: 抄英雄旧忆

/// <summary>
/// 品质，对应 Res.sprites_bg 下标
/// </summary>
public enum ItemQualities {
    Grey, Green, Blue, Purple, Brown, Red
}

/// <summary>
/// 佩戴部位分类( 无法佩戴的材料啥的也包含 )
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

    // 词条容器
    Stat[] stats;
}


/// <summary>
/// 物品词条分类 ( 抄的，不一定抄完整了，不一定用的完, 备注里提到的 数据上限，附加限制，仅供参考 )
/// </summary>
public enum StatTypes {

    /// <summary>
    /// 生命
    /// How much damage you can take before you die.
    /// </summary>
    Health,

    /// <summary>
    /// 魔法
    /// This is a resource that is drained when you use skills.
    /// </summary>
    Mana,

    /// <summary>
    /// 伤害
    /// This is your base damage, elemental bonuses are applied on top of this number.
    /// </summary>
    Damage,

    /// <summary>
    /// 攻击速度
    /// This is how fast you will be able to attack it affects the animation speed of your attacks,
    /// so skills with low cooldown can be performed faster. 
    /// It also allows you to move sooner after attacking, as the animation completes faster.
    /// </summary>
    AttackSpeed,

    /// <summary>
    /// 暴击概率
    /// This indicates how high your chance is to score a Critical Hit, dealing extra damage.
    /// </summary>
    CriticalHitChance,

    /// <summary>
    /// 暴击伤害
    /// This indicates how much damage your Critical Hits will do in comparison to regular strikes.
    /// </summary>
    CriticalHitDamage,

    // Elemental Damage stats( Physical, Fire, Frost, etc. )
    // These stats show how much extra damage you do with skills of certain elements.

    /// <summary>
    /// 物理伤害
    /// </summary>
    PhysicalDamage,
    /// <summary>
    /// 毒素伤害
    /// </summary>
    PoisonDamage,
    /// <summary>
    /// 闪电伤害
    /// </summary>
    LightningDamage,
    /// <summary>
    /// 冰霜伤害
    /// </summary>
    FrostDamage,
    /// <summary>
    /// 火焰伤害
    /// </summary>
    FireDamage,
    /// <summary>
    /// 神圣伤害
    /// </summary>
    HolyDamage,
    /// <summary>
    /// 暗影伤害
    /// </summary>
    ShadowDamage,

    /// <summary>
    /// 燃烧伤害
    /// This stat is a special damage stat for items which may cause a Burn damage effect on foes.
    /// It is considered Fire damage and is increased by Fire Damage stat, too.
    /// NOTE: This stat does not affect skills which cause Burn effects.
    /// </summary>
    BurnDamage,

    /// <summary>
    /// 流血伤害
    /// This stat is a special damage stat for items which may cause a Bleed damage effect on foes.
    /// It is considered Ethereal damage but is NOT improved by any elemental stats.
    /// NOTE: This stat does not affects skills which cause Bleeding effects.
    /// </summary>
    BleedDamage,

    /// <summary>
    /// 荆棘
    /// This is a special damage stat that is dealt to enemies when they strike you.
    /// Thorns is considered to be Physical damange unless otherwise specified.
    /// Thorns damage can crit, but is considered indirect damage and does not
    /// Overpower to scale through enemy resistances.
    /// </summary>
    Thorns,

    /// <summary>
    /// 击退概率
    /// How high your chance is per strike to knock away foes.
    /// </summary>
    KnockbackChance,

    /// <summary>
    /// 生命回复
    /// How much Health you restore per second.
    /// </summary>
    HealthRegeneration,

    /// <summary>
    /// 魔法回复
    /// How much Mana you restore per second.
    /// </summary>
    ManaRegeneration,

    /// <summary>
    /// 击中回血
    /// How much Health you restore on a hit, minimum. The larger your hit, the more you will restore.
    /// Note that Health on hit has a cooldown of 0.1 seconds.
    /// </summary>
    HealthOnHit,

    /// <summary>
    /// 击中回魔
    /// How much Mana you restore on a hit, minimum. The larger your hit, the more you will restore.
    /// Note that Health on hit has a cooldown of 0.1 seconds.
    /// </summary>
    ManaOnHit,

    /// <summary>
    /// 闪避
    /// Evasion gives a chance to completely avoid damage from most direct hits.
    /// </summary>
    Evasion,

    /// <summary>
    /// 伤害减免
    /// Reduces damage take from all sources, scales multiplicatively rather than additively.
    /// </summary>
    DamageReduction,

    /// <summary>
    /// 意志力
    /// Lowers the duration of negative effects on the player, such as damaging debuffs.
    /// </summary>
    Willpower,

    // Resistances( Physical, Fire, Frost, etc. )
    // These stats show how much less damage you take from all sources of the matching elements.
    // Note that all resistances are capped at 80%.

    /// <summary>
    /// 物理抗性
    /// </summary>
    PhysicalResistance,
    /// <summary>
    /// 毒素抗性
    /// </summary>
    PoisonResistance,
    /// <summary>
    /// 闪电抗性
    /// </summary>
    LightningResistance,
    /// <summary>
    /// 冰霜抗性
    /// </summary>
    FrostResistance,
    /// <summary>
    /// 火焰抗性
    /// </summary>
    FireResistance,
    /// <summary>
    /// 神圣抗性
    /// </summary>
    HolyResistance,
    /// <summary>
    /// 暗影抗性
    /// </summary>
    ShadowResistance,

    /// <summary>
    /// 冷却时间减少
    /// How much time is removed from your skill cooldowns.
    /// Caps at 90% Scales multiplicatively rather than additively.
    /// </summary>
    CooldownReduction,

    /// <summary>
    /// 魔法消耗减少
    /// How much of the Mana cost is removed from your skill usage.
    /// Scales multiplicatively rather than additively.
    /// </summary>
    ManaCostReduction,

    /// <summary>
    /// 效果持续时间
    /// How long buffs (positive effects) and spells will last No cap.
    /// Scales multiplicatively rather than additively.
    /// </summary>
    EffectDuration,

    /// <summary>
    /// 额外水晶
    /// This stat increases the amount of Crystals you gan from all sources except vendors.
    /// </summary>
    CrystalsFound,

    /// <summary>
    /// 寻宝值
    /// This stat increase the likelihood that you will find more rare items and items fitting your class.
    /// It caps off at 999%.
    /// </summary>
    MagicFind,

    /// <summary>
    /// 额外经验
    /// This stat affects how much Experience you gain from all sources.
    /// </summary>
    ExperienceGained,

    /// <summary>
    /// 药剂效果
    /// How effective your Healing and Mana potions are. Increases this number and they will heal you more!
    /// </summary>
    PotionEffectiveness,

    /// <summary>
    /// 效果范围
    /// This stat increases the effective range, radius, and distance used by all skills.
    /// </summary>
    Reach,

    /// <summary>
    /// 移动速度
    /// This is how fast you can run.
    /// </summary>
    MovementSpeed,

    /// <summary>
    /// 拾取强度
    /// This stat affects how much Health and Mana pickups will heal you.
    /// </summary>
    PickupStrenth,

    /// <summary>
    /// 拾取范围
    /// This stat affects the distance of which Health, Mana, and Crystal pickups will be automatically taken.
    /// Your companions also use this stat when picking up pickups!
    /// </summary>
    PickupRadius,

    /// <summary>
    /// 同伴生命
    /// This stat directly increases the Health stat of all your Companions.
    /// </summary>
    CompanionHealth,

    /// <summary>
    /// 同伴伤害
    /// This stat directly affects the Damage stat of all your Companions.
    /// </summary>
    CompanionDamage,

    /// <summary>
    /// 同伴伤害减免
    /// </summary>
    CompanionDamageReduction,

    /// <summary>
    /// 同伴主动和被动效果
    /// This stat increases the effect of any passive bonuses granted by companions,
    /// such as stat boosts and auras. It also increases the 'Use' effect of all companion skills.
    /// For example, a companion that has a spell cast listed as its 'Use' effect will
    /// deal increased damage with that spell proportional to this stat.
    /// </summary>
    CompanionPassiveAndUseEffect,

    /// <summary>
    /// 宝石强度
    /// Increases the strength of all gems socketed into your equipment.
    /// </summary>
    GemStrength,

    /// <summary>
    /// 光照范围
    /// This stat affects the light surrounding your character.
    /// </summary>
    LightRadius,
}

/// < summary >
/// 物品词条
/// </ summary >
[StructLayout(LayoutKind.Explicit)]
public struct Stat {
    [FieldOffset(0)]
    public StatTypes type;

    [FieldOffset(4)]
    public float f1;
    [FieldOffset(4)]
    public int i1;
}


    /// <summary>
    /// 物品词条
    /// </summary>
    //[StructLayout(LayoutKind.Explicit)]
    //public struct Stats {
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

    /// <summary>
    /// 掉落到地上的物品( 当从地面拾取时，将转为 BagItem 智能的放入背包 )
    /// </summary>
    public class FloorItem : Item {
    // todo: 地面坐标 啥的?
}
