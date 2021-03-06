﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/**
 *  Function : Jenkins 自动化构建项目
 *             需要跑在Mac上，Win不支持。因为Mac可以构建仍和平台。Win是傻逼。
 * 	Author: WongJJ
 * 	Date: 2017年09月26日15:52:38
 */
public class BuildProject
{
	/// <summary>
	/// 获取需要构建的场景
	/// </summary>
	/// <value>The get build scenes.</value>
    static string[] GetBuildScenes
	{
		get
		{
			List<string> buildScneces = new List<string> ();
			foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes) 
			{
				if (s.enabled)
					buildScneces.Add (s.path);
			}
            return buildScneces.ToArray();
		}
	}

    /// <summary>
    /// 获取构建平台
    /// </summary>
    /// <value>The get build target.</value>
	static BuildTargetGroup GetBuildTarget
	{
		get
		{
            BuildTargetGroup target = BuildTargetGroup.iOS;
			string[] strArr = System.Environment.GetCommandLineArgs ();
			foreach(string str in strArr)
			{
				if (str.StartsWith ("type")) 
				{
                    string str1 = str.Replace("type=", "");
                    switch (str1)
                    {
                        case "Android":
                            target = BuildTargetGroup.Android;
                            break;

                        case "iOS":
                            target = BuildTargetGroup.iOS;
                            break;
                    }
				}
			}
            return target;
		}
	}

    /// <summary>
    /// 获取构建版本
    /// </summary>
    /// <value>The get version.</value>
    static string GetVersion
    {
        get
        {
            string version = string.Empty;
            string[] strArr = System.Environment.GetCommandLineArgs();
            foreach (string str in strArr)
            {
                if (str.StartsWith("version"))
                {
                    version = str.Replace("version", "");
                }
                else
                    version = "1.0";
            }
            return version;
        }
    }


    /// <summary>
    /// 获取生成到的路径
    /// </summary>
    /// <value>The get out put path.</value>
    static string GetOutPutPath
    {
        get
        {
            string outPutPath = string.Empty;
            string[] strArr = System.Environment.GetCommandLineArgs();
            foreach (string str in strArr)
            {
                if (str.StartsWith("ouputpath"))
                {
                    outPutPath = str.Replace("ouputpath=", "");
                }
                else
                    outPutPath = "./../AutoGenerated";
            }
            return outPutPath;
        }
    }

    [UnityEditor.MenuItem("Tools/bulid")]
    public static void Build()
    {
        PlayerSettings.bundleVersion = GetVersion;
        PlayerSettings.productName = "羊群公社";
        if (GetBuildTarget == BuildTargetGroup.Android)
        {
            BuildForAndroid();
        }
        else
        {
            BuildForiOS();
        }
    }

    private static void BuildForAndroid()
    {
        PlayerSettings.applicationIdentifier = "com.sheep.jesus";
        string res = BuildPipeline.BuildPlayer(GetBuildScenes, GetOutPutPath, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
    }

    private static void BuildForiOS()
    {
        PlayerSettings.applicationIdentifier = "com.sheep.jesus";
        string res = BuildPipeline.BuildPlayer(GetBuildScenes, GetOutPutPath, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
    }
}
