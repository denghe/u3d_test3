using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEditor.SceneManagement;

/// <summary>
/// attach to scene game object
/// </summary>
public class Scene2 : MonoBehaviour {

    // map to default material
    public Material material;

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

    // runtime fields
    Player player;                                      // 当前玩家上下文( 指向 Player.instance )


    void Start() {
        // binds
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

        // static inits
        Res.sprites_bg = sprites_bg;
        Res.sprites_item = sprites_item;
        GO.Init(material, 50000);

        // instance inits
        player = Player.instance;
        var bag = player.bagInventory;
        var bagPos = go_bag_inventory.transform.position;
        Debug.Log(bagPos);
        bag.Init(7, 10, 132, bagPos.x, bagPos.y);

        // 先随便生成一些 item for test
        for (int rowIndex = 0; rowIndex < 7; rowIndex++) {
            for (int colIndex = 0; colIndex < 10; colIndex++) {
                if (Random.value > 0.5f) {
                    var id = Random.Range(0, sprites_item.Length);
                    var quality = (BagItemQuality)Random.Range(0, sprites_bg.Length);
                    new BagItem(bag, id, quality, 1, rowIndex, colIndex);
                }
            }
        }

        Inputs.Init();
    }

    void Update() {
        Inputs.Update();

        Env.timePool += Time.deltaTime;
        if (Env.timePool > Env.frameDelay) {
            Env.timePool -= Env.frameDelay;
            ++Env.frameNumber;
            Env.time = Env.frameNumber * Env.frameDelay;

            // todo: logic
            player.bagInventory.Update();
        }

        Draw();
    }

    internal void Draw() {
        text_health.text = player.health.ToString();
        text_damage.text = player.damage.ToString();
        text_defence.text = player.defence.ToString();
        text_speed.text = player.speed.ToString();

        //player.bagChar.Draw();
        player.bagInventory.Draw();
    }
}
