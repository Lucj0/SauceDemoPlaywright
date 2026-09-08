using Microsoft.Playwright;

namespace SauceDemoPlaywright;

public class InventoryPage
{
    private readonly IPage _page;

    public InventoryPage(IPage page)
    {
        _page = page;
    }
    public async Task AddItemAsync(string dataTestId)
    {
        await _page.Locator($"[data-test='{dataTestId}']").ClickAsync();
    }

    public async Task GoToCartAsync()
    {
        await _page.Locator("[data-test='shopping-cart-link']").ClickAsync();
    }

    public async Task SortItems(string sortMethod)
    {
        await _page.Locator("[data-test='product-sort-container']").SelectOptionAsync($"{sortMethod}");
    }
}