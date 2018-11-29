using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace WongJJ.Game.Core
{
    static public class Util
    {
        #region 常量
        public static Vector3 AxisX = new Vector3(1, 0, 0);
        public static Vector3 AxisY = new Vector3(0, 1, 0);
        public static Vector3 AxisZ = new Vector3(0, 0, 1);
        public static Vector2 AxisX2D = new Vector2(1, 0);
        public static Vector2 AxisY2D = new Vector2(0, 1);
        public static float COS_15 = Mathf.Cos(Mathf.Deg2Rad * 15.0f);
        public static float COS_35 = Mathf.Cos(Mathf.Deg2Rad * 35.0f);
        public static float COS_45 = Mathf.Cos(Mathf.Deg2Rad * 45.0f);
        public static float COS_75 = Mathf.Cos(Mathf.Deg2Rad * 75.0f);
        public static float COS_60 = Mathf.Cos(Mathf.Deg2Rad * 60.0f);
        public static float COS_30 = Mathf.Cos(Mathf.Deg2Rad * 30.0f);
        public static float COS_20 = Mathf.Cos(Mathf.Deg2Rad * 20.0f);
        public static float ONE_DIV_PI = 1.0f / Mathf.PI;
        public static float EPSILON = 0.001f;
        #endregion

        #region 寻找添加游戏物体等相关
        /// <summary>
        /// if T is Exitis Return, if not, then add T and Return
        /// </summary>
        /// <returns>The or get component.</returns>
        /// <param name="_go">Go.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        static public T CreateOrGetComponent<T>(GameObject _go) where T : Component
        {
            if (null == _go)
            {
                return null;
            }
            T comp = _go.GetComponent<T>();
            if (null == comp)
            {
                comp = _go.AddComponent<T>();
            }
            return comp;
        }

        /// <summary>
        /// Adds the game object to parent.
        /// </summary>
        /// <param name="_child">Child.</param>
        /// <param name="_parent">Parent.</param>
        static public void AddGameObjectToParent(Transform _child, Transform _parent)
        {
            if (null == _child || null == _parent || _child.parent == _parent)
            {
                return;
            }
            _child.parent = _parent;
            Transform trans = _parent;
            Vector3 scaleParam = new Vector3();
            while (null != trans)
            {
                scaleParam = trans.localScale;
                scaleParam.Scale(_child.localPosition);
                _child.localPosition = trans.localPosition + scaleParam;
                _child.localRotation = trans.localRotation * _child.localRotation;
                scaleParam = _child.localScale;
                scaleParam.Scale(trans.localScale);
                _child.localScale = scaleParam;
                trans = trans.parent;
            }
        }

        /// <summary>
        /// Adds the game object to parent with zero position.
        /// </summary>
        /// <param name="_child">Child.</param>
        /// <param name="_parent">Parent.</param>
        static public void AddGameObjectToParentWithZeroPos(Transform _child, Transform _parent)
        {
            if (null == _child || null == _parent)
            {
                return;
            }

            _child.parent = _parent;
            _child.localPosition = Vector3.zero;
            _child.localRotation = Quaternion.identity;
            _child.localScale = Vector3.one;
        }

        /// <summary>
        /// Determines if is sub class of the specified _baseT t.
        /// </summary>
        /// <returns><c>true</c> if is sub class of the specified _baseT t; otherwise, <c>false</c>.</returns>
        /// <param name="_baseT">Base t.</param>
        /// <param name="t">T.</param>
        static public bool IsSubClassOf(Type _baseT, Type t)
        {
            return _baseT.IsAssignableFrom(t);
        }

        /// <summary>
        /// Find the game object in child.
        /// </summary>
        /// <returns>The game object in child.</returns>
        /// <param name="_transform">Transform.</param>
        /// <param name="_name">Name.</param>
        /// <param name="justFirstLevel">If set to <c>true</c> just first level.</param>
        static public Transform FindGameObjectInChild(Transform _transform, string _name, bool justFirstLevel = false)
        {
            List<Transform> ret = FindGameObjectInChild(_transform, new List<string>() { _name });
            if (ret.Count > 0)
            {
                return ret[0];
            }
            return null;
        }

        /// <summary>
        /// Find the game object in child.
        /// </summary>
        /// <returns>The game object in child.</returns>
        /// <param name="_transform">Transform.</param>
        /// <param name="_names">Names.</param>
        /// <param name="justFirstLevel">If set to <c>true</c> just first level.</param>
        static public List<Transform> FindGameObjectInChild(Transform _transform, List<string> _names, bool justFirstLevel = false)
        {
            List<Transform> ret = new List<Transform>();

            if (_transform == null)
            {
                return ret;
            }

            if (_transform.childCount != 0)
            {
                for (int i = 0; i < _transform.childCount; i++)
                {
                    if (justFirstLevel)
                    {
                        if (_names.Contains(_transform.GetChild(i).name))
                        {
                            ret.Add(_transform);
                        }
                    }
                    else
                    {
                        ret.AddRange(FindGameObjectInChild(_transform.GetChild(i), _names));
                    }
                }
            }

            if (_names.Contains(_transform.name))
            {
                ret.Add(_transform);
            }

            return ret;
        }

        /// <summary>
        /// Find the component in child.
        /// </summary>
        /// <returns>The component in child.</returns>
        /// <param name="tan">Tan.</param>
        /// <param name="name">Name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        static public T FindComponentInChild<T>(Transform _transform, string _name) where T : Component
        {
            List<Transform> ret = FindGameObjectInChild(_transform, new List<string>() { _name });
            if (ret.Count > 0)
            {
                return ret[0].GetComponent<T>();
            }
            return null;
        }
        #endregion

        #region 时间相关
        /// <summary>
        /// Timestamps convert date time.
        /// </summary>
        /// <returns>The convert date time.</returns>
        /// <param name="t">T.</param>
        static public System.DateTime TimestampConvertDateTime(uint t)
        {
            System.DateTime dt = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long lTime = long.Parse(t.ToString() + "0000000");
            System.TimeSpan toNow = new System.TimeSpan(lTime);
            return dt.Add(toNow);
        }

        /// <summary>
        /// 根据秒数获取时间字符串;
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string GetTimeStr(int second)
        {
            int minute = second / 60;
            int sec = second % 60;
            string timeStr = "";
            if (minute <= 9)
            {
                timeStr = "0" + minute + ":";
            }
            else
            {
                timeStr = minute + ":";
            }

            if (sec <= 9)
            {
                timeStr += "0" + sec;
            }
            else
            {
                timeStr += sec + "";
            }

            return timeStr;
        }
        #endregion

        #region 向量相关
        /// <summary>
        /// 根据指定最大距离和最大高度获取施加力;
        /// </summary>
        /// <param name="x">距离</param>
        /// <param name="y">高度</param>
        /// <returns></returns>
        public static Vector3 GetForce(float x, float y)
        {
            float angle = Mathf.Atan(4 * y / x);
            float force = Mathf.Sqrt(2 * -Physics.gravity.y * y) / Mathf.Sin(angle);
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;

            return force * dir;
        }

        /// <summary>
        /// 根据指定高度获取到达高度所需要的时间（斜抛运动到达最高点所用的时间）
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float GetTimeInTop(float y)
        {
            float flyTime = Mathf.Sqrt(2 * -Physics.gravity.y * y) / -Physics.gravity.y;
            return flyTime;
        }

        /// <summary>
        /// 根据横向距离获取最大发射力度;
        /// </summary>
        /// <param name="x">距离</param>
        /// <returns></returns>
        public static float GetMaxPower(float x)
        {
            float force = Mathf.Sqrt(-Physics.gravity.y * x / 2.0f) / Mathf.Sin(Mathf.PI / 4);
            return force;
        }

        /// <summary>
        /// 对Vector3整体保留几位小数;
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static Vector3 Vector3ToRound(Vector3 vec, int decimals)
        {
            float x = (float)Math.Round(vec.x, decimals);
            float y = (float)Math.Round(vec.y, decimals);
            float z = (float)Math.Round(vec.z, decimals);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// 对Vector2整体保留几位小数
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static Vector2 Vector2ToRound(Vector2 vec, int decimals)
        {
            float x = (float)Math.Round(vec.x, decimals);
            float y = (float)Math.Round(vec.y, decimals);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Distance the specified a and b.
        /// </summary>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        static public float Distance(Vector3 _a, Vector3 _b)
        {
            return (_a.x - _b.x) * (_a.x - _b.x) + (_a.y - _b.y) * (_a.y - _b.y) + (_a.z - _b.z) * (_a.z - _b.z);
        }

        /// <summary>
        /// Distance the specified a and b.
        /// </summary>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        static public float Distance(Vector2 _a, Vector2 _b)
        {
            return (_a.x - _b.x) * (_a.x - _b.x) + (_a.y - _b.y) * (_a.y - _b.y);
        }

        /// <summary>
        /// Gets the angle.
        /// </summary>
        /// <returns>The angle.</returns>
        /// <param name="form">Form.</param>
        /// <param name="to">To.</param>
        static public float GetAngle(Vector3 _form, Vector3 _to)
        {
            Vector3 nVector = Vector3.zero;
            nVector.x = _to.x;
            nVector.y = _form.y;
            float a = _to.y - nVector.y;
            float b = nVector.x - _form.x;
            float tan = a / b;
            return Mathf.Atan(tan) * 180.0f * ONE_DIV_PI;
        }

        /// <summary>
        /// Normalize the specified vec.
        /// </summary>
        /// <param name="vec">Vec.</param>
        static public float Normalize(ref Vector3 vec)
        {
            float length = Mathf.Sqrt((vec.x * vec.x) + (vec.y * vec.y) + (vec.z * vec.z));
            if (length > 0)
            {
                float oneDivLength = 1.0f / length;
                vec.x = vec.x * oneDivLength;
                vec.y = vec.y * oneDivLength;
                vec.z = vec.z * oneDivLength;
            }
            return length;
        }

        /// <summary>
        /// Tries to move to position with speed.
        /// </summary>
        /// <returns>The to move to position with speed.</returns>
        /// <param name="dest">Destination.</param>
        /// <param name="cur">Current.</param>
        /// <param name="speed">Speed.</param>
        /// <param name="time">Time.</param>
        static public Vector3 TryMoveToPosWithSpeed(Vector3 _to, Vector3 _from, float _speed, float _time)
        {
            Vector3 dir = _to - _from;
            float dis = Normalize(ref dir);
            if (_speed * _time < dis)
            {
                return _from + dir * _speed * _time;
            }
            else
            {
                return _to;
            }
        }

        /// <summary>
        /// Offsets the move to position with speed.
        /// </summary>
        /// <returns>The move to position with speed.</returns>
        /// <param name="_to">To.</param>
        /// <param name="_from">From.</param>
        /// <param name="_speed">Speed.</param>
        /// <param name="_time">Time.</param>
        static public Vector3 OffsetMoveToPosWithSpeed(Vector3 _to, Vector3 _from, float _speed, float _time)
        {
            Vector3 dir = _to - _from;
            Vector3 maxOffset = dir;
            float dis = Normalize(ref dir);
            if (_speed * _time < dis)
            {
                return dir * _speed * _time;
            }
            else
            {
                return maxOffset;
            }
        }

        /// <summary>
        /// Determines if is equal float the specified a b.
        /// </summary>
        /// <returns><c>true</c> if is equal float the specified a b; otherwise, <c>false</c>.</returns>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        static public bool IsEqualFloat(float _a, float _b)
        {
            return (Math.Abs(_a - _b) < 0.001f);
        }

        /// <summary>
        /// Determines if is equal float raw the specified a b.
        /// </summary>
        /// <returns><c>true</c> if is equal float raw the specified a b; otherwise, <c>false</c>.</returns>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        static public bool IsEqualFloatRaw(float _a, float _b)
        {
            return (Math.Abs(_a - _b) < 0.05f);
        }

        /// <summary>
        /// 获取目标点周围圆形的随机点
        /// </summary>
        /// <param name="targerPos"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        static public Vector3 CircleAroundTargetPoint(Vector3 targerPos, float distance)
        {
            //1.定义一个向量
            Vector3 v = new Vector3(0, 0, 1); //z轴超前的

            //2.让向量旋转
            v = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0) * v;

            //3.向量 * 距离(半径) = 坐标点
            Vector3 pos = v * distance * UnityEngine.Random.Range(0.8f, 1f);

            //4.计算出来的 围绕目标点的 随机坐标点
            return targerPos + pos;
        }

        /// <summary>
        /// 获取目标点周围半圆的随机点
        /// </summary>
        /// <returns>The circle around target point.</returns>
        /// <param name="currPos">Curr position.</param>
        /// <param name="targerPos">Targer position.</param>
        /// <param name="distance">Distance.</param>
        static public Vector3 SemiCircleAroundTargetPoint(Vector3 currPos, Vector3 targerPos, float distance)
        {
            //1.定义一个向量
            Vector3 v = (currPos - targerPos).normalized;

            //2.让向量旋转
            v = Quaternion.Euler(0, UnityEngine.Random.Range(-90f, 90f), 0) * v;

            //3.向量 * 距离(半径) = 坐标点
            Vector3 pos = v * distance * UnityEngine.Random.Range(0.8f, 1f);

            //4.计算出来的 围绕主角的 随机坐标点
            return targerPos + pos;
        }
        #endregion

        #region 文件读写相关
        /// <summary>
        /// 创建文本文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        static public void CreateTextFile(string filePath, string content)
        {
            DeleteFile(filePath);

            using (FileStream fs = File.Create(filePath))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(content.ToString());
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        static public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        static public void CopyDirectory(string sourceDirName, string destDirName)
        {
            try
            {
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                    File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));

                }

                if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                    destDirName = destDirName + Path.DirectorySeparatorChar;

                string[] files = Directory.GetFiles(sourceDirName);
                foreach (string file in files)
                {
                    if (File.Exists(destDirName + Path.GetFileName(file)))
                        continue;
                    FileInfo fileInfo = new FileInfo(file);
                    if (fileInfo.Extension.Equals(".meta", StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    File.Copy(file, destDirName + Path.GetFileName(file), true);
                    File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
                }

                string[] dirs = Directory.GetDirectories(sourceDirName);
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, destDirName + Path.GetFileName(dir));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 设备相关
        /// <summary>
        /// 获取设备标识符
        /// </summary>
        static public string DeviceIdentifier
        {
            get
            {
                return SystemInfo.deviceUniqueIdentifier;
            }
        }
        #endregion

        #region 加密解密相关
        /// <summary>
        /// 获取物体唯一标识ID;
        /// </summary>
        /// <returns></returns>
        public static int GetUniqueID(GameObject unit)
        {
            if (unit == null)
            {
                Debug.Log("unit is invalid");
                return 0;
            }

            return unit.GetInstanceID();
        }

        /// <summary>
        /// Md5 the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        static public string Md5(string value, bool lower = true)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(value));
            string strResult = BitConverter.ToString(bytResult);
            strResult = strResult.Replace("-", "");
            return lower ? strResult.ToLower() : strResult;
        }

        /// <summary>
        /// 获取文件的MD5
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static public string GetFileMD5(string filePath, bool lower = true)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytResult = md5.ComputeHash(file);
                string strResult = BitConverter.ToString(bytResult);
                strResult = strResult.Replace("-", "");
                return lower ? strResult.ToLower() : strResult;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region String处理相关
        static public int ToInt(this string str)
        {
            int temp = 0;
            int.TryParse(str, out temp);
            return temp;
        }

        static public float ToFloat(this string str)
        {
            float temp = 0;
            float.TryParse(str, out temp);
            return temp;
        }

        static public long ToLong(this string str)
        {
            long temp = 0;
            long.TryParse(str, out temp);
            return temp;
        }

        static public bool ToBool(this string str)
        {
            bool temp;
            if (str.Equals("1"))
            {
                temp = true;
            }
            else
            {
                temp = false;
            }
            return temp;
        }
        #endregion

        #region 组件数组赋空快捷
        static public void SetNull(this MonoBehaviour[] arr)
        {
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = null;
                }
                arr = null;
            }
        }

        static public void SetNull(this GameObject[] arr)
        {
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = null;
                }
                arr = null;
            }
        }

        static public void SetNull(this Transform[] arr)
        {
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = null;
                }
                arr = null;
            }
        }

        static public void SetNull(this Sprite[] arr)
        {
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = null;
                }
                arr = null;
            }
        }
        #endregion

        #region 字符串转换相关
        /// <summary>
        /// 以逗号分隔的规范字符串转换Vector2;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Vector2 StrToVector2(string str)
        {
            Vector2 value = Vector2.zero;
            string[] strArray = str.Split(',');
            if (strArray.Length < 2)
            {
                Debug.LogError(str + " string to Vector2 failed with , spilt");
                return value;
            }
            float x = Convert.ToSingle(strArray[0]);
            float y = Convert.ToSingle(strArray[1]);
            value = new Vector2(x, y);

            return value;
        }

        /// <summary>
        /// 以逗号分隔的规范字符串转换Vector2;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Vector3 StrToVector3(string str)
        {
            if (string.IsNullOrEmpty(str))
                return Vector3.zero;

            Vector3 value = Vector3.zero;
            string[] strArray = str.Split(',');
            if (strArray.Length < 3)
            {
                Debug.LogError(str + " string to Vector2 failed with , spilt");
                return value;
            }
            float x = Convert.ToSingle(strArray[0]);
            float y = Convert.ToSingle(strArray[1]);
            float z = Convert.ToSingle(strArray[2]);
            value = new Vector3(x, y, z);

            return value;
        }

        /// <summary>
        /// 将Vector3转换成字符串;
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static string Vector3ToStr(Vector3 vec)
        {
            return vec.x + "," + vec.y + "," + vec.z;
        }

        /// <summary>
        /// 将Vector2转换成字符串;
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static string Vector2ToStr(Vector2 vec)
        {
            return vec.x + "," + vec.y;
        }

        /// <summary>
        /// 字符串转整型;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int Str2Int(string str)
        {
            return string.IsNullOrEmpty(str) == true ? 0 : Convert.ToInt32(str);
        }

        public static Int64 Str2Int64(string str)
        {
            return string.IsNullOrEmpty(str) == true ? 0 : Convert.ToInt64(str);
        }
        /// <summary>
        /// 字符串转换数组;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] Str2IntArray(string str)
        {
            if (str == "")
            {
                return new int[0];
            }
            int temp = -1;
            string[] strArray = str.Split(';');
            int[] intArray = new int[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                temp = Str2Int(strArray[i]);
                intArray[i] = temp;
            }

            return intArray;
        }

        /// <summary>
        /// 数组转换字符串;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Array2Str(int[] arr)
        {
            string temp = "";
            if (arr.Length > 0)
            {
                temp = arr[0].ToString();
            }

            for (int i = 1; i < arr.Length; i++)
            {
                temp += ";";
                temp += arr[i].ToString();
            }

            return temp;
        }
        public static string Array2Str(string[] arr)
        {
            string temp = "";
            if (arr.Length > 0)
            {
                temp = arr[0].ToString();
            }

            for (int i = 1; i < arr.Length; i++)
            {
                temp += ";";
                temp += arr[i].ToString();
            }

            return temp;
        }


        public static List<int> Str2IntList(string str)
        {
            int temp = -1;
            string[] strArray = str.Split(';');
            List<int> intList = new List<int>();
            for (int i = 0; i < strArray.Length; i++)
            {
                temp = Str2Int(strArray[i]);
                intList.Add(temp);
            }

            return intList;
        }

        /// <summary>
        /// 字符串转换数组;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] Str2StringArray(string str)
        {
            string[] strArray = str.Split(';');
            return strArray;
        }

        /// <summary>
        /// 字符串转成bool;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Str2Boolean(string str)
        {
            if (str == "1" || str.ToLower() == "true" || str.ToLower() == "yes")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 字符串转成float;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float Str2Float(string str)
        {
            return string.IsNullOrEmpty(str) == true ? 0 : Convert.ToSingle(str);
        }

        /// <summary>
        /// 转换4位小数浮点;
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static float ToFloat4(float number)
        {
            int int4 = ToInt4(number);
            return ToFloat4(int4);
        }

        /// <summary>
        /// 转换浮点;
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static float ToFloat4(int number)
        {
            return number / 10000.0f;
        }

        /// <summary>
        /// 转换4位整数;
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int ToInt4(float number)
        {
            return (int)(number * 10000);
        }

        /// <summary>
        /// 颜色转换;
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color GetColor(int r, int g, int b)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
        }

        #endregion

        #region 随机相关
        /// <summary>
        /// 根据概率获取随机数是否随机到;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool OnProbability(float value)
        {
            if (value >= 1)
                return true;

            if (value <= 0)
                return false;

            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng =
                new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            int seed = BitConverter.ToInt32(bytes, 0);

            System.Random random = new System.Random(seed);
            int randomValue = random.Next(100);
            if (randomValue < value * 100)
            {
                return true;
            }
            return false;
        }

        public static bool IsGetValue(int baseValue)
        {
            if (baseValue <= 0)
                return false;

            if (baseValue >= 100)
                return true;

            int rnd = UnityEngine.Random.Range(0, 100);

            return rnd <= baseValue;
        }
        #endregion

        /// <summary>
        /// 获取品质颜色;
        /// </summary>
        /// <param name="heroQuailty">0时为白   1时为绿   2时为蓝    3时为紫   4时为橙</param>
        /// <returns></returns>
        public static Color GetSummonerQualityColor(int heroQuailty)
        {
            Color color = Color.white;
            switch (heroQuailty)
            {
                case 0:
                    color = Color.white;
                    break;
                case 1:
                    color = Color.green;
                    break;
                case 2:
                    color = Color.blue;
                    break;
                case 3:
                    color = new Color(191f / 255f, 79f / 255f, 249f / 255f, 1f);
                    break;
                case 4:
                    color = new Color(255f / 255f, 139f / 255f, 36f / 255f, 1f);
                    break;
            }
            return color;
        }

        /// <summary>
        /// 转换钱币取“锭”
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int ToDing(long money)
        {
            return (int)(money / 1000000);
        }
        /// <summary>
        /// 转换钱币取“两”
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static int ToLiang(long money)
        {
            int liang = (int)(money / 1000);
            return liang % 1000;
        }
        /// <summary>
        /// 转换钱币取“文”
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static int ToWen(long money)
        {
            return (int)(money % 1000);
        }

        /// <summary>
        /// 随机一个Object
        /// </summary>
        /// <param name="????"></param>
        /// <returns></returns>
        public static object RandomObject(List<object> list)
        {
            if (list == null)
            {
                return null;
            }

            if (list.Count <= 0)
            {
                return null;
            }

            System.Random rnd = new System.Random();
            int result = rnd.Next(0, list.Count);

            return list[result];
        }
    }
}

