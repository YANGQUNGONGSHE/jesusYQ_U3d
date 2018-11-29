using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;

public class AddGroupMember : MonoBehaviour {

	// Use this for initialization
	void Start () {

		transform.GetComponent<Button>().onClick.AddListener(() =>
		{
		    NotificationCenter.DefaultCenter().PostNotification(NotifiyName.AddMembers, this);
		});
	}
}
