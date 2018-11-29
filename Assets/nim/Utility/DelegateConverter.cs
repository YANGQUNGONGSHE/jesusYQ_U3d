using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace NimUtility
{
    /// <summary>
    /// 转换 CLR Delegate 和 Native function pointer
    /// </summary>
    public static class DelegateConverter
    {
        private static readonly Dictionary<IntPtr, string> _allocedMemDic = new Dictionary<IntPtr, string>();

        public static IntPtr ConvertToIntPtr(Delegate d)
        {
            if (d != null)
            {
                GCHandle gch = GCHandle.Alloc(d);
                IntPtr ptr = GCHandle.ToIntPtr(gch);
                _allocedMemDic[ptr] = d.Method.Name;
                return ptr;
            }
            return IntPtr.Zero;
        }

        public static IntPtr ConvertToIntPtr(object obj)
        {
            if (obj != null)
            {
                GCHandle gch = GCHandle.Alloc(obj);
                IntPtr ptr = GCHandle.ToIntPtr(gch);
                _allocedMemDic[ptr] = obj.ToString();
                return ptr;
            }
            return IntPtr.Zero;
        }

        public static T ConvertFromIntPtr<T>(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return default(T);
            GCHandle handle = GCHandle.FromIntPtr(ptr);
            var x = (T)handle.Target;
            return x;
        }

        public static object ConvertFromIntPtr(IntPtr ptr)
        {
            return ConvertFromIntPtr<object>(ptr);
        }

        public static void Invoke<TDelegate>(this IntPtr ptr,params object[] args)
        {
            var d = ConvertFromIntPtr<TDelegate>(ptr);
            var delegateObj = d as Delegate;
            if (delegateObj != null)
            {
                System.Diagnostics.Debug.Assert(CheckDelegateParams(delegateObj, args));
                delegateObj.Method.Invoke(delegateObj.Target, args);
            }
        }

        public static void InvokeOnce<TDelegate>(this IntPtr ptr, params object[] args)
        {
            var d = ConvertFromIntPtr<TDelegate>(ptr);
            var delegateObj = d as Delegate;
            if (delegateObj != null)
            {
                System.Diagnostics.Debug.Assert(CheckDelegateParams(delegateObj, args));
                delegateObj.Method.Invoke(delegateObj.Target, args);
                FreeMem(ptr);
            }
        }

        static bool CheckDelegateParams(Delegate d,params object[] args)
        {
            var ps = d.Method.GetParameters();
            if (args == null)
                return true;
            if (args.Count() != ps.Count())
                return false;
            for (int i = 0; i < args.Count(); i++)
            {
                if (args[i] == null || ps[i].ParameterType.IsValueType)
                    continue;
                if (!ps[i].ParameterType.IsInstanceOfType(args[i]))
                    return false;
            }
            return true;
        }

        public static void FreeMem(this IntPtr ptr)
        {
            _allocedMemDic.Remove(ptr);
            GCHandle handle = GCHandle.FromIntPtr(ptr);
            handle.Free();
        }

        public static void ClearHandles()
        {
            foreach(var item in _allocedMemDic)
            {
                GCHandle handle = GCHandle.FromIntPtr(item.Key);
                handle.Free();
            }
            _allocedMemDic.Clear();
        }
    }
}
