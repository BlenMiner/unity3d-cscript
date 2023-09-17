using UnityEngine;

[System.Serializable]
public class CScriptField
{
    public const float BOTTOM_MARGIN = 20f;
    
    [SerializeField] CScript m_cscript;
    
    public CScriptField(CScript cscript)
    {
        UpdateScript(cscript);
    }
    
    public void UpdateScript(CScript cscript)
    {
        m_cscript = cscript;
    }
    
    public void OnGUI(Rect rect)
    {
        if (!m_cscript)
        {
            GUI.Label(rect, "Invalid CScript provided.");
            return;
        }

        var scriptRect = new Rect(rect.x, rect.y, rect.width, rect.height - BOTTOM_MARGIN);
        var source = GUI.TextArea(scriptRect, m_cscript.SourceCode);

        var bottomBarRect = new Rect(rect.x, rect.y + rect.height - BOTTOM_MARGIN, rect.width, BOTTOM_MARGIN);

        GUI.contentColor = new Color(1f, 0.5f, 0.5f);
        GUI.skin.box.richText = true;
        GUI.Box(bottomBarRect, m_cscript.Errors.Count > 0 ? m_cscript.Errors[0].ToString() : string.Empty);
        GUI.contentColor = Color.white;

        if (m_cscript.SourceCode != source && string.IsNullOrWhiteSpace(m_cscript.SavePath))
        {
            m_cscript.SourceCode = source;
            m_cscript.Save();
        }
        else m_cscript.SourceCode = source;
    }

    public void Save()
    {
        m_cscript.Save();
    }
}
