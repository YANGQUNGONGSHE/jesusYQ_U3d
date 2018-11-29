﻿using System.IO;
using LitJson;

namespace WongJJ.Game.Core.Config
{
    public class JsonHelper
    {
        /// <summary>
        /// 反序列化JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T FormatConfig<T>(string path) where T : class
        {
            string jsonData = File.ReadAllText(path);
            JsonReader jr = new JsonReader(jsonData);
            return JsonMapper.ToObject<T>(jr);
        }
    }
}
