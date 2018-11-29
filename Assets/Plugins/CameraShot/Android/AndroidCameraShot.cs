
using UnityEngine;

namespace CameraShot
{

	public class AndroidCameraShot
	{
		#if UNITY_ANDROID
		static AndroidJavaClass _plugin;

		static AndroidCameraShot()
		{
			_plugin = new AndroidJavaClass("com.astricstore.camerashots.CameraShots");
		}

		public static void LaunchCameraForImageCapture()
		{
			CameraShot.mode = 0;
			LaunchCameraForImage (false);
		}

		public static void LaunchCameraForImageCapture(bool cropping, int aspectX=1, int aspectY=1)
		{
			CameraShot.mode = 0;
			LaunchCameraForImage (cropping,aspectX,aspectY);
		}
		
		public static void GetTexture2DFromCamera()
		{
			CameraShot.mode = 1;
			LaunchCameraForImage (false);
		}

		public static void GetTexture2DFromCamera(bool cropping, int aspectX=1, int aspectY=1)
		{
			CameraShot.mode = 1;
			LaunchCameraForImage (cropping,aspectX,aspectY);
		}
		

		// for video
		public static void LaunchCameraForVideoCapture(int maxDuration = 0)
		{
			LaunchCameraForVideo(maxDuration);
		}


		private static void LaunchCameraForImage(bool cropping, int aspectX=1, int aspectY=1)
		{
			_plugin.CallStatic("launchCameraForImageCapture",cropping,aspectX,aspectY);

		}

		private static void LaunchCameraForVideo(int maxDuration)
		{
			_plugin.CallStatic("launchCameraForVideoCapture",maxDuration);

		}
#endif
	}
}


