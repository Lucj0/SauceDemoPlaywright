using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace SauceDemoPlaywright;

public class CartTests : PageTest
{
    [Test]
    public async Task AddBackpackToCart_CartShowsBackpack()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        var inventoryPage = new InventoryPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("standard_user", "secret_sauce");

        await inventoryPage.AddItemAsync("add-to-cart-sauce-labs-backpack");
        await inventoryPage.GoToCartAsync();

        //Assert
        await Expect(Page.Locator("[data-test='inventory-item-name']")).ToContainTextAsync("Sauce Labs Backpack");
    }

    [Test]
    public async Task CheckoutBackpack_RedirectsToCompletedOrder()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        var inventoryPage = new InventoryPage(Page);
        var cartPage = new CartPage(Page);
        var checkoutInfoPage = new CheckoutInfoPage(Page);
        var checkoutOverviewPage = new CheckoutOverviewPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("standard_user", "secret_sauce");

        await inventoryPage.AddItemAsync("add-to-cart-sauce-labs-backpack");
        await inventoryPage.GoToCartAsync();

        await cartPage.GoToCheckoutAsync();

        await checkoutInfoPage.CheckoutInfoAndContinueAsync("Alvin", "Gwak", "123456");

        await checkoutOverviewPage.FinishCheckoutAsync();

        //Assert
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/checkout-complete.html");
        await Expect(Page.Locator("[data-test='complete-text']")).ToBeVisibleAsync();
    }
}