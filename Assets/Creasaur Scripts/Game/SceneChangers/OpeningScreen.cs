using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts.Game.SceneChangers
{
    public class OpeningScreen : MonoBehaviour
    {
        [Inject]
        private void Initializor(UserDataManager udm)
        {
            udm.OnUserDataLoaded += UserDataLoaded;
            udm.OnInitialize();
        }

        public void UserDataLoaded()
        {
            SceneManager.LoadScene("Loading");
        }
    }
}
