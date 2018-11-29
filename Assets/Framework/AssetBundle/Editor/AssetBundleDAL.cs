using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System;

namespace WongJJ.Game.Core
{
    //===================================================
    //作    者：王家俊  http://www.unity3d.com  QQ：394916173
    //创建时间：2016-08-30 18:37:47
    //备    注：
    //===================================================
    public class AssetBundleDAL
    {
        /// <summary>
        /// xml配置文件路径
        /// </summary>
        private readonly string _mXmlPath;

        /// <summary>
        /// 需要打包的资源集合
        /// </summary>
        private readonly List<AssetBundleEntity> _mList = null;


        public AssetBundleDAL(string xmlPath)
        {
            _mXmlPath = xmlPath;
            _mList = new List<AssetBundleEntity>();
        }

        /// <summary>
        /// 读取XML配置，获取需要打包的资源集合
        /// </summary>
        /// <returns></returns>
        public List<AssetBundleEntity> GetBundleList()
        {
            _mList.Clear();

            XDocument doc = XDocument.Load(_mXmlPath);
            XElement root = doc.Root;
            XElement assetBundleNode = root.Element("AssetBundle");

            IEnumerable<XElement> list = assetBundleNode.Elements("Item");
            int index = 0;
            foreach (XElement item in list)
            {
                AssetBundleEntity entity = new AssetBundleEntity();
                entity.Key = "key" + ++index;
                entity.Name = item.Attribute("Name").Value;
                entity.Tag = item.Attribute("Tag").Value;
                entity.IsFolder = item.Attribute("IsFolder").Value.Equals("True", StringComparison.CurrentCultureIgnoreCase);
                entity.IsFirstData = item.Attribute("IsFirstData").Value.Equals("True", StringComparison.CurrentCultureIgnoreCase);

                IEnumerable<XElement> pathList = item.Elements("Path");
                foreach (XElement path in pathList)
                {
                    //entity.PathList.Add (string.Format ("Assets/{0}", path.Attribute ("Value").Value));
                    entity.PathList.Add(path.Attribute("Value").Value);
                }
                _mList.Add(entity);
            }
            return _mList;
        }
    }
}