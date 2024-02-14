using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public static DragManager instance;


    public GameObject draggingObject;
    public bool isDragging
    {
        get
        {
            return draggingObject != null;
        }
    }

    public bool isSnapped = false;

    private void Awake()
    {
        instance = this;
    }

    public void StartDrag(GameObject obj)
    {
        draggingObject = obj;
    }

    public void EndDrag()
    {
        draggingObject = null;
    }
}
