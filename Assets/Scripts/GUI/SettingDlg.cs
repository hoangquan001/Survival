using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingDlg : BaseDlg<SettingDlg>
{
    // Start is called before the first frame update
    Button btnStart;
    protected override string UIName
    {
        get
        {
            return "SettingDlg";
        }
    }

    protected override void Init()
    {
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
