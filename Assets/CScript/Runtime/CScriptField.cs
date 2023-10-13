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

    public bool OnGUI(Rect rect)
    {
        if (!m_cscript)
        {
            GUI.Label(rect, "Invalid CScript provided.");
            return false;
        }
        
        bool showBottomBar = m_cscript.Errors.Count > 0;
        float bottomMargin = showBottomBar ? BOTTOM_MARGIN : 0f;

        var scriptRect = new Rect(rect.x, rect.y, rect.width, rect.height - bottomMargin);
        var source = GUI.TextArea(scriptRect, m_cscript.SourceCode);

        if (showBottomBar)
        {
            var bottomBarRect = new Rect(rect.x, rect.y + rect.height - bottomMargin, rect.width, bottomMargin);

            GUI.contentColor = new Color(1f, 0.5f, 0.5f);
            GUI.skin.box.richText = true;
            GUI.Box(bottomBarRect, m_cscript.Errors.Count > 0 ? m_cscript.Errors[0].ToString() : string.Empty);
            GUI.contentColor = Color.white;
        }

        if (m_cscript.SourceCode != source)
        {
            if (string.IsNullOrWhiteSpace(m_cscript.SavePath))
            {
                m_cscript.SourceCode = source;
                m_cscript.Save();
                return true;
            }

            m_cscript.SourceCode = source;
            return true;
        }

        return false;
    }

    public void Save()
    {
        m_cscript.Save();
    }
}
