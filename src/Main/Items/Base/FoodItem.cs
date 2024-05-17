namespace Main;
internal abstract class FoodItem : Item
{
    public int HungerRestored;

    public FoodItem()
    {
        Type = ItemType.Food;
    }
}
