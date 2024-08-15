using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// 用于附加到 UI 上提供拖拽功能. 当前只支持 pivot 为 0.5, 0.5( 除非不要 snap )
/// 只能拖拽没有鼠标交互的地方( 因为不会响应 OnPointerDown 而直接响应 OnDrag )
/// </summary>
public class UIDragger : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {
    public const int borderSnapSize = 10;
    private Vector2 mouseDragOffset;
    private RectTransform trans;
    private Camera cam;
    private static HashSet<UIDragger> allWindows = new();
    private bool dragging;

    public void Init() {
        cam = Camera.main;
        trans = (RectTransform)transform;
        allWindows.Add(this);
    }

    /// <summary>
    /// 当前子控件已经 接管 了 pointer down 事件时，这里就收不到了。但当鼠标移出子控件区域时，却会触发 OnDrag
    /// 会导致无法正确记录 mouseDragOffset 从而产生奇怪的拖动效果，故用 dragging 来避免 OnDrag 在这种情况生效
    /// </summary>
    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        dragging = true;
        mouseDragOffset = eventData.position - (Vector2)trans.position;
        trans.SetAsLastSibling();
    }

    public void OnPointerUp(PointerEventData eventData) {
        dragging = false;
    }

    public void OnDrag(PointerEventData eventData) {
        if (!dragging) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;

        trans.position = eventData.position - mouseDragOffset;
        TrapToScreen();
        SnapEachOther();
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

