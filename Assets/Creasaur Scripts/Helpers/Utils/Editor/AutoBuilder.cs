using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Helpers.Utils.Editor;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

    public static class AutoBuilder
    {
        #region Variables

        private static string[] _scenes = null;
        
        private static string _versionNumber;
        private static string _buildNumber;

        private static string _appName = "Captain Merge";
        private static string _applicationIdentifier = "com.creasaur.captainmerge";
        private static string _defaultApkName = "captainmerge";
        private static string _keyStorePass = "Creasaur1Game48a4!";
        private static string _keyAliasName = "captainmerge";
        private static string _keyAliasPass = "Creasaur1Game48a4!";
        
        // User Params
        private static bool _isDebugEnabled = false;
        private static BuildType _buildType;
        private static string _fileName = "";

        private static string iosBuildFolder = "Build/Ios";
        
        // Params
        private static string _paramFileName = "";

        #endregion
        
        #region Android

        [MenuItem ("Creasaur/CI/Build Android")]
        static void PerformAndroidBuild()
        {
            _buildType = BuildType.Android;

            BuildScript();

            //_targetData = "Builds/Android";
            //EditorPrefs.SetString("AndroidSdkRoot", "C:/Program Files/Java/jdk1.8.0_181");
            
            PlayerSettings.applicationIdentifier = _applicationIdentifier;
            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = @"key.keystore";// Directory.GetParent(Application.dataPath).GetFiles("*.keystore").First().FullName;
            PlayerSettings.Android.keystorePass = _keyStorePass;
            PlayerSettings.Android.keyaliasName = _keyAliasName;
            PlayerSettings.Android.keyaliasPass = _keyAliasPass;

            
            Debug.Log("KeyStorePath > " + PlayerSettings.Android.keystoreName + " - KeyStoreAlias > " + _keyAliasName + " - KeyStorePass > " + _keyStorePass + " - KeyStoreAliasPass > " + _keyAliasPass);

            EditorUserBuildSettings.buildAppBundle = true;
            
            BuildOptions buildOptions = BuildOptions.CompressWithLz4HC;
            if (_isDebugEnabled) buildOptions |= BuildOptions.Development;
            
            var buildResponse = BuildPipeline.BuildPlayer(_scenes, _fileName, BuildTarget.Android, buildOptions);
            BuildSummary summary = buildResponse.summary;
            
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
            
            #if UNITY_EDITOR == false
            EditorApplication.Exit(buildResponse.summary.result == BuildResult.Succeeded ? 0 : 2);
            #endif
        }
        
        #endregion
        
        #region iOS

        [MenuItem ("Creasaur/CI/Build iOS")]
        static void PerformiOSBuild()
        {
            _buildType = BuildType.Ios;

            BuildScript();
                        
            string folder = Path.Combine(Directory.GetParent(Application.dataPath).FullName, iosBuildFolder);
            // CreateBuildFolder(folder);
            // LookForLastIosBuilds(folder);

            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            
            BuildOptions buildOptions = BuildOptions.CompressWithLz4HC;
            if (_isDebugEnabled) buildOptions |= BuildOptions.Development;

            var report = BuildPipeline.BuildPlayer(_scenes, folder, BuildTarget.iOS, buildOptions);

            string projectPath = folder + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);
            string target = pbxProject.TargetGuidByName("Unity-iPhone");

            //pbxProject.SetTeamId(target, "36ZKA76W95");
            pbxProject.AddCapability(target, PBXCapabilityType.InAppPurchase);
            pbxProject.AddCapability(target, PBXCapabilityType.PushNotifications);
            pbxProject.AddCapability(target, PBXCapabilityType.BackgroundModes);

            pbxProject.SetBuildProperty(target, "USYM_UPLOAD_AUTH_TOKEN", "A1");
            pbxProject.WriteToFile (projectPath);

            // Add background modes
            string plistPath = folder + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;
            
            PlistElementArray UIBackgroundModes = rootDict.CreateArray("UIBackgroundModes");
            UIBackgroundModes.AddString("remote-notification");
            
            //PlistElementDict NSAppTransportSecurity = rootDict.CreateDict("NSAppTransportSecurity");
            //NSAppTransportSecurity.SetBoolean("NSAllowsArbitraryLoads",true);
            //NSAppTransportSecurity.SetBoolean("NSAllowsArbitraryLoads",true);

            //arr.AddString("fetch");
            File.WriteAllText(plistPath, plist.WriteToString());

            #if UNITY_EDITOR == false

            //EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : 2);
            
            #endif
        }

        #endregion
        
        #region General
        
        static void BuildScript()
        {
            _scenes = GetScenes();
            
            _versionNumber = System.Environment.GetEnvironmentVariable("VERSION_NUMBER");
            if (string.IsNullOrEmpty(_versionNumber))
                _versionNumber = "1";

            _buildNumber = System.Environment.GetEnvironmentVariable("BUILD_NUMBER");
            if (string.IsNullOrEmpty(_buildNumber))
                _buildNumber = "1";


            if (_buildType == BuildType.Ios)
            {
                _fileName = "Build/Ios";
            }
            else
            {
                _paramFileName = GetParam("-filename");
                if (_paramFileName != "" && _paramFileName != null)
                {
                    _fileName = "Build/Android/" + _paramFileName + ".aab";
                }
                else
                {
                    _fileName = "Build/Android/" + _defaultApkName + DateTime.Now.ToString("yy-MM-dd-HH-mm") + ".aab";
                }
            }

            
            // Is Debug Enabled
            string debugEnabled = GetParam("-debug");
            if (debugEnabled == "1")
            {
                _isDebugEnabled = true;
            }
            else
                _isDebugEnabled = false;
            
            
            // BuildNumber
            string buildNumber = GetParam("-buildnumber");
            if (buildNumber != "")
                _buildNumber = buildNumber;
            else
                _buildNumber = "1";

            
            // var debug = CommandLineReader.GetCustomArgument("debug");
            // _isDebugEnabled = !string.IsNullOrEmpty(debug) && int.Parse(debug) == 1;

            Debug.unityLogger.logEnabled = _isDebugEnabled;
            SRDebugger.Settings.Instance.IsEnabled = _isDebugEnabled;

            if (_buildType == BuildType.Ios)
            {
                PlayerSettings.iOS.appleDeveloperTeamID = "36ZKA76W95";
                PlayerSettings.iOS.appleEnableAutomaticSigning = true;
                PlayerSettings.iOS.buildNumber = _buildNumber;
                PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Low);
            }
            else
            {
                PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Disabled);
            }

            
            
            Debug.Log("Auto Builder: Started");
            Debug.Log("Debug Enabled: " + _isDebugEnabled.ToString());
            Debug.Log("Build Platform: " + _buildType.ToString());
            Debug.Log("File Name: " + _fileName);
            
            // Debug.Log("Building commit > " + commit + " - BuildNumber > " + build + " - Debug open > " + isDebugOpen + " - Strip level > " + PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.Android) + " - Development > " + _isDevelopmentBuild);

            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SplashScreen.show = false;
            PlayerSettings.productName = _appName;
            PlayerSettings.bundleVersion = _versionNumber;
        }
        
        static string[] GetScenes()
        {
            return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        }

        private static string GetParam(string name)
        {
            Debug.Log("GetParam: " + name);
            
            var args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }
            
            return null;
        }
        
        private class BuildFile
        {
            public string Path;
            public DateTime Time;
        }
        
        private static void CreateBuildFolder(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }
        
        private static void LookForLastIosBuilds(string path)
        {
            var files = Directory.GetFiles(path, "*.ipa", SearchOption.AllDirectories);
            List<BuildFile> TheList = files.Select(file => new BuildFile()
            {
                Path = file,
                Time = File.GetCreationTimeUtc(file)
            }).ToList();

            TheList.Sort((l1, l2) => l1.Time.CompareTo(l2.Time));

            if (files.Length > 9)
            {
                int r = files.Length - 9;

                for (int i = 0; i < r; i++)
                {
                    File.Delete(TheList[i].Path);
                }
            }
        }

        #endregion
        
    }