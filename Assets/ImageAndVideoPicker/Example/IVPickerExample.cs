using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ImageAndVideoPicker;

public class IVPickerExample : MonoBehaviour {

	string log = "";
	Texture2D texture;

	void OnEnable()
	{
		PickerEventListener.onImageSelect += OnImageSelect;
		PickerEventListener.onImageLoad += OnImageLoad;
		PickerEventListener.onVideoSelect += OnVideoSelect;
		PickerEventListener.onError += OnError;
		PickerEventListener.onCancel += OnCancel;
	}
	
	void OnDisable()
	{
		PickerEventListener.onImageSelect -= OnImageSelect;
		PickerEventListener.onImageLoad -= OnImageLoad;
		PickerEventListener.onVideoSelect -= OnVideoSelect;
		PickerEventListener.onError -= OnError;
		PickerEventListener.onCancel -= OnCancel;
	}
	
	void OnImageSelect(string imgPath, ImageAndVideoPicker.ImageOrientation imgOrientation)
	{
		Debug.Log ("Image Location : "+imgPath);
		log += "\nImage Path : " + imgPath;
		log += "\nImage Orientation : " + imgOrientation;
	}

	void OnImageLoad(string imgPath, Texture2D tex, ImageAndVideoPicker.ImageOrientation imgOrientation)
	{
		Debug.Log ("Image Location : "+imgPath);
		texture = tex;
	
	}

	void OnVideoSelect(string vidPath)
	{
		Debug.Log ("Video Location : "+vidPath);
		log += "\nVideo Path : " + vidPath;
		Handheld.PlayFullScreenMovie ("file://" + vidPath, Color.blue, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFill);
	}
	
	void OnError(string errorMsg)
	{
		Debug.Log ("Error : "+errorMsg);
		log += "\nError :" +errorMsg;
	}

	void OnCancel()
	{
		Debug.Log ("Cancel by user");
		log += "\nCancel by user";
	}

	void OnGUI()
	{
		GUILayout.Label (log);

		if(GUI.Button(new Rect(10,10,120,35),"Browse Image"))
		 {
			#if UNITY_ANDROID
			AndroidPicker.BrowseImage(false);
			#elif UNITY_IPHONE
			IOSPicker.BrowseImage(false); // true for pick and crop
			#endif
		}

		if(GUI.Button(new Rect(140,10,150,35),"Browse & Crop Image"))
		{
			#if UNITY_ANDROID
			AndroidPicker.BrowseImage(true);
			#elif UNITY_IPHONE
			IOSPicker.BrowseImage(true); // true for pick and crop
			#endif
		}

		if(GUI.Button(new Rect(300,10,120,35),"Browse Video"))
		{
			#if UNITY_ANDROID
			AndroidPicker.BrowseVideo();
			#elif UNITY_IPHONE
			IOSPicker.BrowseVideo();
			#endif
		}

		if (texture != null){
			
			GUI.DrawTexture(new Rect(20,50,Screen.width - 40,Screen.height - 60), texture, ScaleMode.ScaleToFit, true);
		}
	}

}
