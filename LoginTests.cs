using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace SauceDemoPlaywright;

public class LoginTest : PageTest
{
    [Test]
    public async Task ValidLogin_RedirectsToProducts()
    {
        await Page.GotoAsync("https://www.saucedemo.com");

        await Page.Locator("[data-test='username']").FillAsync("standard_user");
        await Page.Locator("[data-test='password']").FillAsync("secret_sauce");
        await Page.Locator("[data-test='login-button']").ClickAsync();

        //Assert
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    }
}