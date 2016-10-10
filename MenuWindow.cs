using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// Menu window (attached MunuWindow of Panel)
//public class MenuWindow : MonoBehaviour {
public class MenuWindow : Selectable {

    public static MenuWindow m_Instance = null;
    //private static GameObject m_menuWindowPrefab;
    private GameObject m_menuWindowObj;
    //private const string kMenuWindowPath = "prefabs\\MenuWindow";
    //private static Canvas m_mainCanvas;

    // Button component
    // (Prefab にEditor上からPrefab を設定するのは無理？)
    //public GameObject[] Buttons;
    private const string kMenuButtonPath = "prefabs\\";
    private readonly string[] kMenuButtonNames = new string[2] 
                    { "MenuButton0", "MenuButton1" };
    private GameObject[] m_menuButtonPrefab;
    private Button[] m_cloneButtons;
    private EventSystem m_refEventSystem;

    // Key 記憶
    private int m_lastIndex = 0;



#if false
    // note: Thread unsafe
    public static MenuWindow GetInstance()
    {
        if(m_instance == null)
        {
            m_instance = new MenuWindow();

            //　MenuWindow のPrefab読込み
            if (m_menuWindowPrefab == null)
            {
                m_menuWindowPrefab = (GameObject)Resources.Load(kMenuWindowPath);
                if (m_menuWindowPrefab == null) { Debug.Log("Menu windows prefab is not found"); }
            }
            // MainCanvasの参照
            m_mainCanvas = GameObject.FindObjectOfType<Canvas>();
            if (m_mainCanvas == null) { Debug.Log("Main Canvas is not found"); }
        }
        return m_instance;
    }

    private MenuWindow()
    { }
#endif

    // ウィンドウの各種設定
    public void SetupWindow(Canvas parent, Button[] buttons)
    {
        // 親を設定
        this.transform.SetParent(parent.transform);
        // 位置とスケール
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0.0f, 0.0f);
        rt.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // ボタンの数によって高さを設定(TODO)

        // ボタンの設定
        if(buttons != null && buttons.Length > 0)
        {
            m_cloneButtons = buttons;
            RectTransform brt;
            for (int i = 0; i < m_cloneButtons.Length; ++i)
            {
                m_cloneButtons[i].transform.SetParent(this.transform);
                brt = m_cloneButtons[i].GetComponent<RectTransform>();
                brt.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                brt.anchoredPosition = new Vector2(8.0f, -28 - i * 30);
            }
        }
        else
        {
            // ここで抜けてしまう
            Debug.Log("メニュー画面のボタンが設定されていません");
            return;
        }

        // ボタンの上下左右キー遷移の設定
        SetButtonTransition(m_cloneButtons);

        // ボタンのOnClick設定
        ItemWindowMaker wmItem = GameObject.Find("WindowManager").GetComponent<ItemWindowMaker>();
        m_cloneButtons[0].onClick.AddListener(() => {
            Debug.Log("メニューボタン1 が押されました");
            //Debug.Log("My Name is " + gameObject.name);
            // デリゲートメソッドはあくまでMenuWindow(Clone)らしい

            // MenuWindow をNot interactiveにして、ItemWindow表示(選択は内部で？)
            SetInteractive(false);
            wmItem.ShowItemWindow(this);
            // (TODO) アイテムを開くときメンバーを選ぶようにする？

        });

        // 自動でボタンを選択状態にする
        if (m_refEventSystem == null)
        {
            m_refEventSystem = GameObject.FindObjectOfType<EventSystem>();
        }
        //EventSystem.current.SetSelectedGameObject(null);
        m_refEventSystem.SetSelectedGameObject(m_cloneButtons[0].gameObject);
    }

    // ボタンの上下左右キー遷移の設定
    private void SetButtonTransition(Button[] btn)
    {
        if (btn.Length == 0) return;

       
    }

#if false
    public void DisplayMenuWindow()
    {
        if(m_menuWindowPrefab == null || m_mainCanvas == null)
        { Debug.Log("Prefab or MainCanbus is not found"); return; }

        // window生成
        if(m_menuWindowObj == null) {
            m_menuWindowObj = Instantiate(m_menuWindowPrefab);
        }
        m_menuWindowObj.transform.parent = m_mainCanvas.transform;
        RectTransform rt = m_menuWindowObj.GetComponent<RectTransform>();

        // Set position and scale
        rt.anchoredPosition = new Vector2(0.0f, 0.0f);
        rt.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Create Button
        //Button bt = obj.transform.GetComponentInChildren<Button>();
        Button[] bts = m_menuWindowObj.transform.GetComponentsInChildren<Button>();
        if (bts.Length == 0) { Debug.Log("Button component is not found"); }
        else
        {
            foreach (Button bt in bts)
                Debug.Log(bt.name);

            string[] buttonString = new string[] { "逃げる", "防御", "魔法" };
            Button[] newButtons = new Button[3];
            RectTransform workR;
            for (int i = 0; i < 3; ++i)
            {
                newButtons[i] = Instantiate(bts[0]);
                //newButtons[i].transform.parent = obj.transform;
                newButtons[i].transform.SetParent(m_menuWindowObj.transform);
                workR = newButtons[i].GetComponent<RectTransform>();
                workR.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                workR.anchoredPosition = new Vector2(8.0f, -88 - i * 30);

                newButtons[i].GetComponentInChildren<Text>().text = buttonString[i];
                //workRt.sizeDelta = 
            }

        }

        // Button Prefabの読込み
        if(m_menuButtonPrefab == null)
        {
            m_menuButtonPrefab = new GameObject[kMenuButtonNames.Length];
            for(int i=0; i<kMenuButtonNames.Length; ++i)
            {
                m_menuButtonPrefab[i] = (GameObject)Resources.Load(kMenuButtonPath + kMenuButtonNames[i]);
                if (m_menuButtonPrefab[i] == null)
                    Debug.Log("MenuButton のPrefabが見つかりません, path=" + kMenuButtonPath + kMenuButtonNames[i]);
            }
        }
        // Button を表示
        RectTransform workRt;
        if (m_cloneButtons == null)
            m_cloneButtons = new Button[kMenuButtonNames.Length];
        for(int i=0; i < kMenuButtonNames.Length; ++i)
        {
            m_cloneButtons[i] = Instantiate(m_menuButtonPrefab[i]).GetComponent<Button>();
            m_cloneButtons[i].transform.SetParent(m_menuWindowObj.transform);
            workRt = m_cloneButtons[i].GetComponent<RectTransform>();
            workRt.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            workRt.anchoredPosition = new Vector2(8.0f, -28 - i * 30);
        }
    }
#endif

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        Debug.Log("MenuWindow が選択されました");
        //Debug.Log(m_cloneButtons.Length);
        //if(m_refEventSystem == null)
        //{
        //    m_refEventSystem = GameObject.FindObjectOfType<EventSystem>();
        //}
        //// Selectを解除して、ボタンに移す
        ////EventSystem.current.SetSelectedGameObject(null);
        //m_refEventSystem.SetSelectedGameObject(m_cloneButtons[0].gameObject);

    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        Debug.Log("Menu window の選択が解除されました");
        // キー位置記憶
        // m_lastIndex = currentIndex;

    }

    public override void Select()
    {
        Debug.Log("MenuWindow が選択されました");
        base.Select();

        // インタラクティブにしてキー位置のボタンを選択状態に
        SetInteractive(true);
        m_refEventSystem.SetSelectedGameObject(m_cloneButtons[m_lastIndex].gameObject);
    }


    public void SetInteractive(bool val)
    {
        // 子の設定
        foreach (Transform bt in transform)
        {
            Selectable sl = bt.GetComponent<Selectable>();
            if (sl != null)
                sl.interactable = val;
        }

        this.interactable = val;
    }

    // Use this for initialization
    //   void Start () {

    //}

    // Update is called once per frame
    //void Update () {

    //}
}
