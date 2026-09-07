using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace SauceDemoPlaywright;

public class LoginTest : PageTest
{
    [Test]
    public async Task ValidLogin_RedirectsToProducts()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("standard_user", "secret_sauce");

        //Assert
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    }

    [Test]
    public async Task InvalidLogin_ShowsError()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("wrong_user", "wrong_sauce");

        //Assert
        await Expect(Page.Locator("[data-test='error']")).ToBeVisibleAsync();
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com");
    }

    [Test]
    public async Task LockedOutUserLogin_ConfirmsLockedOut()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("locked_out_user", "secret_sauce");

        //Assert
        await Expect(Page.Locator("[data-test='error']")).ToBeVisibleAsync();
        await Expect(Page.Locator("[data-test='error']")).ToContainTextAsync("Epic sadface: Sorry, this user has been locked out.");
    }

    [Test]
    public async Task ErrorUserLogin_CheckoutInfoLastNameError()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        var inventoryPage = new InventoryPage(Page);
        var cartPage = new CartPage(Page);
        var checkoutInfoPage = new CheckoutInfoPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("error_user", "secret_sauce");
        await inventoryPage.GoToCartAsync();
        await cartPage.GoToCheckoutAsync();
        await checkoutInfoPage.CheckoutInfoAsync("Alvin", "Gwak", "12345");

        //Assert
        await Expect(Page.Locator("[data-test='lastName']")).ToHaveValueAsync("");
    }

    [Test]
    public async Task ErrorUserLogin_CheckoutOverviewFinishError()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        var inventoryPage = new InventoryPage(Page);
        var cartPage = new CartPage(Page);
        var checkoutInfoPage = new CheckoutInfoPage(Page);
        var checkoutOverviewPage = new CheckoutOverviewPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("error_user", "secret_sauce");
        await inventoryPage.GoToCartAsync();
        await cartPage.GoToCheckoutAsync();
        await checkoutInfoPage.CheckoutInfoAndContinueAsync("Alvin", "Gwak", "12345");
        await checkoutOverviewPage.FinishCheckoutAsync();

        //Assert
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/checkout-step-two.html");
    }
}