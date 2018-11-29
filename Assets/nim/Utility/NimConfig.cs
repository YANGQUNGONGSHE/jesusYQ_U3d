/** @file NimConfig.cs
  * @brief NIM SDK提供的SDK配置定义
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author Harrison
  * @date 2015/12/8
  */

using System.Collections.Generic;

namespace NimUtility
{
    /// <summary>
    /// SDK log级别，级别越高，log越详细 
    /// </summary>
    public enum SdkLogLevel
    {
        /// <summary>
        /// SDK Fatal级别Log
        /// </summary>
        Fatal = 1,

        /// <summary>
        /// SDK Error级别Log
        /// </summary>
        Error = 2,

        /// <summary>
        /// SDK Warn级别Log
        /// </summary>
        Warn = 3,

        /// <summary>
        /// 应用级别Log，正式发布时为了精简sdk log，可采用此级别
        /// </summary>
        App = 5,

        /// <summary>
        /// SDK调试过程级别Log，更加详细，更有利于开发调试
        /// </summary>
        Pro = 6
    };

    public class SdkCommonSetting
    {
        /// <summary>
        /// 数据库秘钥，必填，目前只支持最多32个字符的加密密钥！建议使用32个字符
        /// </summary>
        [Newtonsoft.Json.JsonProperty("db_encrypt_key")]
        public string DataBaseEncryptKey { get; set; }

        /// <summary>
        /// 必填，是否需要预下载附件缩略图，默认为true
        /// </summary>
        [Newtonsoft.Json.JsonProperty("preload_attach")]
        public bool PredownloadAttachmentThumbnail { get; set; }

        /// <summary>
        /// 定义见SdkLogLevel选填，SDK默认的内置级别为Pro
        /// </summary>
        [Newtonsoft.Json.JsonProperty("sdk_log_level")]
        public SdkLogLevel LogLevel { get; set; }

        /// <summary>
        /// 选填，是否使用私有服务器
        /// </summary>
        [Newtonsoft.Json.JsonProperty("private_server_setting")]
        public bool UsePriviteServer { get; set; }

        /// <summary>
        /// 登录超时，单位秒，默认30
        /// </summary>
        [Newtonsoft.Json.JsonProperty("custom_timeout")]
        public int CustomTimeout { get; set; }

        /// <summary>
        /// 登录重试最大次数，如需设置建议设置大于3次
        /// </summary>
        [Newtonsoft.Json.JsonProperty("login_retry_max_times")]
        public int MaxLoginRetry { get; set; }

#if UNITY_IPHONE || UNITY_IOS
		/// <summary>
		/// iOS 推送证书名配置
		/// </summary>
		[Newtonsoft.Json.JsonProperty("push_cer_name")]
		public string PushCerName { get; set;}
#endif
		/// <summary>
		/// 预下载图片质量,选填,范围0-100
		/// </summary>
		[Newtonsoft.Json.JsonProperty("preload_image_quality")]
		public int PreloadImageQuality { get; set; }

		/// <summary>
		/// 预下载图片基于长宽做内缩略,选填,比如宽100高50,则赋值为100x50,中间为字母小写x 
		/// </summary>
		[Newtonsoft.Json.JsonProperty("preload_image_resize")]
		public string PreloadImageResize { get; set; }

		/// <summary>
		/// 设置是否已读未读状态多端同步，默认true
		/// </summary>
		[Newtonsoft.Json.JsonProperty("sync_session_ack")]
		public bool SyncSessionAck { get; set; }
#if !NIMAPI_UNDER_WIN_DESKTOP_ONLY
		/// <summary>
		/// 设置是否不保存自定义(kNIMMessageTypeCustom)消息（对PC版本SDK不支持，仅对Unity、cocos版本有效），默认为false
		/// </summary>
		[Newtonsoft.Json.JsonProperty("not_need_save_custom_msg")]
		public bool NotNeedSaveCustomMsg { get; set; }
#endif

#if NIMAPI_UNDER_WIN_DESKTOP_ONLY
        /// <summary>
        /// 是否启用HTTPS协议，默认为false
        /// </summary>
        [Newtonsoft.Json.JsonProperty("use_https")]
        public bool UseHttps { get; set; }

        /// <summary>
        /// 群通知是否计入未读数，默认为false
        /// </summary>
        [Newtonsoft.Json.JsonProperty("team_notification_unread_count")]
        public bool CountingTeamNotification { get; set; }


#endif

        public SdkCommonSetting()
        {
            PredownloadAttachmentThumbnail = true;
            UsePriviteServer = false;
            LogLevel = SdkLogLevel.Pro;
			PreloadImageQuality = -1;
			SyncSessionAck = true;
#if !NIMAPI_UNDER_WIN_DESKTOP_ONLY
			NotNeedSaveCustomMsg = true;
#endif
#if NIMAPI_UNDER_WIN_DESKTOP_ONLY            
            CustomTimeout = 30;
            UseHttps = false;    
#endif
        }
    }

    public class SdkPrivateServerSetting
    {
        /// <summary>
        /// lbs地址，如果选择使用私有服务器，则必填
        /// </summary>
        [Newtonsoft.Json.JsonProperty("lbs")]
        public string LbsAddress { get; set; }

        /// <summary>
        /// nos lbs地址，如果选择使用私有服务器，则必填
        /// </summary>
        [Newtonsoft.Json.JsonProperty("nos_lbs")]
        public string NOSLbsAddress { get; set; }

        /// <summary>
        /// 默认link服务器地址，如果选择使用私有服务器，则必填
        /// </summary>
        [Newtonsoft.Json.JsonProperty("default_link")]
        public List<string> LinkServerList { get; set; }

        /// <summary>
        /// 默认nos 上传服务器地址，如果选择使用私有服务器，则必填
        /// </summary>
        [Newtonsoft.Json.JsonProperty("default_nos_upload")]
        public List<string> UploadServerList { get; set; }

        /// <summary>
        /// 默认nos 下载服务器地址，如果选择使用私有服务器，则必填
        /// </summary>
        [Newtonsoft.Json.JsonProperty("default_nos_download")]
        public List<string> DownloadServerList { get; set; }

        /// <summary>
        /// 默认nos access服务器地址，如果选择使用私有服务器，则必填
        /// </summary>
        [Newtonsoft.Json.JsonProperty("default_nos_access")]
        public List<string> AccessServerList { get; set; }

        /// <summary>
        /// RSA public key，如果选择使用私有服务器，则必填
        /// </summary>
        [Newtonsoft.Json.JsonProperty("rsa_public_key_module")]
        public string RSAPublicKey { get; set; }

        /// <summary>
        /// RSA version，如果选择使用私有服务器，则必填
        /// </summary>
        [Newtonsoft.Json.JsonProperty("rsa_version")]
        [Newtonsoft.Json.JsonIgnore()]
        public int RsaVersion { get; set; }

        public SdkPrivateServerSetting()
        {
            RsaVersion = 0;
        }
    }

    public class NimConfig : NimUtility.NimJsonObject<NimConfig>
    {
        [Newtonsoft.Json.JsonProperty("global_config")]
        public SdkCommonSetting CommonSetting { get; set; }

        /// <summary>
        /// 私有服务器配置（一旦设置了私有服务器，则全部连私有服务器，必须确保配置正确！
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "private_server_setting", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public SdkPrivateServerSetting PrivateServerSetting { get; set; }

        /// <summary>
        /// AppKey
        /// </summary>
        [Newtonsoft.Json.JsonProperty("app_key")]
        public string AppKey { get; set; }

        public bool IsValiad()
        {
            return CommonSetting != null;
        }
    }
}
