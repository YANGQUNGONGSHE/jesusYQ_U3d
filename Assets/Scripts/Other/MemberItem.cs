using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberItem : MonoBehaviour 
{
	public CircleRawImage HeadIconImage;
	public Text DisplayName;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	private void Awake()
	{
		transform.GetComponent<Button>().onClick.AddListener(()=>{
			
		});
	}

	public void SetUi(Texture2D headIcon, string displayname)
	{
		HeadIconImage.texture = headIcon;
		DisplayName.text = displayname;
	}

	// IEnumerator LoadImage(string path)
	// {
	// 	WWW www = new WWW(path);
	// 	yield return www;
	// 	if (string.IsNullOrEmpty(www.error))
	// 	{
	// 		HeadIconImage.texture = www.texture;
	// 	}
	// 	else
	// 	{
	// 		Log.I("出错:"+ www.error);
	// 	}
	// }
}
