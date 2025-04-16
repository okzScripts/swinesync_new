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
            //SlowMo = 3000
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
        await _page.GotoAsync("http://localhost:5173");
        var title = await _page.TitleAsync();
        Assert.AreEqual("SWINE_SYNC INC.", title);
    }

    public async Task GivenThatIAmOnTheMainPage()
    {
        await _page.GotoAsync("http://localhost:5173");
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

}