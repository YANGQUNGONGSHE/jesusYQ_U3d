using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportFiler : MonoBehaviour {

    public GameObject CellPrefab;
    public Transform PartObject;
    public List<string> DataSource;
    public Action<int, string> OnCellClick;
    public int IsSelected = -1;

    public void Refresh()
    {
        GetListItem();
    }

    private void GetListItem()
    {
        if (PartObject != null && PartObject.childCount > 0)
        {
            for (var i = 0; i < PartObject.childCount; i++)
            {
                Destroy(PartObject.transform.GetChild(i).gameObject);
            }
        }
        for (var i = 0; i < DataSource.Count; i++)
        {
            var go = Instantiate(CellPrefab);
            if (PartObject != null) go.transform.SetParent(PartObject, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            go.GetComponent<ReportCell>().InitUi(i, DataSource[i], (index, reportModel) =>
            {
                if (OnCellClick == null) return;
                OnCellClick(index, reportModel);
                IsSelected = index;
                Refresh();
            },null,i==IsSelected);
        }
    }
}
