using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace PlaywrightTests;

[TestClass]
public class Test1
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext _browserContext;
    private IPage? _page;

    [TestInitialize]
    public async Task TestSetup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            //SlowMo = 500
        });
        _browserContext = await _browser.NewContextAsync();
        _page = await _browserContext.NewPageAsync();
    }

    [TestCleanup]
    public async Task TestCleanup()
    {
        await _browser.CloseAsync();
    }


    [TestMethod]
    public async Task TestMethod1()
    {
        await _page.GotoAsync("http://localhost:5000");
        var title = await _page.TitleAsync();
        Assert.AreEqual("SWINE_SYNC INC.", title);
    }

    public async Task GivenThatIAmOnTheMainPage()
    {
        await _page.GotoAsync("http://localhost:5000");
    }

    public async Task GivenThatIAmLoggedInAsAdmin()
    {
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("grune@grymt.se");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("hejhej");
        await _page.GetByText("login!").ClickAsync();
    }


    [TestMethod]
    public async Task LogInAsAdmin()
    {
        await GivenThatIAmOnTheMainPage();
        await GivenThatIAmLoggedInAsAdmin();

    }

    [TestMethod]
    public async Task CreateAProductAndBlockItAndThenUnblockIt()
    {
        await LogInAsAdmin();

        await _page.GetByText("Products").ClickAsync();
        await _page.GetByText("Add product").ClickAsync();

        string productName = "TestProduct Swine-Killer 9000";

        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Name:" }).FillAsync(productName);
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Description:" }).FillAsync("Tool for mass slaughter of swines");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Price:" }).FillAsync("1000000");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Category:" }).FillAsync("Equipment");

        await _page.GetByText("Save").ClickAsync();

        await _page.GetByRole(AriaRole.Button, new() { Name = "⬅️ Back" }).ClickAsync();


        var newProductCard = _page.Locator($"li:has-text(\"{productName}\")");
        await newProductCard.GetByRole(AriaRole.Button, new() { Name = "block" }).ClickAsync();

        await _page.GetByText("Show Inactive Products").ClickAsync();

        var inactiveItem = _page.Locator("li.inactive-list-item").Filter(new() { HasTextString = productName });

        await inactiveItem.GetByRole(AriaRole.Button, new() { Name = "Activate " }).ClickAsync();

        await _page.GetByText("Sign Out").ClickAsync();

        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("super_gris@mail.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("kung");
        await _page.GetByText("login!").ClickAsync();

        await _page.GetByText("Admins").ClickAsync();
        await _page.GetByText("Add Admin").ClickAsync();

        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Name:" }).FillAsync("TestAdmin555");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email:" }).FillAsync("testadmin555@testmail.com");

        await _page.SelectOptionAsync("select[name='company']", "3");

        await _page.GetByText("Save").ClickAsync();


    }



}