using UnityEngine;

// 모든 던전의 방들의 연결 정보
// 해당 정보에 따라 문들이 활성화되고 캐릭터가 이동가능하게 된다.
public class LinkedData
{
    public GameObject rightMap = null;
    public GameObject leftMap = null;
    public GameObject upMap = null;
    public GameObject downMap = null;


    public int num;
    public int indexX;
    public int indexY;
}