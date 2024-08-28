using System.Linq;
using UnityEngine;

/// <summary>
/// 全局资源引用容器，在 scene 的 start 中填充，方便使用，减少传参
/// </summary>
public static class Res {
    public static Sprite[] sprites_bg;                      // 已按 quality 排好序
    public static Sprite[] sprites_item;                    // 所有资源

    public static Sprite sprite_arrow_basic;
    public static Sprite sprite_bone_skull;
    public static Sprite sprite_bone_white;
    public static Sprite sprite_book_closed_red;
    public static Sprite sprite_bottle_standard_blue;
    public static Sprite sprite_bottle_standard_empty;
    public static Sprite sprite_bottle_standard_green;
    public static Sprite sprite_bow_wood1;
    public static Sprite sprite_cape_hood_darkyellow;
    public static Sprite sprite_crown_gold;
    public static Sprite sprite_fish_green;
    public static Sprite sprite_gem_diamond_red;
    public static Sprite sprite_gold_bars_three;
    public static Sprite sprite_gold_coins_many;
    public static Sprite sprite_key_silver;
    public static Sprite sprite_leafs_long;
    public static Sprite sprite_mushroom_big_red;
    public static Sprite sprite_necklace_silver_red;
    public static Sprite sprite_pickaxe_basic;
    public static Sprite sprite_pouch_leather_small;
    public static Sprite sprite_ring_gold_magic;
    public static Sprite sprite_scroll_map2;
    public static Sprite sprite_shield_basic_metal;
    public static Sprite sprite_silver_bars;
    public static Sprite sprite_silver_coins_many;
    public static Sprite sprite_stone_basic_grey;
    public static Sprite sprite_sword_basic4_blue;
    public static Sprite sprite_sword_basic_blue;
    public static Sprite sprite_wood_log;
    public static Sprite sprite_wood_logs_three;

    // ...

    static void SetSprite(ref Sprite sprite, string name) {
        sprite = sprites_item.First(s => s.name == name);
        Debug.Assert(sprite != null);
    }

    public static void Init(Sprite[] sprites_bg_, Sprite[] sprites_item_) {
        Debug.Assert(sprites_bg_.Length == 6);
        sprites_bg = sprites_bg_;

        Debug.Assert(sprites_item_.Length > 0);
        sprites_item = sprites_item_;

        SetSprite(ref sprite_arrow_basic, "arrow_basic");
        SetSprite(ref sprite_bone_skull, "bone_skull");
        SetSprite(ref sprite_bone_white, "bone_white");
        SetSprite(ref sprite_book_closed_red, "book_closed_red");
        SetSprite(ref sprite_bottle_standard_blue, "bottle_standard_blue");
        SetSprite(ref sprite_bottle_standard_empty, "bottle_standard_empty");
        SetSprite(ref sprite_bottle_standard_green, "bottle_standard_green");
        SetSprite(ref sprite_bow_wood1, "bow_wood1");
        SetSprite(ref sprite_cape_hood_darkyellow, "cape_hood_darkyellow");
        SetSprite(ref sprite_crown_gold, "crown_gold");
        SetSprite(ref sprite_fish_green, "fish_green");
        SetSprite(ref sprite_gem_diamond_red, "gem_diamond_red");
        SetSprite(ref sprite_gold_bars_three, "gold_bars_three");
        SetSprite(ref sprite_gold_coins_many, "gold_coins_many");
        SetSprite(ref sprite_key_silver, "key_silver");
        SetSprite(ref sprite_leafs_long, "leafs_long");
        SetSprite(ref sprite_mushroom_big_red, "mushroom_big_red");
        SetSprite(ref sprite_necklace_silver_red, "necklace_silver_red");
        SetSprite(ref sprite_pickaxe_basic, "pickaxe_basic");
        SetSprite(ref sprite_pouch_leather_small, "pouch_leather_small");
        SetSprite(ref sprite_ring_gold_magic, "ring_gold_magic");
        SetSprite(ref sprite_scroll_map2, "scroll_map2");
        SetSprite(ref sprite_shield_basic_metal, "shield_basic_metal");
        SetSprite(ref sprite_silver_bars, "silver_bars");
        SetSprite(ref sprite_silver_coins_many, "silver_coins_many");
        SetSprite(ref sprite_stone_basic_grey, "stone_basic_grey");
        SetSprite(ref sprite_sword_basic4_blue, "sword_basic4_blue");
        SetSprite(ref sprite_sword_basic_blue, "sword_basic_blue");
        SetSprite(ref sprite_wood_log, "wood_log");
        SetSprite(ref sprite_wood_logs_three, "wood_logs_three");

        // ...
    }
}

// for copy

// sprite_arrow_basic
// sprite_bone_skull
// sprite_bone_white
// sprite_book_closed_red
// sprite_bottle_standard_blue
// sprite_bottle_standard_empty
// sprite_bottle_standard_green
// sprite_bow_wood1
// sprite_cape_hood_darkyellow
// sprite_crown_gold
// sprite_fish_green
// sprite_gem_diamond_red
// sprite_gold_bars_three
// sprite_gold_coins_many
// sprite_key_silver
// sprite_leafs_long
// sprite_mushroom_big_red
// sprite_necklace_silver_red
// sprite_pickaxe_basic
// sprite_pouch_leather_small
// sprite_ring_gold_magic
// sprite_scroll_map2
// sprite_shield_basic_metal
// sprite_silver_bars
// sprite_silver_coins_many
// sprite_stone_basic_grey
// sprite_sword_basic4_blue
// sprite_sword_basic_blue
// sprite_wood_log
// sprite_wood_logs_three

// ...