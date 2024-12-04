using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public interface IBaseDlg
{
    void SetVisible(bool visible);
}
public class BaseDlg<T> : IBaseDlg where T : BaseDlg<T>, new()
{
    private static BaseDlg<T> _instance;
    public static BaseDlg<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = LoadUI();
            }
            return _instance;
        }
    }

    public bool IsVisible { get { return gameObject.activeSelf; } }

    public static BaseDlg<T> LoadUI()
    {
        BaseDlg<T> dlg = Activator.CreateInstance<T>();
        GameObject gameObject = GameObject.Find($"UIRoot/{dlg.UIName}");
        var parent = GameObject.Find("UIRoot").transform;
        if (gameObject == null)
        {
            gameObject = Resources.Load<GameObject>($"Prefabs/GUI/{dlg.UIName}");
        }
        gameObject = GameObject.Instantiate(gameObject, parent);
        if (gameObject != null)
        {
            dlg.gameObject = gameObject;
            UIManager.Instance.AddDlg(dlg);
            dlg.Init();
        }
        return dlg;
    }

    public GameObject gameObject;

    protected virtual string UIName { get { return string.Empty; } }
    protected virtual void Init()
    {

    }

    protected virtual void OnLoad()
    {

    }

    protected virtual void OnShow()
    {

    }

    protected virtual void OnHide()
    {

    }

    public void SetVisible(bool visible)
    {
        if (visible)
        {
            gameObject.SetActive(true);
            OnShow();
        }
        else
        {
            gameObject.SetActive(false);
            OnHide();
        }

    }


    // Update is called once per frame

}
