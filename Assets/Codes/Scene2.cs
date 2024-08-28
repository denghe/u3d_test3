using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// attach to scene game object
/// </summary>
public class Scene2 : MonoBehaviour {

    // map to default material
    public Material material;

    // map to res
    public Sprite[] sprites_bg;
    public Sprite[] sprites_item;

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

        // todo: 先生成一个 item 列表 含所有物品



        button_sort.onClick.AddListener(() => {
            player.bagInventory.Sort();
        });

        button_prev.onClick.AddListener(() => {
            // 随便生成一些 item for test
            var bag = player.bagInventory;
            bag.Clear();
            for (int rowIndex = 0; rowIndex < bag.numRows; rowIndex++) {
                for (int colIndex = 0; colIndex < bag.numCols; colIndex++) {
                    if (Random.value > 0.5f) {
                        var id = Random.Range(0, sprites_item.Length);
                        var quality = (ItemQualities)Random.Range(0, sprites_bg.Length);
                        //new BagItem(bag, id, quality, 1, rowIndex, colIndex);
                    }
                }
            }
        });

        button_next.onClick.AddListener(() => {
            // 随便生成一些 item for test
            var bag = player.bagChar;
            bag.Clear();
            for (int rowIndex = 0; rowIndex < bag.numRows; rowIndex++) {
                for (int colIndex = 0; colIndex < bag.numCols; colIndex++) {
                    if (!bag.masks[rowIndex * bag.numCols + colIndex]) {
                        var id = Random.Range(0, sprites_item.Length);
                        var quality = (ItemQualities)Random.Range(0, sprites_bg.Length);
                        //new BagItem(bag, id, quality, 1, rowIndex, colIndex);
                    }
                }
            }
        });

        // inits
        Inputs.Init();
        Res.Init(sprites_bg, sprites_item);
        Cfg.Init();
        GO.Init(material, 50000);
        player = Player.instance;
        var p = go_bag_inventory.transform.position;
        player.bagInventory.Init(7, 10, 132, p.x, p.y);
        p = go_bag_char.transform.position;
        player.bagChar.Init(4, 4, 132, p.x, p.y);
        player.bagChar.SetMasks(1, 2, 5, 6, 9, 10);
    }

    void Update() {
        Inputs.Update();

        Env.timePool += Time.deltaTime;
        if (Env.timePool > Env.frameDelay) {
            Env.timePool -= Env.frameDelay;
            ++Env.frameNumber;
            Env.time = Env.frameNumber * Env.frameDelay;

            // todo: other logic 

            player.bagChar.Update();
            player.bagInventory.Update();
        }

        Draw();
    }

    internal void Draw() {
        text_health.text = player.health.ToString();
        text_damage.text = player.damage.ToString();
        text_defence.text = player.defence.ToString();
        text_speed.text = player.speed.ToString();

        player.bagChar.Draw();
        player.bagInventory.Draw();
    }
}
