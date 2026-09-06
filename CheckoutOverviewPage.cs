using Microsoft.Playwright;

namespace SauceDemoPlaywright;

public class CheckoutOverviewPage
{
    private readonly IPage _page;

    public CheckoutOverviewPage(IPage page)
    {
        _page = page;
    }

    public async Task CancelCheckoutAsync()
    {
        await _page.Locator("[data-test='cancel']").ClickAsync();
    }

    public async Task FinishCheckoutAsync()
    {
        await _page.Locator("[data-test='finish']").ClickAsync();
    }
}