using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    public static SoundMgr instance; 
    private void Awake(){
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        else
            Destroy(gameObject);

        }

    public void SFXPlay(string sfxName, AudioClip clipF) //ÆøÅºÅõÃ´ »ç¿îµå
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = clipF;
        audiosource.Play();

        Destroy(go, clipF.length);
    }
    public void SFXPlay2(string sfxName, AudioClip clipB)//ÃÑ¾Ë »ç¿îµå
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = clipB;
        audiosource.Play();

        Destroy(go, clipB.length);
    }
    public void SFXPlay3(string sfxName, AudioClip Item_Frag)//¼ö·ùÅº ¾ÆÀÌÅÛ(Áõ°¡) »ç¿îµå
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = Item_Frag;
        audiosource.Play();

        Destroy(go, Item_Frag.length);
    }
    public void SFXPlay4(string sfxName, AudioClip Item_gun)//ÃÑ¾Ë RPM Áõ°¡ ¾ÆÀÌÅÛ »ç¿îµå
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = Item_gun;
        audiosource.Play();

        Destroy(go, Item_gun.length);
    }
    public void SFXPlay5(string sfxName, AudioClip Bronze)//Bronze
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = Bronze;
        audiosource.Play();

        Destroy(go, Bronze.length);
    }
    public void SFXPlay6(string sfxName, AudioClip Silver)//Silver
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = Silver;
        audiosource.Play();

        Destroy(go, Silver.length);
    }
    public void SFXPlay7(string sfxName, AudioClip Gold)//Gold
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = Gold;
        audiosource.Play();

        Destroy(go, Gold.length);
    }
    public void SFXPlay8(string sfxName, AudioClip DroneExp)//µå·ÐÆøÆÄ
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = DroneExp;
        audiosource.Play();

        Destroy(go, DroneExp.length);
    }
    public void SFXPlay9(string sfxName, AudioClip TurretExp)//ÅÍ·¿ÆøÆÄ
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = TurretExp;
        audiosource.Play();

        Destroy(go, TurretExp.length);
    }
    public void SFXPlay10(string sfxName, AudioClip FragExp)//¼ö·ùÅº ÆøÆÄ
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = FragExp;
        audiosource.Play();

        Destroy(go, FragExp.length);
    }


}
