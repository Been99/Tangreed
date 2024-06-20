
public interface IInteractable
{
    public string GetItemNamePrompt(); // 출력될 아이템의 이름
    public string GetItemDescriptionPrompt(); // 출력될 아이템의 설명

    public ItemSO GetItemData(); // ItemSO 반환
}
