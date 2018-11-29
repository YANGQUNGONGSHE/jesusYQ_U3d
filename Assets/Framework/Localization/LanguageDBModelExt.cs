using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LanguageDBModel
{
    public Language CurrentLanguage
    {
        get;
        set;
    }

    public string GetText(string module, string key)
    {
        if(m_List != null && m_List.Count > 0)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                if(m_List[i].Module.Equals(module, System.StringComparison.CurrentCultureIgnoreCase) &&
                    m_List[i].Key.Equals(key, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    switch(CurrentLanguage)
                    {
                        case Language.ZH:
                            return m_List[i].ZH;

                        case Language.EN:
                            return m_List[i].EN;
                                     
                    }
                }
            }
        }
        return null;
    }

    public List<string> GetModules()
    {
        if (m_List != null && m_List.Count > 0)
        {
            List<string> ret = new List<string>();
            for (int i = 0; i < m_List.Count; i++)
            {
                if (ret.Contains(m_List[i].Module))
                    continue;
                ret.Add(m_List[i].Module);
            }
            return ret;
        }
        return null;
    }

    public List<string> GetKeysByModule(string module)
	{
		if (m_List != null && m_List.Count > 0)
		{
			List<string> ret = new List<string>();
			for (int i = 0; i < m_List.Count; i++)
			{
                if (!m_List[i].Module.Equals(module, System.StringComparison.CurrentCultureIgnoreCase))
                    continue;
                ret.Add(m_List[i].Key);
			}
			return ret;
		}
		return null;
	}
}

public enum Language
{
    ZH,
    EN,
}
