using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// в классе храняться все постоянные данные
/// </summary>
public class Data
{

    private static Data _instance;

    public static Data Instance
    {
        get
        {
            return _instance ?? (_instance = new Data());
        }
    }


    #region FieldsForSave

    private string _bestScore = "BEST_SCORE";
    private string _premiumAcc = "PREMIUM_ACCAUNT";
    private string _godMode = "GOD_MOD";
    private string _soundValue = "SOUND";
    private string _musicValue = "MUSIC";

    #endregion


    private int _score;
    public int BestScore
    {
        get
        {
            if (!PlayerPrefs.HasKey(_bestScore))
            {
                PlayerPrefs.SetInt(_bestScore, 0);
            }

            _score = PlayerPrefs.GetInt(_bestScore);
            return _score;
        }

        set
        {
            if (value > BestScore)
            {
                _score = value;
                PlayerPrefs.SetInt(_bestScore, _score);
            }
        }
    }

    private int _music;
    public bool Music
    {
        get
        {
            if (!PlayerPrefs.HasKey(_musicValue))
            {
                PlayerPrefs.SetInt(_musicValue, 1);
            }

            _music = PlayerPrefs.GetInt(_musicValue);
            return _music == 1;
        }

        set
        {
            _music = value ? 1 : 0;
            PlayerPrefs.SetInt(_musicValue, _music);
            EventManager.TriggerEvent("ChangeMusic", null);
        }
    }



    private int _sound;
    public bool Sound
    {
        get
        {
            if (!PlayerPrefs.HasKey(_soundValue))
            {
                PlayerPrefs.SetInt(_soundValue, 1);
            }

            _sound = PlayerPrefs.GetInt(_soundValue);
            return _sound == 1;
        }

        set
        {
            _sound = value ? 1 : 0;
            PlayerPrefs.SetInt(_soundValue, _sound);
            EventManager.TriggerEvent("ChangeSound", null);
        }
    }


    private int _mode;
    public bool GODMODE
    {
        get
        {
            if (!PlayerPrefs.HasKey(_godMode))
            {
                PlayerPrefs.SetInt(_godMode, 0);
            }

            _mode = PlayerPrefs.GetInt(_godMode);
            return _mode == 1;
        }

        set
        {
            _mode = value ? 1 : 0;
            PlayerPrefs.SetInt(_godMode, _mode);
        }
    }


    private int _premAcc;
    public bool PremiumAccautn
    {
        get
        {
            if (!PlayerPrefs.HasKey(_premiumAcc))
            {
                PlayerPrefs.SetInt(_premiumAcc, 0);
            }

            _mode = PlayerPrefs.GetInt(_godMode);
            return _mode == 1;
        }

        set
        {
            _mode = value ? 1 : 0;
            PlayerPrefs.SetInt(_godMode, _mode);
        }
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }
}
