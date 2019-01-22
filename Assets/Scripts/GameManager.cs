using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance=null;
    private void Awake() {
        if(_instance==null)_instance=this;
    }
    #endregion

    public int _maxEnemiesNumber=5;
    public static int MaxEnemiesNumber{
        get{
            return _instance._maxEnemiesNumber;
        }
        set{
            _instance._maxEnemiesNumber=value;
        }
    }

}
