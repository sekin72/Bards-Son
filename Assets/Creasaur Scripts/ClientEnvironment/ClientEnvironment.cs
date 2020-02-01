using UnityEngine;
using Zenject;

namespace Environment
{
    public class ClientEnvironment : MonoBehaviour
    {
        public EnvironmentType EnvironmentType;

        public static EnvironmentType StaticEnvironmentType;
        
        public string Url
        {
            get
            {
                //StaticEnvironmentType = EnvironmentType;

                //if (!string.IsNullOrEmpty(url))
                //    return url;

                //if (EnvironmentType == EnvironmentType.Local)
                //{
                //    #if !UNITY_EDITOR
                //         url = "https://masherzdev.creasaurco.com";
                //    #else
                //        url = "http://192.168.2.40:3000";
                //    #endif
                //}
                //else if (EnvironmentType == EnvironmentType.Development)
                //{
                //    url = "https://captainmergedev.creasaurco.com";
                //}
                //else if (EnvironmentType == EnvironmentType.Production)
                //{
                //    url = "https://captainmerge.creasaurco.com";
                //}

                //return url;
                return string.Empty;
            }
        }
        
        private string url;

    }
}
