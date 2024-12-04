using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("UIRoot").GetComponent<UIManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }

    }

    // Start is called before the first frame update
    List<IBaseDlg> dlgList = new List<IBaseDlg>();
    public BaseDlg<StartMenuDlg> startMenuDlg;

    void Awake()
    {
        startMenuDlg = BaseDlg<StartMenuDlg>.Instance;
    }
    protected virtual void OnEnable()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void AddDlg(IBaseDlg dlg)
    {
        dlgList.Add(dlg);
    }

    public void RemoveDlg(IBaseDlg dlg)
    {
        dlgList.Remove(dlg);
    }

}
