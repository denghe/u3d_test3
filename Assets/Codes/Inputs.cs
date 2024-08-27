using UnityEngine;

/// <summary>
/// 玩家输入处理系统( 基于 new input system )
/// </summary>
public static class Inputs {

    public const float sqrt2 = 1.414213562373095f;
    public const float sqrt2_1 = 0.7071067811865475f;
    public static InputActions ia;
    public static bool kbMovingUp;                      // 键盘 W/UP
    public static bool kbMovingDown;                    // 键盘 S/Down
    public static bool kbMovingLeft;                    // 键盘 A/Left
    public static bool kbMovingRight;                   // 键盘 D/Right

    /// <summary>
    /// 玩家是否正在使用键盘( false: 手柄 )
    /// </summary>
    public static bool usingKeyboard;

    /// <summary>
    /// 玩家是否按下了跳键( 键盘 Space 或 手柄按钮 A / X )
    /// </summary>
    public static bool jumping;

    // todo: more action?

    /// <summary>
    /// 玩家是否正在移动( 键盘 ASDW 或 手柄左 joy 均能触发 )
    /// </summary>
    public static bool moving;

    /// <summary>
    /// 归一化之后的移动矢量( 读前先判断 moving )
    /// </summary>
    public static Vector2 moveVector;

    /// <summary>
    /// 最后移动朝向角度( 根据 moveVector 计算所得 )
    /// </summary>
    public static float moveRadians;

    /// <summary>
    /// 当前鼠标是否覆盖在 UI 上( 需要 Canvas UI 代码实现 IPointerEnterHandler, IPointerExitHandler 并修改这里 )
    /// </summary>
    public static bool mouseCoveredUI;

    /// <summary>
    /// 当前鼠标的屏幕坐标
    /// </summary>
    public static Vector2 mousePosition, mousePositionWithoutUI;

    /// <summary>
    /// 当前鼠标的 camera 坐标
    /// </summary>
    public static Vector2 mousePositionInCamera, mousePositionWithoutUIInCamera;

    /// <summary>
    /// 当前鼠标按键状态 -- left button
    /// </summary>
    public static bool mouseButtonLeftDown;

    /// <summary>
    /// 当前鼠标按键状态 -- right button
    /// </summary>
    public static bool mouseButtonRightDown;

    // todo: more mouse info

    /// <summary>
    /// 场景 Start 时调用
    /// </summary>
    public static void Init() {
        ia = new InputActions();
        ia.Enable();

        // keyboard
        ia.Player.KBJump.started += c => {
            usingKeyboard = true;
            jumping = true;
        };
        ia.Player.KBJump.canceled += c => {
            usingKeyboard = true;
            jumping = false;
        };

        ia.Player.KBMoveUp.started += c => {
            usingKeyboard = true;
            kbMovingUp = true;
        };
        ia.Player.KBMoveUp.canceled += c => {
            usingKeyboard = true;
            kbMovingUp = false;
        };

        ia.Player.KBMoveDown.started += c => {
            usingKeyboard = true;
            kbMovingDown = true;
        };
        ia.Player.KBMoveDown.canceled += c => {
            usingKeyboard = true;
            kbMovingDown = false;
        };

        ia.Player.KBMoveLeft.started += c => {
            usingKeyboard = true;
            kbMovingLeft = true;
        };
        ia.Player.KBMoveLeft.canceled += c => {
            usingKeyboard = true;
            kbMovingLeft = false;
        };

        ia.Player.KBMoveRight.started += c => {
            usingKeyboard = true;
            kbMovingRight = true;
        };
        ia.Player.KBMoveRight.canceled += c => {
            usingKeyboard = true;
            kbMovingRight = false;
        };

        // gamepad
        ia.Player.GPJump.started += c => {
            usingKeyboard = false;
            jumping = true;
        };
        ia.Player.GPJump.canceled += c => {
            usingKeyboard = false;
            jumping = false;
        };

        ia.Player.GPMove.started += c => {
            usingKeyboard = false;
            moving = true;
        };
        ia.Player.GPMove.performed += c => {
            usingKeyboard = false;
            moving = true;
        };
        ia.Player.GPMove.canceled += c => {
            usingKeyboard = false;
            moving = false;
        };

        // mouse
        ia.Player.MPoint.performed += o => {
            mousePosition = o.ReadValue<Vector2>();
            mousePositionInCamera = Camera.main.ScreenToWorldPoint(mousePosition);
            if (!mouseCoveredUI) {
                mousePositionWithoutUI = mousePosition;
                mousePositionWithoutUIInCamera = mousePositionInCamera;
            }
        };

        ia.Player.MBLeft.started += o => {
            if (!mouseCoveredUI) {
                mouseButtonLeftDown = true;
            }
        };
        ia.Player.MBLeft.canceled += o => {
            mouseButtonLeftDown = false;
        };

        ia.Player.MBRight.started += o => {
            if (!mouseCoveredUI) {
                mouseButtonRightDown = true;
            }
        };
        ia.Player.MBRight.canceled += o => {
            mouseButtonRightDown = false;
        };

    }

    /// <summary>
    /// 场景 Update 开始处调用
    /// </summary>
    public static void Update() {

        if (usingKeyboard) {
            // 键盘需要每帧判断, 合并方向, 计算最终矢量
            if (!kbMovingUp && !kbMovingDown && !kbMovingLeft && !kbMovingRight
                || kbMovingUp && kbMovingDown && kbMovingLeft && kbMovingRight) {
                moveVector.x = 0f;
                moveVector.y = 0f;
                moving = false;
            } else if (!kbMovingUp && kbMovingDown && kbMovingLeft && kbMovingRight) {
                moveVector.x = 0f;
                moveVector.y = 1f;
                moving = true;
            } else if (kbMovingUp && !kbMovingDown && kbMovingLeft && kbMovingRight) {
                moveVector.x = 0f;
                moveVector.y = -1f;
                moving = true;
            } else if (kbMovingUp && kbMovingDown && !kbMovingLeft && kbMovingRight) {
                moveVector.x = 1f;
                moveVector.y = 0f;
                moving = true;
            } else if (kbMovingUp && kbMovingDown && kbMovingLeft && !kbMovingRight) {
                moveVector.x = -1f;
                moveVector.y = 0f;
                moving = true;
            } else if (kbMovingUp && kbMovingDown
                  || kbMovingLeft && kbMovingRight) {
                moveVector.x = 0f;
                moveVector.y = 0f;
                moving = false;
            } else if (kbMovingUp && kbMovingLeft) {
                moveVector.x = -sqrt2_1;
                moveVector.y = -sqrt2_1;
                moving = true;
            } else if (kbMovingUp && kbMovingRight) {
                moveVector.x = sqrt2_1;
                moveVector.y = -sqrt2_1;
                moving = true;
            } else if (kbMovingDown && kbMovingLeft) {
                moveVector.x = -sqrt2_1;
                moveVector.y = sqrt2_1;
                moving = true;
            } else if (kbMovingDown && kbMovingRight) {
                moveVector.x = sqrt2_1;
                moveVector.y = sqrt2_1;
                moving = true;
            } else if (kbMovingUp) {
                moveVector.x = 0;
                moveVector.y = -1;
                moving = true;
            } else if (kbMovingDown) {
                moveVector.x = 0;
                moveVector.y = 1;
                moving = true;
            } else if (kbMovingLeft) {
                moveVector.x = -1;
                moveVector.y = 0;
                moving = true;
            } else if (kbMovingRight) {
                moveVector.x = 1;
                moveVector.y = 0;
                moving = true;
            }
        } else {
            // 手柄不需要判断
            var v = ia.Player.GPMove.ReadValue<Vector2>();
            // v.Normalize();
            moveVector.x = v.x;
            moveVector.y = -v.y;
            // todo: playerMoving = 距离 > 死区长度 ?
        }

        if (moveVector.x != 0 || moveVector.y != 0) {
            moveRadians = Mathf.Atan2(moveVector.y, moveVector.x);
        }
    }
}
