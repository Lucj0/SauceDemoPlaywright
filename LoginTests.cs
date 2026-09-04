using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace SauceDemoPlaywright;

public class LoginTest : PageTest
{
    [Test]
    public async Task ValidLogin_RedirectsToProducts()
    {
        //Arrange
        await Page.GotoAsync("https://www.saucedemo.com");

        //Act
        await Page.Locator("[data-test='username']").FillAsync("standard_user");
        await Page.Locator("[data-test='password']").FillAsync("secret_sauce");
        await Page.Locator("[data-test='login-button']").ClickAsync();

        //Assert
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    }

    [Test]
    public async Task InvalidLogin_ShowsError()
    {
        //Arrange
        await Page.GotoAsync("https://www.saucedemo.com");

        //Act
        await Page.Locator("[data-test='username']").FillAsync("wrong_user");
        await Page.Locator("[data-test='password']").FillAsync("wrong_sauce");
        await Page.Locator("[data-test='login-button']").ClickAsync();

        //Assert
        await Expect(Page.Locator("[data-test='error']")).ToBeVisibleAsync();
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com");
    }

    [Test]
    public async Task AddBackpackToCart_CartShowsBackpack()
    {
        //Arrange
        await Page.GotoAsync("https://www.saucedemo.com");

        //Act
        await Page.Locator("[data-test='username']").FillAsync("standard_user");
        await Page.Locator("[data-test='password']").FillAsync("secret_sauce");
        await Page.Locator("[data-test='login-button']").ClickAsync();

        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator("[data-test='shopping-cart-link']").ClickAsync();

        //Assert
        await Expect(Page.Locator("[data-test='inventory-item-name']")).ToContainTextAsync("Sauce Labs Backpack");
    }
}