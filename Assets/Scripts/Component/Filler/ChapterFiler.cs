using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class ChapterFiler : MonoBehaviour
{

    public GameObject CellPrefab;
    public Transform PartObject;
    public Action<int, Chapter> OnCellClick;
    public List<Chapter> DataSource;
    private ScrollRect _mScrollRect;
    [HideInInspector]public int IsSelected = -1;

    private void Awake()
    {
        _mScrollRect = transform.GetComponent<ScrollRect>();
        
    }

     public void Refresh(int biographyId)
    {
        InitDataSourceWith(biographyId);
        IsSelected = -1;
    }

    public  void InitDataSourceWith(int biographyId)
    {
        var brId = biographyId;
        if (PartObject != null && PartObject.childCount > 0)
        {
            for (var i = 0; i < PartObject.childCount; i++)
            {
                Destroy(PartObject.transform.GetChild(i).gameObject);
            }
        }

        DataSource = LocalDataManager.LoadSQLTable<Chapter>(SQL.QUERY_CHAPTER_BYBIOGRAPHYID, biographyId);
        for (var i = 0; i < DataSource.Count; i++)
        {
            var go = Instantiate(CellPrefab);
            if (PartObject != null) go.transform.SetParent(PartObject, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            go.GetComponent<ChapterCell>().InitUi(i, DataSource[i], (index, chapter) =>
            {
                
                if(OnCellClick==null)return;
                OnCellClick(index, chapter);
                IsSelected = index;
                Refresh(brId);

            },null,i== IsSelected);
        }

    }
}
