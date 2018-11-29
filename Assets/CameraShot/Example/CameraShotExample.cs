using UnityEngine;
using System.Collections;

using CameraShot;

public class CameraShotExample : MonoBehaviour {

	string log = "";
	void OnEnable()
	{
		CameraShotEventListener.onImageSaved += OnImageSaved;
		CameraShotEventListener.onImageLoad += OnImageLoad;
		CameraShotEventListener.onVideoSaved += OnVideoSaved;
		CameraShotEventListener.onError += OnError;
		CameraShotEventListener.onCancel += OnCancel;
	}

	void OnDisable()
	{
		CameraShotEventListener.onImageSaved -= OnImageSaved;
		CameraShotEventListener.onImageLoad -= OnImageLoad;
		CameraShotEventListener.onVideoSaved -= OnVideoSaved;
		CameraShotEventListener.onError -= OnError;
		CameraShotEventListener.onCancel -= OnCancel;
	}

	void OnImageSaved(string path, ImageOrientation orientation)
	{
		log += "\nImage Saved to gallery, path : " + path + ", orientation : " + orientation;
	}

	void OnImageLoad(string path,Texture2D tex, ImageOrientation orientation)
	{
		GameObject.Find("Cube").GetComponent<Renderer>().material.mainTexture = tex;
		log += "\nImage Saved to gallery, loaded :" + path + ", orientation : " + orientation;
	}

	void OnVideoSaved(string path)
	{
		Debug.Log ("Video Saved at path : "+path);
		log += "\nVideo Saved at path :" + path;
	}

	void OnError(string errorMsg)
	{
		Debug.Log ("Error : "+errorMsg);
		log += "\nError : "+errorMsg;
	}

	void OnCancel()
	{
		Debug.Log ("OnCancel");
		log += "\nOnCancel";
	}

	void OnGUI()
	{
		GUILayout.Label (log);
		float btnWidth = 150;
		float btnHeight = 50;
		float y = Screen.height/2-btnHeight/2 - 50;
		if(GUI.Button(new Rect(Screen.width/2-btnWidth/2,y,btnWidth,btnHeight),"Capture Image"))
		{
			#if UNITY_ANDROID
			AndroidCameraShot.LaunchCameraForImageCapture(false); 
			#elif UNITY_IPHONE
			//IOSCameraShot.LaunchCameraForImageCapture(false);// true - capture and crop
			#endif
		}

		y += 100;
		if(GUI.Button(new Rect(Screen.width/2-btnWidth/2,y,btnWidth,btnHeight),"Get Texture"))
		{
			#if UNITY_ANDROID
			AndroidCameraShot.GetTexture2DFromCamera(true);
			#elif UNITY_IPHONE
			//IOSCameraShot.GetTexture2DFromCamera(true);// capture and crop
			#endif
		}

		y += 100;
		if(GUI.Button(new Rect(Screen.width/2-btnWidth/2,y,btnWidth,btnHeight),"Record Video"))
		{
			#if UNITY_ANDROID
			AndroidCameraShot.LaunchCameraForVideoCapture(0);
			//AndroidCameraShot.LaunchCameraForVideoCapture(10);// record for 10 seconds
			#elif UNITY_IPHONE
			//IOSCameraShot.LaunchCameraForVideoCapture(0); // record for unlimited time
			//IOSCameraShot.LaunchCameraForVideoCapture(10); // record for 10 sec
			#endif
		}
	}
}
