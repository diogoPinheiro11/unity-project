using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private string Scene;

    public void start(){
        SceneManager.LoadScene("Jogo");
    }

    public void restart(){
        SceneManager.LoadScene("Jogo");
    }


   public void exit(){
        Debug.Log("Sair do jogo");
        Application.Quit();
   }
}