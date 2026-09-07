using Microsoft.Playwright;

namespace SauceDemoPlaywright;

public class CartPage
{
    private readonly IPage _page;

    public CartPage(IPage page)
    {
        _page = page;
    }

    public async Task GoToInventoryAsync()
    {
        await _page.Locator("[data-test='continue-shopping']").ClickAsync();
    }

    public async Task RemoveItemFromCartAsync(string removeDataTestId)
    {
        await _page.Locator($"[data-test='{removeDataTestId}']").ClickAsync();
    }

    public async Task GoToCheckoutAsync()
    {
        await _page.Locator("[data-test='checkout']").ClickAsync();
    }
}