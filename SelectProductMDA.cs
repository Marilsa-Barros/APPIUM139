

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.MultiTouch;

namespace Appium;

public class SelectProductMDA
{
    // Atributos
    public static string SAUCE_USERNAME = Environment.GetEnvironmentVariable("SAUCE_USERNAME");
    public static string SAUCE_ACCESS_KEY = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY");
    Uri URI = new Uri($"https://{SAUCE_USERNAME}:{SAUCE_ACCESS_KEY}@ondemand.us-west-1.saucelabs.com:443/wd/hub");

    public AndroidDriver<AndroidElement> driver { get; set; } // declara o objeto do Appium para leitura e gravação

    // Funções e Metodos

    [SetUp] // Inicializa - Antes do teste
    public void MobileBaseSetup() // Configurações de Inicialização para o Mobile
    {
        var options = new AppiumOptions(); // objeto que vai reunir as configurações
        options.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
        options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "9.0");
        options.AddAdditionalCapability(MobileCapabilityType.DeviceName, "Samsung Galaxy S9 FHD GoogleAPI Emulator");
        options.AddAdditionalCapability(MobileCapabilityType.App, "storage:filename=mda-2.0.0-21.apk");
        options.AddAdditionalCapability("appPackage", "com.saucelabs.mydemoapp.android");
        options.AddAdditionalCapability("appActivity", "com.saucelabs.mydemoapp.android.view.activities.SplashActivity");
        options.AddAdditionalCapability("newCommandTimeout", 90); // espera os elementos por 90s

        // Inicializa o Appium como Android
        driver = new AndroidDriver<AndroidElement>(remoteAddress: URI, driverOptions: options, commandTimeout: TimeSpan.FromSeconds(180));
    }

    [TearDown] // Encerra - depois do teste
    public void Finalizar()
    {
        if (driver == null) return; // Se não tem o driver ativo, apenas termina o script
        driver.Quit();
    }

    [Test] // Teste em si
    public void SelecionarProdutoMDA()
    {
        // É importante começar com uma linha que aguarda carregar algum elemento da página inicial
        Assert.That(driver.FindElement(MobileBy.AccessibilityId("App logo and name")).Displayed, Is.True);

        // Clicar no produto mochila
        driver.FindElement(MobileBy.AccessibilityId("Sauce Labs Backpack")).Click();

        // verificar o nome do produto na tela do produto
        String tituloProduto = driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/cartIV")).Text;
        Assert.That(tituloProduto, Is.EqualTo("Sauce Labs Backpack"));

        // Exemplo com 1 linha
        // Assert.That(driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/cartIV")).Text, Is.EqualTo("Sauce Labs Back Pack"));

        // Verificar o preço do protudo
        String precoProduto = driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/priceTV")).Text;
        Assert.That(precoProduto, Is.EqualTo("$ 29.99"));

        // Arrasta para cima
        TouchAction touchAction = new TouchAction(driver); // instancia objeto para reproduzir gestos
        touchAction.Press(600, 1700);
        touchAction.MoveTo(600, 700);
        touchAction.Release();
        touchAction.Perform();

        // Adicionar no carrinho
        driver.FindElement(MobileBy.AccessibilityId("Tap to add product to cart")).Click();

        // Clicar no carrinho
        driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/cartTV")).Click();

        // Validar o nome do produto no carrinho
        tituloProduto = driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/cartIV")).Text;
        Assert.That(tituloProduto, Is.EqualTo("Sauce Labs Backpack"));

        // Validar o preço do produto no carrinho
        precoProduto = driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/priceTV")).Text;
        Assert.That(precoProduto, Is.EqualTo("$ 29.99"));

    }
}