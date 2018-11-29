using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WongJJ.Game.Core
{
	//===================================================
	//作    者：王家俊  http://www.unity3d.com  QQ：394916173
	//创建时间：2016-08-30 18:37:26
	//备    注：AssetBundle实体 -> 对应AssetBundleConfig.xml
	//===================================================
	public class AssetBundleEntity
	{
		/// <summary>
		/// 资源名称
		/// </summary>
		public string Name;

		/// <summary>
		/// 资源标记
		/// </summary>
		public string Tag;

		/// <summary>
		/// 是否整目录打包
		/// </summary>
		public bool IsFolder;

		/// <summary>
		/// 是否初始化下载加载
		/// </summary>
		public bool IsFirstData;

		public bool IsChecked;

		/// <summary>
		/// 路径集合
		/// </summary>
		private List<string> m_PathList = new List<string> ();

		public List<string> PathList {
			get {
				return m_PathList;
			}
		}

		/// <summary>
		/// 用户打包时候的选的（编辑器）
		/// </summary>
		public string Key;
	}
}