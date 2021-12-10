using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int StagePoint;
    public int stageIndex;
    public int FragCount;
    public int LifeCount;

    public GameObject[] Stages;
    public GameObject StartStage;
    public GameObject Startmenu;
    public GameObject Gameset;
    public GameObject UImain;
    public GameObject player_online;
    public PlayerMove player;
    public Image[] UIhealth;
    public Text UIPoint;
    public Text UILife;
    public Text UIStage;
    public GameObject[] Checkpoint;
    public GameObject UIRestartBtn;
    public bool isUnBeatTime;
    public float unBeatTime;

    void Update()
    {
        UIPoint.text = (totalPoint + StagePoint).ToString();
    }

    public void OnClickStart()
    {
        UImain.SetActive(true);
        Startmenu.SetActive(false);
    }
    public void OnClickExit()
    {
        Application.Quit();
    }

    public void NextStage()
    {   
        //change stage
        if(stageIndex < Stages.Length - 1) {
            Stages[stageIndex].SetActive(false);
            stageIndex = stageIndex + 1;         
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
            //Stage Ç¥½Ã
            UIStage.text = "STAGE " + (stageIndex + 1);
        }

        else
        {//Game Clear
         //player control lock
            Time.timeScale = 0;
            //Result UI
            Debug.Log("Finish");
            //Restart button
            Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
            btnText.text = "Clear!";
            Gameset.SetActive(true);
            UIRestartBtn.SetActive(true);
        }
        //Calculate Point
        totalPoint += StagePoint;
        StagePoint = 0;
    }

    public void HealthDown()
    {
        if (LifeCount > 1)
        {
            Debug.Log("damaged");
            Thread.Sleep(25);
            LifeCount = LifeCount - 1;
            UILife.text = LifeCount.ToString();
        }
        else
        {
            Time.timeScale = 0;
            //player die effect
            player.OnDie();
            //Result UI
            Debug.Log("DEAD");
            //Retry
            UIRestartBtn.SetActive(true);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (LifeCount > 1)
            {//Player Respon
                PlayerReposition();
            }
            //Health down
            HealthDown();
        }
    }
    void PlayerReposition()
    {
        player.transform.position = new Vector3(-4, 0.5f, -1);
        player.VelocityZero();
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main01");
    }    
}
