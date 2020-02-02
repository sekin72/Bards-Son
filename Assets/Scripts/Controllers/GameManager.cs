using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsWaitingForInputForText;

    public Player Player;
    public Enemy Goblin, Drake, Maw;
    public GameObject Stage1, Stage2;
    public Camera mainCamera;

    public List<Text> MyTextList;
    public List<Text> GameWonList;
    public Text currentText;
    private int textCounter = 0;

    public bool AfterCheckpoint = false;
    public Image Image;
    public UnityEngine.UI.Text ImageText;
    private void Awake()
    {
        Instance = this;
        MyTextList = new List<Text>();
        GameWonList = new List<Text>();

        PopulateTexts();

        IsWaitingForInputForText = true;
        Player.camera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        currentText = MyTextList[0];
        ImageText.text = currentText.DisplayText;
    }

    public void KillEnemy()
    {
        if (!Goblin.gameObject.activeInHierarchy)
        {
            Stage1.SetActive(false);
            EnhancePlayer1();
        }
        if (!Drake.gameObject.activeInHierarchy)
        {
            Stage2.SetActive(false);
            EnhancePlayer2();
        }
        ShowNextText();
    }

    public void EnhancePlayer1()
    {
        Player.HP = 20;
        Player.AC = 20;
        Player.attackBonus = 8;
        Player.damageBonus = 5;
    }

    public void EnhancePlayer2()
    {
        Player.HP = 30;
        Player.AC = 22;
        Player.attackBonus = 12;
        Player.damageBonus = 7;
    }

    public void PopulateTexts()
    {
        MyTextList.Add(new Text("Welcome to Bard's son. This game tells a story about a half-elf, Alcraes. Game uses tabletop FRP mechanics, meaning it rolls 4,6,8 or 20 sided dice to determine the gameplay or scenario outcomes. Press any key to continue...", ShowNextText));
        MyTextList.Add(new Text("Alcraes lives with her mother in a human only populated village. " +
            "His story actually is not strange to us. His mother is loved and respected in the village. One day, a known bard comes to town and a love sprout between them. " +
            "Sadly, he can't be found the next day. Only thing that he left was his bastard sword. After Alcraes was born, he was exluded by others because he was a half-elf.",
             ShowNextText));
        MyTextList.Add(new Text("After all, he looked different and strange. He had pointy ears but not good looking as an elf, tall but skinny body, more intelligent than his peers. " +
            "His mother raised him with stories about his father. Brave, handsome, talented... She can't except the truth that he left so she couldn't explain this to Alcraes.",
             ShowNextText));
        MyTextList.Add(new Text("Alcraes, growing up alienated, became a self-centered person who lacks self-confidence and courage. Only thing that he cared about was his father's stories and sword. " +
            "One day, their village was attacked by a horde of goblins. Picking up his father's sword, he hold of the goblins that came to his house. " +
            "His sword was broken in the process, but it was enough time for their neighbours to come help. " +
            "Blaming himself by being not good enough to wield the sword, he takes the first steps of adventure...", CloseText));
        MyTextList.Add(new Text("After defeating the goblin, his self-confidence begin to develop. Even his posture started to change. He was standing straight. " +
            "Curious what the future hold for him, he charged through!", () =>
            {
                Player.Courage = true;
                Player.StartIdleAnim();
                CloseText();
            }));
        MyTextList.Add(new Text("With the courage he found in himself, he broke the elemental to a million pieces!", () =>
        {
            ShowNextText();
        }));
        MyTextList.Add(new Text("Beyond the river, he saw a dwarf cornered by the hulking monstrosity, Alcraes needed to make a choice. Was he going to abandon him and move on, or help him?", () =>
        {
            AfterCheckpoint = true;
            CloseText();
        }));
        MyTextList.Add(new Text("After saving the dwarf, Alcraes learned that he was a good blacksmith. Showing him his father's broken sword, he faced the grave truth. " +
            "Apparently, the sword was nothing more than a regular bastard sword. Alcraes still asked him to fix it. He thought that his adventure was over with him fixing the sword. ", () =>
            {
                ShowNextText();
            }));
        MyTextList.Add(new Text("After several years, he understood what was the adventure's real meaning. His mission was fixing the sword, but he actually came back home repairing his personality. " +
            "He now was a courageous person who believed in himself to do anything. But more importantly, he was more considerate of others. This change in him, eventually made others to change their opinions and behaviours about him.", () =>
            {
                RandomGameWonText();
            }));

        GameWonList.Add(new Text("Alcraes married a beautiful women. What they had special. They had a happy marriage with two children.", () =>
        {
            SceneManager.LoadScene(0);
        }));
        GameWonList.Add(new Text("Alcraes married a handsome men. What they had special. They had a happy marriage with two children.", () =>
        {
            SceneManager.LoadScene(0);
        }));
        GameWonList.Add(new Text("Alcraes tasting adventure, decided to leave town after his mother's passing and became an adventurer. His name became a legend.", () =>
        {
            SceneManager.LoadScene(0);
        }));
        GameWonList.Add(new Text("Alcraes, after his mother's passing, decided to be a healer. He went to an academy, learned the ways of the healing and returned to his village. " +
            "His reputation grow each they as his skills.", () =>
        {
            SceneManager.LoadScene(0);
        }));
    }

    public void ShowNextText()
    {
        textCounter++;
        IsWaitingForInputForText = true;
        currentText = MyTextList[textCounter];
        ImageText.text = currentText.DisplayText;
        Image.gameObject.SetActive(true);
        Player.camera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
    public void CloseText()
    {
        IsWaitingForInputForText = false;
        Player.camera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        Image.gameObject.SetActive(false);
    }

    public void AbandonText()
    {
        IsWaitingForInputForText = true;
        currentText = new Text("Alcraes abandon the dwarf. Sadly, he was never able to abandon his self-centered personality. He was even more secluded from society.", () =>
        {
            SceneManager.LoadScene(0);
        });
        ImageText.text = currentText.DisplayText;
        Image.gameObject.SetActive(true);
        Player.camera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }

    public void RandomGameWonText()
    {
        IsWaitingForInputForText = true;
        currentText = GameWonList[Random.Range(0, GameWonList.Count)];
        ImageText.text = currentText.DisplayText;
        Image.gameObject.SetActive(true);
        Player.camera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
    public void Die()
    {
        IsWaitingForInputForText = true;
        currentText = new Text("Alcraes died.", () =>
        {
            SceneManager.LoadScene(0);
        });
        ImageText.text = currentText.DisplayText;
        Image.gameObject.SetActive(true);
        Player.camera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
}
