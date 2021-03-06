﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class KNN : MonoBehaviour
{
    public DataSet UserData; 
    public List<float> distance; 
    public List<DataSet> TrainData; 

    public int kNumber; 

    public float bestLabel; 
    public float bestDistance;

    public float[] kLabelSet; 
    public int countNumber; // 몇번째 거리, 몇번째 트레인데이터 등을 확인하기 위해

    public float[] saveTime; 
    public int[] saveRetry; 
    public int[] saveResult; 

    public float addResult; // 예측값의 평균을 계산하기위해 예측값을 다 담아주는 변수

    public GameData gameData;

    //public Text knnText;

    private void Awake()
    {
        GameManager.Instance.LoadGameDataFromJson();
    }

    void Start()
    {
        TrainData = new List<DataSet>();
        distance = new List<float>();

        kNumber = 3; // 사용자 지정 k

        kLabelSet = new float[kNumber];
        saveTime = new float[kNumber];
        saveRetry = new int[kNumber];
        saveResult = new int[kNumber];

        ReadData(); // 데이터 읽어오는 함수 실행

        KnnAlgorithm();
    }

    public void ReadData()
    {
        TrainData.Clear();
        TextAsset textFile = Resources.Load("Train") as TextAsset; //리소스 폴더안의 트레인 파일을 가져 
        StringReader stringReader = new StringReader(textFile.text); // 파일열기

        while (stringReader != null) //데이터가 존재한다면 계속 읽어오기   
        {
            string line = stringReader.ReadLine(); // 계속 읽기

            if (line == null)
            {
                break;
            }

            //데이터 생성
            DataSet dataSet = new DataSet();
            dataSet.time = float.Parse(line.Split(',')[0]);
            dataSet.retry = int.Parse(line.Split(',')[1]);
            dataSet.result = int.Parse(line.Split(',')[2]);
            TrainData.Add(dataSet); // TrainData에 나눈 항목들하나씩저장해주기
        }
        stringReader.Close(); //파일 닫기
    }

    public void KnnAlgorithm()
    {
        if (GameManager.Instance.gameData.isTutorial == 1)
        {
            CalculateDistance();
            ChangeLabelToKNN();
            MainCanvas.Main.PrintKNN();
            GameManager.Instance.gameData.isTutorial = 0;
            GameManager.Instance.SaveGameDataToJson();
            return;
        }
        else if (GameManager.Instance.gameData.StageNumber > 0 && GameManager.Instance.gameData.StageNumber <= 9)
        {
            CalculateDistance();
            GameManager.Instance.gameData.updatedBestLabelInGame += bestLabel;
            GameManager.Instance.SaveGameDataToJson();
            MainCanvas.Main.PrintKNN();

            if (GameManager.Instance.gameData.StageNumber == 1)
            {
                bestLabel = Mathf.Round(GameManager.Instance.gameData.updatedBestLabelInGame / 9);
                //Debug.Log(GameManager.Instance.gameData.updatedBestLabelInGame / 9);
                //Debug.Log(bestLabel);
                GameManager.Instance.gameData.updatedBestLabelInGame = 0;
                FixBestLabel(bestLabel);
                ChangeLabelToKNN();
                MainCanvas.Main.PrintKNN();
                GameManager.Instance.SaveGameDataToJson();
            }
        }
    }

    public void CalculateDistance()
    {

        UserData.time = GameManager.Instance.gameData.clearTime;
        UserData.retry = GameManager.Instance.gameData.numberOfRenew;

        Debug.Log("time : " + UserData.time);
        Debug.Log("retry : " + UserData.retry);

        distance.Clear();

        for (int i = 0; i < TrainData.Count; i++)
        {
            distance.Add(Mathf.Pow(Twice(TrainData[i].time - UserData.time)
                + Twice((TrainData[i].retry - UserData.retry) * 10), 0.5f)); // 거리계산, 재시작횟수 가중치 높히기 위해 *10

        }

        ChooseData();
    }

    public void ChooseData() // 입력 데이터와 거리가 가까운 K개의 데이터의 레이블을 고르는 함수
    {
        bestDistance = distance[0]; 
        bestLabel = TrainData[0].result; 
        addResult = 0;

        for (int j = 0; j < kNumber; j++) // k만큼 반복문 실행
        {
            for (int i = 0; i < distance.Count - 1; i++) // 데이터만큼 반복문 
            {
                if (bestDistance >= distance[i + 1]) // 현재 저장된 가장 가까운 거리가 새로운 거리보다 크다면 갱신
                {
                    bestDistance = distance[i + 1];
                    bestLabel = TrainData[i + 1].result; // 거리가 가장 가까운 놈의 레이블을 저장
                    countNumber = i + 1; // 몇번쩨 데이터를 저장했는지 알기 위해서 > k만큼 반복을 돌릴때 이전에 결과값은 제외하기 위해                    
                }
            }

            saveTime[j] = TrainData[countNumber].time; //데이터를 데이터 집합에서 삭제하기전 다시 살리기 위해 백업
            saveRetry[j] = TrainData[countNumber].retry;
            saveResult[j] = TrainData[countNumber].result;

            distance.Remove(distance[countNumber]); // 가장 가까운 거리데이터를 제거
            TrainData.Remove(TrainData[countNumber]); // 가장 가까운 거리데이터의 결과를 가져온 트레인데이터를 제거

            kLabelSet[j] = bestLabel; // 찾아낸 최근접 데이터의 레이블을 수집
            addResult += kLabelSet[j]; // 레이블들의 평균을 계산하기 위해 한곳에 계속 더해둠                                
        }

        bestLabel = Mathf.Round(addResult / kNumber); // 최종적으로 입력 데이터의 레이블을 구함

        for (int i = 0; i < kNumber; i++) // k개의 데이터, 레이블을 고르는 과정에서 삭제한 트레인 데이터를 복구시켜줌
        {
            DataSet saveData = new DataSet();
            saveData.time = saveTime[i];
            saveData.retry = saveRetry[i];
            saveData.result = saveResult[i];
            TrainData.Add(saveData);
            //Debug.Log(TrainData.Count);
        }    
 
    }

    public void ChangeLabelToKNN()
    {
        switch (bestLabel)
        {
            case 1:
                GameManager.Instance.gameData.knn = 8;
                break;

            case 2:
                GameManager.Instance.gameData.knn = 6;
                break;

            case 3:
                GameManager.Instance.gameData.knn = 4;
                break;
        }
        //Debug.Log("난이도 선택 결과는?? : " + GameManager.Instance.gameData.knn); // 8 > 어려움, 6 > 중간, 4 > 쉬움
        GameManager.Instance.SaveGameDataToJson();
    }

    public void FixBestLabel(float label)
    {
        if (label == 1 && GameManager.Instance.gameData.knn == 4)
        {
            bestLabel = 2;
            return;
        }
        else if (label == 3 && GameManager.Instance.gameData.knn == 8)
        {
            bestLabel = 2;
            return;
        }
    }

    public float Twice(float a)
    {
        return a * a;
    }
}

[System.Serializable]
public class DataSet // 데이터를 위한 클래스
{
    public float time = 0;
    public int retry = 0;
    public int result = 0;
}