using NimUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NIM
{

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void nim_sdk_exception_cb_func(NIMSDKException exception,
           [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]string log,
           IntPtr user_data);

    class NIMGlobalNativeMethods
    {
        #region NIM C SDK native methods

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void nim_sdk_log_cb_func(int log_level,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]string log,
            IntPtr user_data);

        [DllImport(NIM.NativeConfig.NIMNativeDLL, EntryPoint = "nim_global_free_str_buf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void nim_global_free_str_buf(IntPtr str);

        [DllImport(NIM.NativeConfig.NIMNativeDLL, EntryPoint = "nim_global_free_buf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void nim_global_free_buf(IntPtr data);

        [DllImport(NIM.NativeConfig.NIMNativeDLL, EntryPoint = "nim_global_reg_sdk_log_cb", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void nim_global_reg_sdk_log_cb([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]string jsonExt,
            nim_sdk_log_cb_func cb, IntPtr data);


#if NIMAPI_UNDER_WIN_DESKTOP_ONLY
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void nim_global_detect_proxy_cb_func(bool network_connect, NIMProxyDetectStep step,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]string json_params,
            IntPtr user_data);

        [DllImport(NIM.NativeConfig.NIMNativeDLL, EntryPoint = "nim_global_set_proxy", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void nim_global_set_proxy(NIMProxyType type,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))] string host,
            int port,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))] string user,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))] string password);

        [DllImport(NIM.NativeConfig.NIMNativeDLL, EntryPoint = "nim_global_detect_proxy", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void nim_global_detect_proxy(NIMProxyType type,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]string host, 
            int port,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]string user,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]string password, 
            nim_global_detect_proxy_cb_func cb, IntPtr user_data);

        [DllImport(NIM.NativeConfig.NIMNativeDLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void nim_global_reg_exception_report_cb([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]string json_extension, 
            nim_sdk_exception_cb_func cb, 
            IntPtr user_data);

#endif
        #endregion
    }
}
