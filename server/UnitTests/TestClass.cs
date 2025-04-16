using Xunit;
using server;

public class TicketRoutesTests
{
    [Fact]
    public void GenerateSlugs_ReturnsCorrectLength()
    {

        int expectedLength = 10;


        string slug = TicketRoutes.GenerateSlugs(expectedLength);


        Assert.Equal(expectedLength, slug.Length);
    }

    [Fact]
    public void GenerateSlugs_ContainsOnlyValidCharacters()
    {

        int length = 20;
        string allowed = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789";


        string slug = TicketRoutes.GenerateSlugs(length);


        Assert.All(slug, c => Assert.Contains(c, allowed));
    }

    [Fact]

    public void GenerateSlugsDefferentResults()
    {
        int length = 22;

        string slug1 = TicketRoutes.GenerateSlugs(length);
        string slug2 = TicketRoutes.GenerateSlugs(length);

        Assert.NotEqual(slug1, slug2);
    }

    [Fact]
    public void GeneratePasswordByLength()
    {

        int length = 15;

        string pw = UserRoutes.GeneratePassword(length);

        Assert.Equal(length, pw.Length);
    }

    [Fact]
    public void GeneratePasswordNotEquals()
    {
        int length = 14;

        string pw1 = UserRoutes.GeneratePassword(length);
        string pw2 = UserRoutes.GeneratePassword(length);

        Assert.NotEqual(pw1, pw2);
    }
}