using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Layer { UP, DOWN }
public enum State { ENTER, EXIT }

public class Cart : MonoBehaviour
{
    public Transform upHighLight;
    public Transform downHighLight;

    List<GameObject> upLayerObject = new List<GameObject>();
    List<GameObject> downLayerObject = new List<GameObject>();


    public void EventSet(Layer _layer, State _state, GameObject _target)
    {
        switch (_layer, _state)
        {
            case (Layer.UP, State.ENTER):
                if (!upLayerObject.Contains(_target))
                    upLayerObject.Add(_target);
                break;
            case (Layer.UP, State.EXIT):
                if (upLayerObject.Contains(_target))
                    upLayerObject.Remove(_target);
                break;
            case (Layer.DOWN, State.ENTER):
                if (!downLayerObject.Contains(_target))
                    downLayerObject.Add(_target);
                break;
            case (Layer.DOWN, State.EXIT):
                if (downLayerObject.Contains(_target))
                    downLayerObject.Remove(_target);
                break;
        }
    }

    public bool CheckEvent(Layer _layer, State _state, Transform _target)
    {
        GameObject target = _target.gameObject;
        switch (_layer, _state)
        {
            case (Layer.UP, State.ENTER):
                return upLayerObject.Contains(target);
            case (Layer.UP, State.EXIT):
                return !upLayerObject.Contains(target);
            case (Layer.DOWN, State.ENTER):
                return downLayerObject.Contains(target);
            case (Layer.DOWN, State.EXIT):
                return !downLayerObject.Contains(target);
        }

        Debug.Log("???");
        return false;
    }
}
