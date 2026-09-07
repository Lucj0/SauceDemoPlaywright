using Microsoft.Playwright;

namespace SauceDemoPlaywright;

public class CheckoutInfoPage
{
    private readonly IPage _page;

    public CheckoutInfoPage(IPage page)
    {
        _page = page;
    }

    public async Task CancelCheckoutAsync()
    {
        await _page.Locator("[data-test='cancel']").ClickAsync();
    }

    public async Task CheckoutInfoAndContinueAsync(string firstName, string lastName, string postalCode)
    {
        await _page.Locator("[data-test='firstName']").FillAsync(firstName);
        await _page.Locator("[data-test='lastName']").FillAsync(lastName);
        await _page.Locator("[data-test='postalCode']").FillAsync(postalCode);
        await _page.Locator("[data-test='continue']").ClickAsync();
    }

    public async Task CheckoutInfo(string firstName, string lastName, string postalCode)
    {
        await _page.Locator("[data-test='firstName']").FillAsync(firstName);
        await _page.Locator("[data-test='lastName']").FillAsync(lastName);
        await _page.Locator("[data-test='postalCode']").FillAsync(postalCode);
    }
}