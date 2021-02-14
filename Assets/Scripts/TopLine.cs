using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopLine : MonoBehaviour
{
    public bool IsMove = false;
    public float speed = 0.1f;
    public float limit_y = -5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMove)
        {
            if (this.transform.position.y > limit_y)
            {
                this.transform.Translate(Vector3.down * speed);
            }
            else
            {
                IsMove = false;
                //重新加载游戏
                Invoke("ReloadScene", 1f);
            }
        }
    }

    //碰撞触发
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Contains("Fruit"))
        {
            //判断游戏是否结束
            if ((int)GameManager.gameManagerInstance.gameState < (int)GameState.GameOver)
            {
                if (collider.gameObject.GetComponent<Fruit>().fruitState == FruitState.Collision)
                {
                    //GameOver
                    GameManager.gameManagerInstance.gameState = GameState.GameOver;
                    Invoke("OpenMoveAndCalculateScore", 0.5f);
                    //销毁剩余水果，计算分数

                }
            }

            //计算分数
            if (GameManager.gameManagerInstance.gameState == GameState.CalculateScore)
            {
                float currentScore = collider.GetComponent<Fruit>().fruitScore;
                GameManager.gameManagerInstance.TotalScore += currentScore;
                GameManager.gameManagerInstance.totalScore.text = GameManager.gameManagerInstance.TotalScore.ToString();
                Destroy(collider.gameObject);
            }
        }
    }

    //打开红线向下移动的开关，并且gameState状态变为CalculateScore
    void OpenMoveAndCalculateScore()
    {
        IsMove = true;
        GameManager.gameManagerInstance.gameState = GameState.CalculateScore;
    }

    void CalculateScore()
    {
        //
    }
    
    //
    void ReloadScene()
    {
        //设置历史最高分
        float highestScore = PlayerPrefs.GetFloat("HighestScore");
        if (highestScore < GameManager.gameManagerInstance.TotalScore)
        {
            PlayerPrefs.SetFloat("HighestScore", GameManager.gameManagerInstance.TotalScore);
        }
        SceneManager.LoadScene("SynthesizeBigWatermelon");
    }
}
