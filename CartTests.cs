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

    [Test]
    public async Task RemoveItemFromCart_CartEmpty()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        var inventoryPage = new InventoryPage(Page);
        var cartPage = new CartPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("standard_user", "secret_sauce");
        await inventoryPage.AddItemAsync("add-to-cart-sauce-labs-bike-light");
        await inventoryPage.GoToCartAsync();
        await cartPage.RemoveItemFromCartAsync("remove-sauce-labs-bike-light");

        //Assert
        await Expect(Page.Locator("[data-test='inventory-item']")).ToHaveCountAsync(0);
    }


    [Test]
    public async Task RemoveOneItemFromCart_OtherItemsUnaffected()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        var inventoryPage = new InventoryPage(Page);
        var cartPage = new CartPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("standard_user", "secret_sauce");
        await inventoryPage.AddItemAsync("add-to-cart-sauce-labs-bike-light");
        await inventoryPage.AddItemAsync("add-to-cart-sauce-labs-backpack");
        await inventoryPage.GoToCartAsync();
        await cartPage.RemoveItemFromCartAsync("remove-sauce-labs-bike-light");

        //Assert
        await Expect(Page.Locator("[data-test='inventory-item']")).ToHaveCountAsync(1);
        await Expect(Page.Locator("[data-test='inventory-item-name']")).ToContainTextAsync("Sauce Labs Backpack");
    }

    [Test]
    public async Task AddTwoItemsToCart_CartBadgeShowsTwo()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        var inventoryPage = new InventoryPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("standard_user", "secret_sauce");
        await inventoryPage.AddItemAsync("add-to-cart-sauce-labs-bike-light");
        await inventoryPage.AddItemAsync("add-to-cart-sauce-labs-backpack");

        //Assert
        await Expect(Page.Locator("[data-test='shopping-cart-badge']")).ToContainTextAsync("2");
    }

    [Test]
    public async Task SortItems_ShowsItemsLowToHighPrice()
    {
        //Arrange
        var loginPage = new LoginPage(Page);
        var inventoryPage = new InventoryPage(Page);
        var cartPage = new CartPage(Page);
        await loginPage.GotoAsync();

        //Act
        await loginPage.LoginAsync("standard_user", "secret_sauce");
        await inventoryPage.SortItems("lohi");

        //Assert
        var priceString = await Page.Locator("[data-test='inventory-item-price']").AllTextContentsAsync();

        var prices = priceString.Select(p => decimal.Parse(p.Replace("$", ""))).ToList();

        for (int i = 0; i < (prices.Count - 1); i++)
        {
            Assert.That(prices[i] <= prices[i+1]);
        }
    }
}