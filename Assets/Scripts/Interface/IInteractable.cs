
public interface IInteractable
{
    public string GetItemNamePrompt(); // 출력될 아이템의 이름
    public string GetItemDescriptionPrompt(); // 출력될 아이템의 설명
    public ItemSO GetItemData(); // ItemSO 반환
    public void GetItemInteract(); // Item정보를 인벤토리로 넘기기 위함
}
