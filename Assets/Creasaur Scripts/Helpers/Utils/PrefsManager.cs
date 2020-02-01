using System;
using CodeStage.AntiCheat.ObscuredTypes;
using Environment;
using LogSystem;
using Newtonsoft.Json;
using Zenject;

namespace Helpers.Utils
{
    public class PrefsManager
    {
        #region Variables

        public string DEFAULT_USER_NAME = "Guest";
        private string USER_ID_KEY = "USER_ID_{0}";
        private string USER_DATA_KEY = "USER_DATA_{0}";
        private string USER_TOKEN = "USER_TOKEN_{0}";
        
        private string CONFIGS_KEY = "CONFIGS_{0}";
        
        private string GLOBALRANKING_SAVETIME = "GlobalRanking_LastSaveTime_{0}";
        private string GLOBALRANKING_DATA = "GlobalRanking_{0}";
        private string LOCALRANKING_SAVETIME = "LocalRanking_LastShowTime_{0}";
        private string LOCALRANKING_DATA = "LocalRanking_{0}";
        private string FACEBOOKRANKING_SAVETIME = "FacebookRanking_LastShowTime_{0}";
        private string FACEBOOKRANKING_DATA = "FacebookRanking_{0}";
        private string SOUND_ENABLED = "SoundEnabled_{0}";
        private string MUSIC_ENABLED = "MusicEnabled_{0}";
        private string NOTIFICATION_ENABLED = "NotificationEnabled_{0}";
        
        #endregion

        #region Inject

        private ClientEnvironment _clientEnvironment;
        
        [Inject]
        public PrefsManager(ClientEnvironment clientEnvironment)
        {
            _clientEnvironment = clientEnvironment;

            DEFAULT_USER_NAME = string.Format(DEFAULT_USER_NAME, _clientEnvironment.EnvironmentType);
            USER_ID_KEY = string.Format(USER_ID_KEY, _clientEnvironment.EnvironmentType);
            USER_DATA_KEY = string.Format(USER_DATA_KEY, _clientEnvironment.EnvironmentType);
            
            CONFIGS_KEY = string.Format(CONFIGS_KEY, _clientEnvironment.EnvironmentType);
            
            GLOBALRANKING_SAVETIME = string.Format(GLOBALRANKING_SAVETIME, _clientEnvironment.EnvironmentType);
            GLOBALRANKING_DATA = string.Format(GLOBALRANKING_DATA, _clientEnvironment.EnvironmentType);
            LOCALRANKING_SAVETIME = string.Format(LOCALRANKING_SAVETIME, _clientEnvironment.EnvironmentType);
            LOCALRANKING_DATA = string.Format(LOCALRANKING_DATA, _clientEnvironment.EnvironmentType);
            FACEBOOKRANKING_SAVETIME = string.Format(FACEBOOKRANKING_SAVETIME, _clientEnvironment.EnvironmentType);
            FACEBOOKRANKING_DATA = string.Format(FACEBOOKRANKING_DATA, _clientEnvironment.EnvironmentType);
            
            SOUND_ENABLED = string.Format(SOUND_ENABLED, _clientEnvironment.EnvironmentType);
            MUSIC_ENABLED = string.Format(MUSIC_ENABLED, _clientEnvironment.EnvironmentType);
            NOTIFICATION_ENABLED = string.Format(NOTIFICATION_ENABLED, _clientEnvironment.EnvironmentType);
        }

        #endregion

        #region Check

        public bool HasKey(string dataKey)
        {
            bool hasValue = ObscuredPrefs.HasKey(dataKey);
            return hasValue;
        }

        public bool HasUserData()
        {
            return HasKey(USER_DATA_KEY);
        }

        public bool HasGameData()
        {
            return HasKey(CONFIGS_KEY);
        }
        
        public bool HasLocal()
        {
            return HasKey(GLOBALRANKING_SAVETIME);
        }
        
        #endregion

        #region Load & Save User Data

        public string GetToken()
        {
            if (ObscuredPrefs.HasKey(USER_TOKEN))
            {
                string token = ObscuredPrefs.GetString(USER_TOKEN);
                this.Log("Token saved to local storage");
                return token;
            }
            else
            {
                this.Log("Token not found");
                return null;
            }
        }
        
        public void SaveToken(string token)
        {
            this.Log("Token saved to local storage");
            ObscuredPrefs.SetString(USER_TOKEN, token);
        }

        public void SaveUserData(UserData user)
        {
            this.Log("Save user data to local storage");
            //if (!user.IsUserDeleted)
            {
                string saveData = JsonConvert.SerializeObject(user);
                ObscuredPrefs.SetString(USER_DATA_KEY, saveData);
            }
        }

        public UserData GetUserData()
        {
            this.Log("Load user data from storage");

            bool hasValue = ObscuredPrefs.HasKey(USER_DATA_KEY);
            if (hasValue)
            {
                string userData = ObscuredPrefs.GetString(USER_DATA_KEY);

                UserData user = JsonConvert.DeserializeObject<UserData>(userData);
                return user;
            }
            else
            {
                return null;
            }
        }

        public void DeleteUserData()
        {
            this.Log("Delete user prefs");
            ObscuredPrefs.DeleteAll();
        }
        
        #endregion

        #region Leader Board

//        public bool HasLeaderBoardData(LeaderBoardTabType leaderBoardTabType)
//        {
//            if (leaderBoardTabType == LeaderBoardTabType.Local)
//                return HasKey(LOCALRANKING_DATA);
//
//            if (leaderBoardTabType == LeaderBoardTabType.Global)
//                return HasKey(GLOBALRANKING_DATA);
//
//            if (leaderBoardTabType == LeaderBoardTabType.Friends)
//                return HasKey(FACEBOOKRANKING_DATA);
//
//            else return false;
//        }
//
//        public int GetLeaderBoardSaveDate(LeaderBoardTabType leaderBoardTabType)
//        {
//            DateTime dt = DateTime.Now;
//            
//            if (leaderBoardTabType == LeaderBoardTabType.Local)
//                dt = GetDateLatestSaveDate(LOCALRANKING_SAVETIME);
//            
//            if (leaderBoardTabType == LeaderBoardTabType.Global)
//                dt = GetDateLatestSaveDate(GLOBALRANKING_SAVETIME);
//            
//            if (leaderBoardTabType == LeaderBoardTabType.Friends)
//                dt = GetDateLatestSaveDate(FACEBOOKRANKING_SAVETIME);
//
//            TimeSpan localTimeSpan = DateTime.Now.Subtract (dt);
//            int result = (int)localTimeSpan.TotalSeconds;
//            if (result < 0)
//                result = 0;
//            
//            return result;
//        }

        
        private DateTime GetDateLatestSaveDate(string key)
        {
            DateTime dt = DateTime.Now;
            
            if (ObscuredPrefs.HasKey(key))
            {
                string dateTime = ObscuredPrefs.GetString(key);
                dt = Convert.ToDateTime(dateTime);
            }

            return dt;
        }
        
//        public List<LeaderBoardData> GetLeaderBoardData(LeaderBoardTabType leaderBoardTabType)
//        {
//            List<LeaderBoardData> tmp = new List<LeaderBoardData>();
//
//            if (leaderBoardTabType == LeaderBoardTabType.Local)
//            {
//                bool hasValue = ObscuredPrefs.HasKey(LOCALRANKING_DATA);
//                if (hasValue)
//                {
//                    string data = ObscuredPrefs.GetString(LOCALRANKING_DATA);
//                    tmp = JsonConvert.DeserializeObject<List<LeaderBoardData>>(data);
//                }
//                else
//                {
//                    return null;
//                }
//            }
//
//            // Global
//            if (leaderBoardTabType == LeaderBoardTabType.Global)
//            {
//                bool hasValue = ObscuredPrefs.HasKey(GLOBALRANKING_DATA);
//                if (hasValue)
//                {
//                    string data = ObscuredPrefs.GetString(GLOBALRANKING_DATA);
//                    tmp = JsonConvert.DeserializeObject<List<LeaderBoardData>>(data);
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            
//            // Facebook
//            if (leaderBoardTabType == LeaderBoardTabType.Friends)
//            {
//                bool hasValue = ObscuredPrefs.HasKey(FACEBOOKRANKING_DATA);
//                if (hasValue)
//                {
//                    string data = ObscuredPrefs.GetString(FACEBOOKRANKING_DATA);
//                    tmp = JsonConvert.DeserializeObject<List<LeaderBoardData>>(data);
//                }
//                else
//                {
//                    return null;
//                }
//            }
//
//            return tmp;
//        }
//        
//        public void SaveLeaderBoard(LeaderBoardTabType leaderBoardTabType, List<LeaderBoardData> leaderBoardData)
//        {
//            var dataTName = "";
//            var dateSaveKey = "";
//            DateTime dt = DateTime.Now;
//            
//            string data = JsonConvert.SerializeObject(leaderBoardData);
//
//            if (leaderBoardTabType == LeaderBoardTabType.Global)
//            {
//                dataTName = GLOBALRANKING_DATA;
//                dateSaveKey = GLOBALRANKING_SAVETIME;
//            }
//            else if (leaderBoardTabType == LeaderBoardTabType.Local)
//            {
//                dataTName = LOCALRANKING_DATA;
//                dateSaveKey = LOCALRANKING_SAVETIME;
//            }
//            else
//            {
//                dataTName = FACEBOOKRANKING_DATA;
//                dateSaveKey = FACEBOOKRANKING_SAVETIME;
//            }
//            
//            this.Log("Leader board saved to local storage. [" + dataTName + "]");
//            ObscuredPrefs.SetString(dataTName, data);
//            ObscuredPrefs.SetString(dateSaveKey, DateTime.Now.ToString());
//        }

        #endregion

        #region Game Config
        
        //public void SaveGameConfig(ResponseGameData gameConfig)
        //{
        //    this.Log("Game config saved to local storage");
            
        //    string saveData = JsonConvert.SerializeObject(gameConfig);            
        //    ObscuredPrefs.SetString(CONFIGS_KEY, saveData);
        //}

        //public ResponseGameData LoadGameConfig()
        //{
        //    this.Log("Game config loaded from local storage");

        //    ResponseGameData loadedData = null;

        //    if (HasGameData())
        //    {
        //        string data = ObscuredPrefs.GetString(CONFIGS_KEY);
        //        loadedData = JsonConvert.DeserializeObject<ResponseGameData>(data);
        //    }

        //    return loadedData;
        //}
        
        #endregion

        #region Audio Stats

        public bool GetSoundStatus()
        {
            bool soundStatus = true;
            
            if (ObscuredPrefs.HasKey(SOUND_ENABLED))
            {
                soundStatus = ObscuredPrefs.GetBool(SOUND_ENABLED);
            }
            
            return soundStatus;
        }

        public bool GetMusicStatus()
        {
            bool soundStatus = true;
            
            if (ObscuredPrefs.HasKey(MUSIC_ENABLED))
            {
                soundStatus = ObscuredPrefs.GetBool(MUSIC_ENABLED);
            }
            
            return soundStatus;
        }

        public void SetSoundStatus(bool status)
        { 
            ObscuredPrefs.SetBool(SOUND_ENABLED, status);
        }

        public void SetMusicStatus(bool status)
        { 
            ObscuredPrefs.SetBool(MUSIC_ENABLED, status);
        }

        #endregion

        #region Notification Settings

        public bool GetNotificationStatus()
        {
            bool soundStatus = true;
            
            if (ObscuredPrefs.HasKey(NOTIFICATION_ENABLED))
            {
                soundStatus = ObscuredPrefs.GetBool(NOTIFICATION_ENABLED);
            }
            
            return soundStatus;
        }

        public void SetNotificationStatus(bool status)
        { 
            ObscuredPrefs.SetBool(NOTIFICATION_ENABLED, status);
        }

        
        

        #endregion
        
    }
}