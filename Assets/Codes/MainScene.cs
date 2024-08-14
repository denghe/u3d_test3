using UnityEngine;
using TMPro;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// 这个类的成员当前只支持 2 种数据类型：int, float，且需要用 Range 标注范围
/// </summary>
public class Foo {
    [Range(0, 100)]
    public int intValue = 12;

    [Range(0, 1)]
    public float floatValue = 0.345f;

    [Range(0, 100)]
    public int intValue1 = 12;

    [Range(0, 1)]
    public float floatValue1 = 0.345f;

    [Range(0, 100)]
    public int intValue2 = 12;

    [Range(0, 1)]
    public float floatValue2 = 0.345f;

    [Range(0, 100)]
    public int intValue3 = 12;

    [Range(0, 1)]
    public float floatValue3 = 0.345f;
}

public partial class Helpers {
    public static void GenUI_PropsTo(string containerName, GameObject prefabProperty, System.Object dataClass) {
        var container = GameObject.Find(containerName);
        var fs = dataClass.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        float y = 0;
        foreach (var f in fs) {
            var r = (RangeAttribute)f.GetCustomAttributes(false)[0];
            var p = GameObject.Instantiate(prefabProperty);
            var t = (RectTransform)p.transform;
            t.SetParent(container.transform);
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
            var ov = f.GetValue(dataClass);
            input.text = ov.ToString();
            if (isInt) {
                slider.value = (int)ov;
                slider.onValueChanged.AddListener(v => {
                    f.SetValue(dataClass, (int)v);
                    input.text = v.ToString();
                });
            }
            else {
                slider.value = (float)ov;
                slider.onValueChanged.AddListener(v => {
                    f.SetValue(dataClass, v);
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
                    f.SetValue(dataClass, d);
                }
                else {
                    var d = float.Parse(v);
                    if (d < r.min) d = r.min;
                    else if (d > r.max) d = r.max;
                    slider.value = d;
                    f.SetValue(dataClass, d);
                }
            });
        }

        var ct = ((RectTransform)container.transform);
        ct.sizeDelta = new Vector2(ct.sizeDelta.x, -y);
    }
}

public class MainScene : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler/*, IDragHandler, IPointerDownHandler*/ {
    public GameObject prefab_property;

    public Foo foo = new();

    private TextMeshProUGUI fpsText;    // FpsText
    private TextMeshProUGUI richText;    // RichText
    private Button button; // Button
    private Image image; // Image

    private float lastSecs, drawCounter;

    //public void OnDrag(PointerEventData eventData) {
    //    Debug.Log("MainScene OnDrag " + eventData.pointerDrag.name);
    //}

    public void OnPointerClick(PointerEventData eventData) {
        //Debug.Log(eventData.position);
        var idx = TMP_TextUtilities.FindIntersectingLink(richText, eventData.position, null);
        if (idx != -1) {
            var info = richText.textInfo.linkInfo[idx];
            Debug.Log(info.GetLinkID());

            // todo: 点击超链接之后 弹出临时的 info panel 可以多层, 点击 info panel 的关闭或外围来 关闭它  可级联关闭 也就是说 只要没有点中最上层 info panel 就关所有
        }
    }

    //public void OnPointerDown(PointerEventData eventData) {  
    //    Debug.Log("MainScene OnPointerDown " + eventData.position);
    //}

    public void OnPointerEnter(PointerEventData eventData) {
        // todo: 鼠标悬停时弹出提示？移走后消失？
        Debug.Log("enter " + eventData.pointerEnter.name);
        //if (eventData.pointerEnter == button) {
        //    Debug.Log("enter btn");
        //}
    }

    public void OnPointerExit(PointerEventData eventData) {
        //if (eventData.pointe)
        Debug.Log("exit " + eventData.pointerEnter.name);
    }

    void Start() {
        Helpers.GenUI_PropsTo("Content", prefab_property, foo);
        fpsText = GameObject.Find("FpsText").GetComponent<TextMeshProUGUI>();
        richText = GameObject.Find("RichText").GetComponent<TextMeshProUGUI>();
        button = GameObject.Find("Button").GetComponent<Button>();
        image = GameObject.Find("Image").GetComponent<Image>();
        image.AddComponent<Dragger>();
    }

    void Update() {
        ++drawCounter;
        var nowSecs = Time.time;
        var elapsedSecs = nowSecs - lastSecs;
        if (elapsedSecs >= 1) {
            lastSecs = nowSecs;
            fpsText.text = (drawCounter / elapsedSecs).ToString();
            drawCounter = 0;
        }
    }
}

/// <summary>
/// 用于附加到 gameObject 上提供拖拽功能. 当前只支持 pivot 为 0.5, 0.5( 除非不要 snap )
/// </summary>
public class Dragger : MonoBehaviour, IDragHandler, IPointerDownHandler {
    public const int borderSnapSize = 10;
    private Vector2 mouseDragOffset;
    private RectTransform trans;
    private Camera cam;
    private static HashSet<Dragger> allWindows = new();

    private void Awake() {
        cam = Camera.main;
        trans = (RectTransform)transform;
        allWindows.Add(this);
    }

    public void OnDrag(PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        trans.position = eventData.position - mouseDragOffset;
        TrapToScreen();
        SnapEachOther();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        mouseDragOffset = eventData.position - (Vector2)trans.position;
        trans.SetAsLastSibling();
    }

    void TrapToScreen() {
        var rp = (Vector3)trans.rect.position;
        var diffMin = trans.position + rp;
        var diffMax = (Vector3)cam.pixelRect.size - trans.position + rp;
        if (diffMin.x < borderSnapSize) {
            trans.position -= new Vector3(diffMin.x, 0);
        }
        if (diffMin.y < borderSnapSize) {
            trans.position -= new Vector3(0, diffMin.y);
        }
        if (diffMax.x < borderSnapSize) {
            trans.position += new Vector3(diffMax.x, 0);
        }
        if (diffMax.y < borderSnapSize) {
            trans.position += new Vector3(0, diffMax.y);
        }
    }

    void SnapEachOther() {
        foreach (var w in allWindows) {
            if (w == this) continue;
            if (w.gameObject.activeInHierarchy) {
                var diffMin = w.trans.position - (Vector3)(w.trans.rect.position + trans.rect.position) - trans.position;
                var diffMax = w.trans.position + (Vector3)(w.trans.rect.position + trans.rect.position) - trans.position;
                if (Mathf.Abs(diffMin.x) < borderSnapSize) {
                    trans.position += new Vector3(diffMin.x, 0, 0);
                }
                if (Mathf.Abs(diffMin.y) < borderSnapSize) {
                    trans.position += new Vector3(0, diffMin.y, 0);
                }
                if (Mathf.Abs(diffMax.x) < borderSnapSize) {
                    trans.position += new Vector3(diffMax.x, 0, 0);
                }
                if (Mathf.Abs(diffMax.y) < borderSnapSize) {
                    trans.position += new Vector3(0, diffMax.y, 0);
                }
            }
        }
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