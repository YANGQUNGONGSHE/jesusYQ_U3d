
using UnityEngine;

namespace ImageAndVideoPicker
{

	public class AndroidPicker
	{
		#if UNITY_ANDROID
		static AndroidJavaClass _plugin;

		static AndroidPicker()
		{
			_plugin = new AndroidJavaClass("com.astricstore.imageandvideopicker.AndroidPicker");
		}

		public static void BrowseImage()
		{
			_plugin.CallStatic("BrowseForImage",false,1,1);
			
		}

		public static void BrowseImage(bool cropping, int aspectX = 1, int aspectY = 1)
		{
			_plugin.CallStatic("BrowseForImage",cropping,aspectX,aspectY);

		}

		public static void BrowseVideo()
		{
			_plugin.CallStatic("BrowseForVideo");

		}

		public static void BrowseContact()
		{
			_plugin.CallStatic("BrowseForContact");
			
		}
#endif
	}
}


