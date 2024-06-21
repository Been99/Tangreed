using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 던전을 생성할 때 맵이 생성될 자리를 미리 배치
// 자신의 인덱스 번호와 연결된 방들의 정보를 가지고 있음.
public class StageData : MonoBehaviour
{
    public MAPTYPE type;
    public StageData RightMap = null;
    public StageData LeftMap = null;
    public StageData UpMap = null;
    public StageData DownMap = null;

    public int Num;
    public int indexX;
    public int indexY;

    public void InitSetting(int num, int x, int y, MAPTYPE type)
    {
        this.Num = num;
        this.indexX = x;
        this.indexY = y;
        this.type = type;
    }

    public void InitSetting(int num, int x, int y)
    {
        this.Num = num;
        this.indexX = x;
        this.indexY = y;
    }
}
