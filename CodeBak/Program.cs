using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

// 模拟 1只怪 不断移动，每隔 几帧 会 晕几帧

namespace Battle {

    public enum ActionTypes : int {
        Move,
        Stun,
        // ...
        MaxValue
    }

    /*********************************************************************************************/
    // base data struct

    [StructLayout(LayoutKind.Sequential)]
    public struct Action {
        public ActionTypes type;
        public int _0, _1, _2;
    }

    /*********************************************************************************************/
    // Actions

    [StructLayout(LayoutKind.Sequential)]
    public struct Action_Move {
        public ActionTypes type;
        public float movementSpeed;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Action_Stun {
        public ActionTypes type;
        public float timeout;
    }

    /*********************************************************************************************/
    // runtime env

    public class Scene {
        public const float framePerSeconds = 60f;
        public const float frameDelay = 1f / framePerSeconds;
        public const float stunCastDelay = 0.3f;

        public StringBuilder sb = new();    // for output
        public float time;
        public Monster monster;
        public float stunCastTime;

        public void Init() {
            time = 1000;
            monster = new Monster();
            monster.Init(this);
        }

        public int Update() {
            time += frameDelay;
            StunMonster();
            return monster.Update();
        }

        public bool StunMonster() {
            if (stunCastTime >= time) return false;
            stunCastTime = time + stunCastDelay;
            if (monster.ActionExists(ActionTypes.Stun)) return false;
            monster.ActionTryRemove(ActionTypes.Move);
            monster.Add_Action_Stun(0.1f);
            return true;
        }

    }

    /*********************************************************************************************/
    // logic class

    public unsafe class Monster {
        public Scene scene;
        public float x;
        // ...

        /*********************************************************************************************/
        // action base

        public ulong actionFlags;       // bit exists checker
        public int actionsLen;          // actionArray's data len
        public Action[] actionArray;    // Length == capacity
        public GCHandle actionsHandle;  // point to actionArray[0]
        public Action* actions;         // point to actionArray[0]

        public bool ActionExists(ActionTypes bt) {
            return (actionFlags & ((ulong)1 << (int)bt)) > 0;
        }
        public void ActionsSetFlag(ActionTypes bt) {
            Debug.Assert(!ActionExists(bt));
            actionFlags |= ((ulong)1 << (int)bt);
        }
        public void ActionsClearFlag(ActionTypes bt) {
            Debug.Assert(ActionExists(bt));
            actionFlags &= ~((ulong)1 << (int)bt);
        }

        /// <summary>
        /// return -1 mean not found
        /// </summary>
	    public int ActionFind(ActionTypes bt) {
		    if (!ActionExists(bt)) return -1;
		    for (var i = actionsLen - 1; i >= 0; --i) {
			    if (actions[i].type == bt) return i;
		    }
		    return -1;
	    }

        /// <summary>
        /// call after ActionFind
        /// </summary>
        public void ActionRemove(int index) {
            //actionFlags &= ~(1UL << (int)actions[index].type);
            ActionsClearFlag(actions[index].type);
            actions[index] = actions[--actionsLen];
        }

        /// <summary>
        /// return false mean not found
        /// </summary>
        public bool ActionTryRemove(ActionTypes bt) {
            if (!ActionExists(bt)) return false;
            for (int index = actionsLen - 1; index >= 0; --index) {
                if (actions[index].type == bt) {
                    ActionRemove(index);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// initialize action system
        /// </summary>
        public void ActionInit() {
            Debug.Assert(actionArray == null);
            var cap = 6;
            actionArray = new Action[cap];
            actionsHandle = GCHandle.Alloc(actionArray, GCHandleType.Pinned);
            actions = (Action*)actionsHandle.AddrOfPinnedObject();
            actionsLen = 0;
            actionFlags = 0;
        }

        /*********************************************************************************************/
        // action add

        public bool Add_Action_Move(float movementSpeed) {
            // conflict checks
            Debug.Assert(!ActionExists(ActionTypes.Stun));
            Debug.Assert(!ActionExists(ActionTypes.Move));
            // ...

            ActionsSetFlag(ActionTypes.Move);
            var o = (Action_Move*)&actions[actionsLen++];
            o->type = ActionTypes.Move;
            o->movementSpeed = movementSpeed;
            return true;
        }

        public bool Add_Action_Stun(float effectSeconds) {
            // conflict checks
            Debug.Assert(!ActionExists(ActionTypes.Stun));
            Debug.Assert(!ActionExists(ActionTypes.Move));
            // ...

            ActionsSetFlag(ActionTypes.Stun);
            var o = (Action_Stun*)&actions[actionsLen++];
            o->type = ActionTypes.Stun;
            o->timeout = scene.time + effectSeconds;
            return true;
        }

        // ...

        /*********************************************************************************************/
        // action handle

        public void HandleAction_Move(Action_Move* o, int actionIndex) {
            x += o->movementSpeed;
        }

        public void HandleAction_Stun(Action_Stun* o, int actionIndex) {
            if (o->timeout < scene.time) {
                ActionRemove(actionIndex);  // suicide
            }
        }

        // ...

        /*********************************************************************************************/

        public void Init(Scene scene_) {
            scene = scene_;
            ActionInit();
            TryAddBaseActions();
        }

        public void TryAddBaseActions() {
            // action make
            if (!ActionExists(ActionTypes.Stun)
            && !ActionExists(ActionTypes.Move)
            ) {
                Add_Action_Move(1);
            }
        }

        public int Update() {
            for (int index = actionsLen - 1; index >= 0; --index) {
                var b = &actions[index];
                switch (b->type) {
                    case ActionTypes.Move: HandleAction_Move((Action_Move*)b, index); break;
                    case ActionTypes.Stun: HandleAction_Stun((Action_Stun*)b, index); break;
                    // ...
                }
            }
            TryAddBaseActions();
            return 0;
        }

    }

}


/*********************************************************************************************/
// program entry

public static class MainClass {
    public static void Main() {
        var scene = new Battle.Scene();
        scene.Init();
#if true
		for (int i = 0; i < 100; i++) {
			scene.Update();
            scene.sb.AppendLine(scene.time + "\t" + scene.monster.actionArray[0].type + "\tx = " + scene.monster.x);
		}
#else
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 100000000; i++) {
            scene.Update();
        }
        scene.sb.AppendLine("secs = " + sw.ElapsedMilliseconds / 1000f);
        scene.sb.AppendLine(scene.time + "\tmonster.pos = " + scene.monster.x);
#endif
        Console.WriteLine(scene.sb.ToString());
    }
}

