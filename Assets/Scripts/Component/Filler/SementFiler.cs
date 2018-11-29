using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class SementFiler : MonoBehaviour
{
    public GameObject CellPrefab;
    public Transform PartObject;
    public Action<int, Segment> OnCellClick;
    public List<Segment> DataSource;
    private ScrollRect _mScrollRect;

    private void Awake()
    {
        _mScrollRect = transform.GetComponent<ScrollRect>();
    }




    public void InitDataSourceWith(int chapterId)
    {

        if (PartObject != null && PartObject.childCount > 0)
        {
            
            for (var i = 0; i < PartObject.childCount; i++)
            {
                Destroy(PartObject.transform.GetChild(i).gameObject);
            }
        }

        DataSource = LocalDataManager.LoadSQLTable<Segment>(SQL.QUERY_SEGMENT_BYCHAPTERID, chapterId);

        for (var i = 0; i < DataSource.Count; i++)
        {
            var go = Instantiate(CellPrefab);
            if (PartObject != null) go.transform.SetParent(PartObject, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            go.GetComponent<SementCell>().InitUi(i, DataSource[i], (index, segment) =>
            {
                OnCellClick(index, segment);

            });
        }
    }
}
