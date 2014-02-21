using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    Transform tr;
    public float min = -100, max = 500f;
	
    [HideInInspector]
    public bool on = false;
    Transform panel;

    void Start()
    {
        tr = transform;
		panel = NGUITools.FindInParents<UIDraggablePanel> (gameObject).transform;
    }

    void OnOutside()
    {
        Destroy(gameObject);
    }

	bool IsOutside ()
	{
		Vector3 pos = tr.localPosition + panel.localPosition;
		return (pos.y > max || pos.y < min) || (pos.x > max || pos.x < min);
	}

    void Update()
    {
        if (IsOutside())
        {
            if (!on) OnOutside();
        }
    }
}
