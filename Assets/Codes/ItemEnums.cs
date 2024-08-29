/// <summary>
/// 物品品质分类，对应 Res.sprites_bg 下标
/// </summary>
public enum ItemQualities {
    /// <summary>
    /// 普通
    /// </summary>
    Grey,
    Normal = Grey,
    /// <summary>
    /// 精良
    /// </summary>
    Green,
    Excellent = Green,
    /// <summary>
    /// 稀有
    /// </summary>
    Blue,
    Rare = Blue,
    /// <summary>
    /// 史诗
    /// </summary>
    Purple,
    Epic = Purple,
    /// <summary>
    /// 传奇
    /// </summary>
    Brown,
    Legendary = Brown,
    /// <summary>
    /// 远古
    /// </summary>
    Red,
    Ancient = Red,

    // 太古 archaic ?
}

/// <summary>
/// 物品佩戴部位分类( 也包括非佩戴部位. 不一定用得完 )
/// </summary>
public enum ItemTypes {
    /// <summary>
    /// 头盔
    /// </summary>
    Helm,
    /// <summary>
    /// 项链
    /// </summary>
    Amulet,
    /// <summary>
    /// 肩甲
    /// </summary>
    Shoulder,
    /// <summary>
    /// 护甲( 衣服 )
    /// </summary>
    Armor,
    /// <summary>
    /// 腰带
    /// </summary>
    Belt,
    /// <summary>
    /// 裤子
    /// </summary>
    Pants,
    /// <summary>
    /// 手套
    /// </summary>
    Glove,
    /// <summary>
    /// 鞋子
    /// </summary>
    Boots,
    /// <summary>
    /// 戒指( 2 ~ 多个 )
    /// </summary>
    Ring,
    /// <summary>
    /// 武器( 主手 )
    /// </summary>
    WeaponMaster,
    /// <summary>
    /// 武器( 副手 )
    /// </summary>
    WeaponSlave,
    /// <summary>
    /// 饰品( 多个 )
    /// </summary>
    Accessory,

    /// <summary>
    /// 药瓶, 能喝( 映射到恢复技能 )( 无法装配到身上 )
    /// </summary>
    Potion,
    /// <summary>
    /// 卷轴, 能施展( 映射到技能 )( 无法装配到身上 )
    /// </summary>
    Scrolls,
    /// <summary>
    /// 钥匙, 能开门开箱( 无法装配到身上 )
    /// </summary>
    Keys,
    /// <summary>
    /// 宝石, 可镶嵌( 无法装配到身上 )
    /// </summary>
    Gems,
    /// <summary>
    /// 材料( 无法装配到身上 )
    /// </summary>
    Material,
    /// <summary>
    /// 其他( 任务物品? 无法装配到身上 )
    /// </summary>
    Others
}

/// <summary>
/// 物品词条分类 ( 不一定用的完, 备注仅供参考, 具体细节再说 )
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
    /// 防御值( 以点数的方式，抵消等值伤害 )
    /// </summary>
    Defence,

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

    /********************************************************************************/
    // 下面是一些非通用的，和具体技能紧密相关的词条

    /// <summary>
    /// 击退概率
    /// How high your chance is per strike to knock away foes.
    /// </summary>
    KnockbackChance,

    /// <summary>
    /// 穿透次数( 额外的 ). 对于碰撞到敌人后本该消亡的技能抛射物，穿越并造成多次伤害。
    /// 每次造成伤害有个冷却时间，通常为 0.2 秒( 该数值受冷却时间减少影响 )
    /// </summary>
    PierceCount,

    // todo: more
}
