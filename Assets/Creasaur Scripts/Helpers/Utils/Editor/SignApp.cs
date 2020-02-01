using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Helpers.Utils.Editor
{
    public class SignApp
    {
        [MenuItem("Creasaur/Sign Keystore")]
        public static void SignKeystore()
        {
            PlayerSettings.Android.keystoreName = "key.keystore";// Directory.GetParent(Application.dataPath).GetFiles("*.keystore").First().FullName;
            PlayerSettings.Android.keyaliasName = "captainmerge";
            PlayerSettings.Android.keystorePass = "Creasaur1Game48a4!";
            PlayerSettings.Android.keyaliasPass = "Creasaur1Game48a4!";
        }
    }
}