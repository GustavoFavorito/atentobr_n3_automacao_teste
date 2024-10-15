using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using static System.Net.WebRequestMethods;

namespace atentobr_n3_automacao_teste
{
    [TestFixture]
    public class TesteRobo
    {
        Robo robo = new();
        String testUrl = "https://www.4devs.com.br/gerador_de_pessoas";
        IWebDriver driver;

        [SetUp]
        public void BrowserStartup()
        {
            robo.BrowserInit();
        }

        [Test]
        public void GerarPessoaButton() 
        {
            robo.AccessUrl(testUrl);
            Thread.Sleep(4000);

            driver = robo.GetDriver;

            // IDENTIFICA O BOTÃO DE COOKIES E CLICA EM ACEITAR
            IWebElement cookieButton = driver.FindElement(By.Id("cookiescript_accept"));
            cookieButton.Click();

            Thread.Sleep(1500);

            // IDENTIFICA O CAMPO NUMERICO TXT_QTDE E DEFINE O VALOR MAXIMO (30)
            IWebElement numeroPessoas = driver.FindElement(By.Id("txt_qtde"));
            numeroPessoas.Clear();
            numeroPessoas.SendKeys("30");

            // IDENTIFICA O BOTAO GERAR PESSOA
            IWebElement gerarPessoaButton = driver.FindElement(By.Id("bt_gerar_pessoa"));

            // SCROLLA A PAGINA ATE O BOTAO APARECER EM TELA E CLICA
            Actions actions = new(driver);
            actions.ScrollToElement(gerarPessoaButton);
            actions.Perform();

            gerarPessoaButton.Click();
        }

        [Test]
        public void RetrieveDadosJson()
        {
            robo.AccessUrl(testUrl);
            Thread.Sleep(4000);

            driver = robo.GetDriver;

            // IDENTIFICA O CAMPO DADOS JSON
            IWebElement dadosJson = driver.FindElement(By.Id("dados_json"));

            // SCROLLA A PAGINA ATE O CAMPO APARECER EM TELA
            Actions actions = new(driver);
            actions.ScrollToElement(dadosJson);
            actions.Perform();

            // CLICA O BOTAO COPIAR
            IWebElement copiarButton = driver.FindElement(By.CssSelector("#app-wrapper.person-gen .app-output .button"));
            copiarButton.Click();
        }

        [TearDown]
        public void BrowserClose()
        {
            robo.CloseDriver();
        }
    }
}