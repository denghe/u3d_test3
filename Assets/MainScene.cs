using UnityEngine;
using TMPro;
using System.Reflection;
using UnityEngine.UI;

/// <summary>
/// 这个类的成员当前只支持 2 种数据类型：int, float，且需要用 Range 标注范围
/// </summary>
public class Foo {
    [Range(0, 100)]
    public int intValue = 12;

    [Range(0, 1)]
    public float floatValue = 0.345f;
}

public class MainScene : MonoBehaviour {

    public GameObject prefab_property;

    public Foo foo = new();

    void Start() {
        // todo: 生成到 scroll view? 弄进一个 panel ? 模拟成 window ? 可以 show hide ?

        var canvas = GameObject.Find("Canvas");
        var fs = foo.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        float y = 0;
        foreach (var f in fs) {
            var r = (RangeAttribute)f.GetCustomAttributes(false)[0];
            var p = Instantiate(prefab_property);
            var t = (RectTransform)p.transform;
            t.SetParent(canvas.transform);
            t.anchoredPosition = new Vector2(0, y);
            y -= 30 + 3;
            var label = t.Find("Label").gameObject.GetComponent<TextMeshProUGUI>();
            var slider = t.Find("Slider").gameObject.GetComponent<Slider>();
            var input = t.Find("Input").gameObject.GetComponent<TMP_InputField>();
            label.text = f.Name;
            slider.minValue = r.min;
            slider.maxValue = r.max;
            var isInt = f.FieldType == typeof(int);
            slider.wholeNumbers = isInt;
            input.contentType = isInt ? TMP_InputField.ContentType.IntegerNumber : TMP_InputField.ContentType.DecimalNumber;
            var ov = f.GetValue(foo);
            input.text = ov.ToString();
            if (isInt) {
                slider.value = (int)ov;
                slider.onValueChanged.AddListener(v => {
                    f.SetValue(foo, (int)v);
                    input.text = v.ToString();
                });
            } else {
                slider.value = (float)ov;
                slider.onValueChanged.AddListener(v => {
                    f.SetValue(foo, v);
                    input.text = v.ToString();
                });
            }
            input.onValueChanged.AddListener(v => {
                if (string.IsNullOrEmpty(v)) return;
                if (isInt) {
                    var d = int.Parse(v);
                    if (d < r.min) d = (int)r.min;
                    else if (d > r.max) d = (int)r.max;
                    slider.value = d;
                    f.SetValue(foo, d);
                } else {
                    var d = float.Parse(v);
                    if (d < r.min) d = r.min;
                    else if (d > r.max) d = r.max;
                    slider.value = d;
                    f.SetValue(foo, d);
                }
            });
        }

    }


    void Update() {

    }
}

//var gs = gameObject.scene.GetRootGameObjects();
//foreach (var g in gs) {
//    Debug.Log(g.name + " " + g.GetType());
//}

//foreach (var a in f.GetCustomAttributes(false)) {
//    if (a is RangeAttribute) {
//        var range = (RangeAttribute)a;
//        //range.min
//    }
//}

//void Start() {
//    // 获取当前GameObject的所有直接子节点
//    Transform[] directChildren = transform.GetComponentsInChildren<Transform>();
//    foreach (Transform child in directChildren) {
//        Debug.Log("直接子节点: " + child.name);
//    }

//    // 递归遍历整个对象树
//    TraverseObjectTree(transform);
//}

//void TraverseObjectTree(Transform parent) {
//    foreach (Transform child in parent) {
//        Debug.Log("子节点: " + child.name);
//        // 递归遍历当前子节点的子节点
//        TraverseObjectTree(child);
//    }
//}
