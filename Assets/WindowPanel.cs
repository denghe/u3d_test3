using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(UnityEngine.UI.Panel ???))]
public class WindowPanel : MonoBehaviour, IDragHandler, IPointerDownHandler {
    public PointerEventData.InputButton mouseDragButton;
    public int borderSnapSize = 10;
    private Vector3 mouseDragOffset;
    private RectTransform trans;
    private Camera cam;
    private static HashSet<WindowPanel> allWindows = new();

    private void Awake() {
        cam = Camera.main;
        trans = (RectTransform)transform;
        allWindows.Add(this);
    }

    public void OnDrag(PointerEventData eventData) {
        if (eventData.button != mouseDragButton) return;
        trans.position = Input.mousePosition - mouseDragOffset;
        TrapToScreen();
        SnapEachOther();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button != mouseDragButton) return;
        mouseDragOffset = Input.mousePosition - trans.position;
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