//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class Grass : MonoBehaviour, IInteractable
//{
//    public Tilemap tilemap;
//    public TileBase grassTile; // 타일맵 잔디
//    public TileBase diggedTile; // 파인 땅
//    public Equipment equipment;

//    private void Start()
//    {
//        tilemap = GetComponent<Tilemap>();
//    }

//    public string GetInteractPrompt() // 오브젝트가 감지됐을때 뜨는 프롬프트 문구
//    {
//        if (equipment.curEquipTool != null && equipment.curEquipTool.name == "Shovel")
//        {
//            return "삽을 사용하여 땅을 파세요";
//        }
//        else
//        {
//            return "도구를 장착하세요";
//        }
//    }

//    public void OnInteract() // 인터랙트 됐을 때 일어나는 동작
//    {
//        if (equipment.curEquipTool != null && equipment.curEquipTool.name == "Shovel")
//        {
//            Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
//            tilemap.SetTile(cellPosition, diggedTile);
//        }
//    }
//}
