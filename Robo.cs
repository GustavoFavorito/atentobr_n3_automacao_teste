using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using SuperConvert.Extensions;
using System.Data;
using System.IO;
using NUnit.Framework.Internal;

namespace atentobr_n3_automacao_teste
{
    internal class Robo
    {
        IWebDriver driver;
        private string url = "https://www.4devs.com.br/gerador_de_pessoas";

        public void BrowserInit()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        public void AccessUrl(string url)
        {
            driver.Url = url;
        }

        public void CloseDriver()
        {
            driver.Close();
            driver.Quit();
        }

        public IWebDriver GetDriver
        {
            get { return driver; }
        }

        public void ProcessaRobo()
        {
            BrowserInit();
            AccessUrl(url);
            AlteraQtdePessoas("30");
            Thread.Sleep(1000);
            ClicaAceitaCookie();
            Thread.Sleep(1000);
            ClicaGerarPessoa();
            Thread.Sleep(1000);
            DownloadJsonObject();
            CreateFile();
            CloseDriver();
        }

        private static string ConvertJsonToCsv(string file, string path, string fileName)
        {
            return file.ToCsv(path, fileName);
        }

        private void CreateFile()
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\data\\";
            string pathDownload = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\";
            string file = "data.json";
            string text = SaveJsonObject();

            // converte arquivo gerado na pasta data
            ConvertFile(path, file, text);
            // converte download
            ConvertFile(pathDownload, file, text);
        }

        private static void ConvertFile(string path, string file, string text)
        {
            try
            {
                using FileStream fs = File.Create(path + file);
                byte[] info = new UTF8Encoding(true).GetBytes(text);
                fs.Write(info, 0, info.Length);
                ConvertJsonToCsv(text, path, "data");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private string SaveJsonObject()
        {
            return FindIWebElement("dados_json")?.GetAttribute("value");
        }

        private void DownloadJsonObject()
        {
            FindIWebElement("button.button.button--download.output-btn", "cssSelector")?.Click();
        }

        private IWebElement FindIWebElement(string idElemento, string identificador = "id")
        {
            IWebElement elemento;

            if (identificador == "cssSelector")
            {
                elemento = driver.FindElement(By.CssSelector(idElemento));
            } else
            {
                elemento = driver.FindElement(By.Id(idElemento));
            }

            return elemento;
        }

        private void ClicaAceitaCookie()
        {
            FindIWebElement("cookiescript_accept")?.Click();
        }

        private void ClicaGerarPessoa()
        {
            IWebElement elemento = FindIWebElement("bt_gerar_pessoa");

            new Actions (driver)
                .ScrollToElement(elemento)
                .Perform();

            elemento.Click();
        }

        private void AlteraQtdePessoas(string qtde)
        {
            IWebElement elemento = FindIWebElement("txt_qtde");
            elemento.Clear();
            elemento.SendKeys(qtde);
        }
    }
}
