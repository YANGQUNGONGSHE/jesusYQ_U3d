using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// 本地文件管理
/// </summary>
namespace WongJJ.Game.Core
{
    public class LocalByteFileManager : Singletion<LocalByteFileManager>
    {
        public readonly string LocalFilePath = Application.persistentDataPath + "/";

//#if UNITY_EDITOR
//
//    #if UNITY_STANDALONE_WIN
//        public readonly string LocalFilePath = Application.dataPath + "/../AssetBundles/Win/";
//    #elif UNITY_STANDALONE_OSX
//        public readonly string LocalFilePath = Application.dataPath + "/../AssetBundles/Mac/";
//    #elif UNITY_ANDROID
//        public readonly string LocalFilePath = Application.dataPath + "/../AssetBundles/Android/";
//    #elif UNITY_IPHONE
//        public readonly string LocalFilePath = Application.dataPath + "/../AssetBundles/iOS/";
//    #endif
// 
//#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
//        public readonly string LocalFilePath = "file:///" + Application.persistentDataPath + "/";
//#endif

        /// <summary>
        /// 读取本地文件到byte数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte[] GetBuffer(string path)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
    }
}