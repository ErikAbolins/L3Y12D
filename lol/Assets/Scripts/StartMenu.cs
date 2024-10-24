using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
  public string strSceneName;
  
  

   public void StartGame()
   {
     SceneManager.LoadScene(strSceneName);
   }
}
