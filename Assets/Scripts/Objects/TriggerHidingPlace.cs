//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class TriggerHidingPlace : MonoBehaviour
//{
//
//    private GameProcess _stateGame;
//
//    void Start()
//    {
//        _stateGame = GameManager.Instantiate.FindState("GameState") as GameProcess;
//    }
//
//    void OnTriggerEnter(Collider col)
//    {
//        if (col.CompareTag("Stick") && GameManager.Instantiate.GetCurrentState() as GameProcess)
//        {
//            _stateGame.EnterSecretRoom();
//            gameObject.SetActive(false);
//        }
//    }
//}
