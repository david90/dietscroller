using UnityEngine;
using System.Collections;

public class DietScroller : MonoBehaviour {
    public Transform panel;
    public int total = 100;
    public int bundle = 3;
    public GameObject itemPrefab;
    public float posX = 0f;
	public float posY = 0f;
    public float cellWidth = 102;
    public float cellHeight = 102;

	public enum ScrollMode { Horizontal, Vertical };
	public ScrollMode scrollMode;

    GameObject[] itemList;

    void Start()
    {
		// Update panel's scale
		UIDraggablePanel draggablePanel = panel.GetComponent<UIDraggablePanel> ();

        BoxCollider col = collider as BoxCollider;

		if (scrollMode == ScrollMode.Horizontal)
		{
			draggablePanel.scale = new Vector3(1f, 0f, 1f); // allow horizontal scroll
			col.size = new Vector3 (cellWidth * total, cellHeight, 1f);
			col.center = new Vector3 (cellWidth * (total - 1) / 2f, 0f, 0f);
		} else if (scrollMode == ScrollMode.Vertical)
		{
			draggablePanel.scale = new Vector3(0f, 1f, 1f); // allow vertical scroll
			col.size = new Vector3 (cellWidth, cellHeight * total, 1f);
			col.center = new Vector3 (0f, - cellHeight * (total - 1) / 2f, 0f);
		}

        itemList = new GameObject[total];
        AddItem(0);
        AddItem(total - 1);
    }

    void AddItem(int seq)
    {
        if (seq < 0 || seq >= total) return;
        if (itemList[seq]) return;

        GameObject go = NGUITools.AddChild(gameObject, itemPrefab);
		UpdateItem (go, seq);

		// Save in list
        itemList[seq] = go;
    }

	void UpdateItem(GameObject go, int seq)
	{
		// Update Content
		UILabel label = go.GetComponentInChildren<UILabel>();
		string str = (seq + 1).ToString("0000");
		label.text = str;
		go.name = "item_" + str;

		// Update Position
		Item item = go.GetComponent<Item> ();
		if (scrollMode == ScrollMode.Horizontal)
		{
			go.transform.localPosition = Vector3.right * seq * cellWidth;
			item.min = -cellWidth;
			item.max = (bundle) * cellWidth;
		} else if (scrollMode == ScrollMode.Vertical)
		{
			go.transform.localPosition = Vector3.down * seq * cellHeight;
			item.min = -cellHeight;
			item.max = (bundle) * cellHeight;
		}

		if (seq == 0 || seq == total - 1) item.on = true;
	}

    void Update()
    {
		int pos = 0;
		if (scrollMode == ScrollMode.Horizontal) 
		{
			posX = panel.localPosition.x;
			pos = Mathf.Abs (Mathf.FloorToInt (posX / cellWidth));
		} else if (scrollMode == ScrollMode.Vertical) 
		{
			posY = panel.localPosition.y;
			pos = Mathf.Abs (Mathf.FloorToInt (posY / cellHeight));
		}

		// Batch Update
		for (int i = -1; i < bundle; i++)
		{
			int seq = i + pos;
			AddItem (i + pos);
		}
    }
}
