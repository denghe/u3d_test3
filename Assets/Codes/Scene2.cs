using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// attach to scene game object
/// </summary>
public class Scene2 : MonoBehaviour {

    // map to res
    public Sprite[] sprites_bg;
    public Sprite[] sprites_item;

    // sprites_bg sort by quality
    internal Sprite[] sprites_bg_quality;

    // map to ui components
    internal TextMeshProUGUI text_title, text_page_number;
    internal TextMeshProUGUI text_health, text_damage, text_defence, text_speed;
    internal Button button_back, button_sort, button_prev, button_next;
    internal GameObject go_bag_char, go_bag_inventory;

    internal void GetComponentTo<T>(ref T tar, string name) {
        tar = GameObject.Find(name).GetComponent<T>();
        Debug.Assert(tar != null);
    }
    internal void GetGameObjectTo(ref GameObject tar, string name) {
        tar = GameObject.Find(name);
        Debug.Assert(tar != null);
    }

    void Awake() {

        Debug.Assert(sprites_item.Length > 0);

        Debug.Assert(sprites_bg.Length == 6);
        sprites_bg_quality = new Sprite[6];
        sprites_bg_quality[0] = sprites_bg.First(o => o.name == "bg_grey");
        sprites_bg_quality[1] = sprites_bg.First(o => o.name == "bg_green");
        sprites_bg_quality[2] = sprites_bg.First(o => o.name == "bg_blue");
        sprites_bg_quality[3] = sprites_bg.First(o => o.name == "bg_purple");
        sprites_bg_quality[4] = sprites_bg.First(o => o.name == "bg_brown");
        sprites_bg_quality[5] = sprites_bg.First(o => o.name == "bg_red");
        foreach (var o in sprites_bg_quality) {
            Debug.Assert(o != null);
        }

        GetComponentTo<TextMeshProUGUI>(ref text_title, "Title");
        GetComponentTo<TextMeshProUGUI>(ref text_page_number, "Page Number");

        GetComponentTo<TextMeshProUGUI>(ref text_health, "Player Prop1 Value");
        GetComponentTo<TextMeshProUGUI>(ref text_damage, "Player Prop2 Value");
        GetComponentTo<TextMeshProUGUI>(ref text_defence, "Player Prop3 Value");
        GetComponentTo<TextMeshProUGUI>(ref text_speed, "Player Prop4 Value");


        GetComponentTo<Button>(ref button_back, "Back");
        GetComponentTo<Button>(ref button_sort, "Sort");
        GetComponentTo<Button>(ref button_prev, "Page Previous");
        GetComponentTo<Button>(ref button_next, "Page Next");

        GetGameObjectTo(ref go_bag_char, "Bag Char");
        GetGameObjectTo(ref go_bag_inventory, "Bag Inventory");

        // ...

        Player.instance.bagInventory.Show(go_bag_inventory);
        // todo: hide when close ui ?
    }

    void Start() {
        Inputs.Init();

    }

    void Update() {
        Inputs.Update();

        // todo: logic

        // sync display
        Draw();
    }

    internal void Draw() {
        var p = Player.instance;
        text_health.text = p.health.ToString();
        text_damage.text = p.damage.ToString();
        text_defence.text = p.defence.ToString();
        text_speed.text = p.speed.ToString();

        //p.bagChar.Draw();
        p.bagInventory.Draw();
    }
}
