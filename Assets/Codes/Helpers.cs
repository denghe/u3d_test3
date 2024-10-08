﻿using UnityEngine;
using TMPro;
using System.Reflection;
using UnityEngine.UI;

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
            } else {
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
                } else {
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