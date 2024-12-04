using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartMenuDlg : BaseDlg<StartMenuDlg>
{
    // Start is called before the first frame update
    Button btnStart;
    protected override string UIName
    {
        get
        {
            return "StartMenu";
        }
    }

    protected override void Init()
    {
        btnStart = gameObject.transform.Find("bg/Btn_Newgame").GetComponent<Button>();
        btnStart.onClick.AddListener(onClick);//btnStart.onClick
    }
    void onClick()
    {
        Debug.Log("click");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
