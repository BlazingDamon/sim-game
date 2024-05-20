using Main.Items.Food.Base;

namespace Main.Items.Food;
internal class FarmedFoodItem : FoodItem
{
    public FarmedFoodItem()
    {
        HungerRestored = 25;
    }
}
