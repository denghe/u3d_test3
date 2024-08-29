using System.Collections.Generic;
using UnityEngine;

public static class Cfg {
    public static ItemConfig[] items;
    // todo: 掉率表?

    public static ItemConfig item_arrow_basic;
    public static ItemConfig item_bone_skull;
    public static ItemConfig item_bone_white;
    public static ItemConfig item_book_closed_red;
    public static ItemConfig item_bottle_standard_blue;
    public static ItemConfig item_bottle_standard_empty;
    public static ItemConfig item_bottle_standard_green;
    public static ItemConfig item_bow_wood1;
    public static ItemConfig item_cape_hood_darkyellow;
    public static ItemConfig item_crown_gold;
    public static ItemConfig item_fish_green;
    public static ItemConfig item_gem_diamond_red;
    public static ItemConfig item_gold_bars_three;
    public static ItemConfig item_gold_coins_many;
    public static ItemConfig item_key_silver;
    public static ItemConfig item_leafs_long;
    public static ItemConfig item_mushroom_big_red;
    public static ItemConfig item_necklace_silver_red;
    public static ItemConfig item_pickaxe_basic;
    public static ItemConfig item_pouch_leather_small;
    public static ItemConfig item_ring_gold_magic;
    public static ItemConfig item_scroll_map2;
    public static ItemConfig item_shield_basic_metal;
    public static ItemConfig item_silver_bars;
    public static ItemConfig item_silver_coins_many;
    public static ItemConfig item_stone_basic_grey;
    public static ItemConfig item_sword_basic4_blue;
    public static ItemConfig item_sword_basic_blue;
    public static ItemConfig item_wood_log;
    public static ItemConfig item_wood_logs_three;

    // ...

    public static SetsConfig sets_bone;

    public static void Init() {
        Debug.Assert(Res.sprites_bg != null);
        Debug.Assert(items == null);

        /**************************************************************************************************************/
        // arrow_basic
        item_arrow_basic = ItemConfig.Make(ItemQualities.Epic, ItemTypes.WeaponSlave, Res.sprite_arrow_basic, 0, false
            , StatConfig.Make(StatTypes.Damage, 10, 20)
        );

        /**************************************************************************************************************/
        // bone_skull
        item_bone_skull = ItemConfig.Make(ItemQualities.Legendary, ItemTypes.WeaponSlave, Res.sprite_bone_skull, 1, false
            , StatConfig.Make(StatTypes.Damage, 20)
        );
        item_bone_skull.FillAvaliableStats(
            StatConfig.Make(StatTypes.Health, 10),
            StatConfig.Make(StatTypes.Mana, 10)
        );

        /**************************************************************************************************************/
        // bone_white
        item_bone_white = ItemConfig.Make(ItemQualities.Legendary, ItemTypes.WeaponMaster, Res.sprite_bone_white, 1, false
            , StatConfig.Make(StatTypes.Damage, 25)
        );
        item_bone_white.FillAvaliableStats(
            StatConfig.Make(StatTypes.Health, 15),
            StatConfig.Make(StatTypes.Mana, 15)
        );

        /**************************************************************************************************************/
        // set: bone_skull + bone_white
        sets_bone = SetsConfig.Make(item_bone_skull, item_bone_white);
        sets_bone.FillFixedStats(StatConfig.Make(StatTypes.CriticalHitChance, 0.2f));

        /**************************************************************************************************************/
        // book_closed_red
        item_book_closed_red = ItemConfig.Make(ItemQualities.Ancient, ItemTypes.Accessory, Res.sprite_book_closed_red, 0, false
            , StatConfig.Make(StatTypes.CriticalHitChance, 0.5f)
            , StatConfig.Make(StatTypes.CriticalHitDamage, 1f)
        );

        // todo

        // ...

        var os = new List<ItemConfig>();

        os.Add(item_arrow_basic);
        os.Add(item_bone_skull);
        os.Add(item_bone_white);
        os.Add(item_book_closed_red);
        os.Add(item_bottle_standard_blue);
        os.Add(item_bottle_standard_empty);
        os.Add(item_bottle_standard_green);
        os.Add(item_bow_wood1);
        os.Add(item_cape_hood_darkyellow);
        os.Add(item_crown_gold);
        os.Add(item_fish_green);
        os.Add(item_gem_diamond_red);
        os.Add(item_gold_bars_three);
        os.Add(item_gold_coins_many);
        os.Add(item_key_silver);
        os.Add(item_leafs_long);
        os.Add(item_mushroom_big_red);
        os.Add(item_necklace_silver_red);
        os.Add(item_pickaxe_basic);
        os.Add(item_pouch_leather_small);
        os.Add(item_ring_gold_magic);
        os.Add(item_scroll_map2);
        os.Add(item_shield_basic_metal);
        os.Add(item_silver_bars);
        os.Add(item_silver_coins_many);
        os.Add(item_stone_basic_grey);
        os.Add(item_sword_basic4_blue);
        os.Add(item_sword_basic_blue);
        os.Add(item_wood_log);
        os.Add(item_wood_logs_three);

        // ...

        items = os.ToArray();
    }
}
