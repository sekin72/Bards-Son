using System.Collections;
using System.Collections.Generic;
using System.IO;
using LogSystem;
using UnityEditor;
using UnityEngine;

namespace Environment
{
    [CustomEditor(typeof(ClientEnvironment))]
    public class ClientEnvironmentEditor : Editor
    {
        private ClientEnvironment _clientEnvironment;

        private EnvironmentType _lastEnvironment;
        private string _appDataPath;
        private const string configPaths = "/3rdParty/Firebase/";
        
        private void OnEnable()
        {
            _appDataPath = Application.dataPath;
            _clientEnvironment = (ClientEnvironment) target;
            _lastEnvironment = _clientEnvironment.EnvironmentType;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(_clientEnvironment == null)
                return;


            if (_lastEnvironment != _clientEnvironment.EnvironmentType)
            {
                OnEnvironmentTypeChanged();
                this.Log("Environment Changed To => " + _clientEnvironment.EnvironmentType);
                _lastEnvironment = _clientEnvironment.EnvironmentType;
            }
        }
        
        private void OnEnvironmentTypeChanged()
        {
            //var environment = _clientEnvironment.EnvironmentType;
        
            //var environmentName = environment == EnvironmentType.Local || environment == EnvironmentType.Development
            //    ? "dev-"
            //    : "prod-";

            //var googleServisesPath = _appDataPath + configPaths + environmentName + "google-services.json";
            //var googleServicesInfoPath = _appDataPath + configPaths + environmentName + "GoogleService-Info.plist";

            //var googleServicesFile = new FileInfo(googleServisesPath);
            //var googleServicesInfoFile = new FileInfo(googleServicesInfoPath);

            //googleServicesFile.CopyTo(_appDataPath + "/google-services.json", true);
            //googleServicesInfoFile.CopyTo(_appDataPath + "/GoogleService-Info.plist", true);
            //googleServicesFile.CopyTo(Application.streamingAssetsPath + "/google-services-desktop.json", true);
        }
    }
}
