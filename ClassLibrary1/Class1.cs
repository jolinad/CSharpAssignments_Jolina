using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Assignment1
{
    class Program
    {
        public static IWebDriver driver;



        [SetUp]
        public void SetUp()
        {

            driver = new ChromeDriver();
            driver.Url = "http://www.automation.com/";
            driver.Manage().Window.Maximize();
        }

        [Test]
        //Use relative XPaths for object identification
        public void Industries()
        {
            driver.FindElement(By.XPath("//span[contains(text(),'Industries')]")).Click();
            driver.FindElement(By.XPath("//ul[@class='sub-nav']//a[contains(text(),'Building Automation')]")).Click();
            driver.FindElement(By.XPath("//ul[@class='sub-nav']//a[contains(text(),'Building Automation e-Newsletter Archive')]")).Click();

            String currentURL = driver.Url;
            Assert.AreEqual("https://www.automation.com/portals/industries/building-automation/building-automation-e-newsletter-archive", currentURL);

            //Fetching and printing the list of titles in Archived News Letters
            IWebElement columnList = driver.FindElement(By.XPath("//*[@id='content']/div/div[4]/ul"));
            IList<IWebElement> links = columnList.FindElements(By.TagName("li"));
            Console.WriteLine("The total number of links:" + links.Count);
            String[] allLinks = new String[links.Count];
            int i = 0;
            foreach (IWebElement listElement in links)
            {
                Console.WriteLine(listElement.Text);
                allLinks[i++] = listElement.Text;
            }

        }
        [Test]
        //Use CSS for locators. Search key & Category will be passed as parameter to method.
        public void TestOnProducts()
        {
            driver.FindElement(By.CssSelector("li.newnav:nth-child(3)")).Click();

            driver.FindElement(By.CssSelector("#field_707")).SendKeys("Weidmuller");
            driver.FindElement(By.CssSelector("div.search-button > a:nth-child(1)")).Click();

            IWebElement containerTitle = driver.FindElement(By.CssSelector("div#pagination-container"));
            IList<IWebElement> titles = containerTitle.FindElements(By.CssSelector("div.info-block div.text-holder a"));

            //Fetching all the titles and veriying it has the search key 
            String[] allTitles = new String[titles.Count];
            int i = 0;
            foreach (IWebElement listTitle in titles)
            {
                Console.WriteLine(listTitle.Text);
                Assert.That(listTitle.Text, Does.Contain("Weidmuller"));
                allTitles[i++] = listTitle.Text;
            }

            //Navigating back to previos page 
            driver.Navigate().Back();
            driver.FindElement(By.CssSelector("span.selection > span.select2-selection.select2-selection--multiple")).Click();

            //Fetching results for category search 
            List<IWebElement> listelement = driver.FindElements(By.CssSelector(".select2-results__options li")).ToList();
            IWebElement secondElement = listelement.First(condition => condition.Text.Equals("Actuators / Digital Actuators"));
            secondElement.Click();
            IWebElement searchKey1 = driver.FindElement(By.CssSelector("#field_707"));
            searchKey1.Clear();
            IWebElement clickButton = driver.FindElement(By.CssSelector("div.search-button > a:nth-child(1)"));
            clickButton.Click();

            //Verifying product category  for at least for 1 item to be Category  selected
            IWebElement categoryContainer = driver.FindElement(By.CssSelector("#pagination-container"));
            IList<IWebElement> categoryTitle = categoryContainer.FindElements(By.CssSelector(".info-block .text-holder"));
            String[] allCategory = new String[categoryTitle.Count];
            int j = 0;
            foreach (IWebElement categoryitem in categoryTitle)
            {
                Console.WriteLine(categoryitem.Text);
                Assert.That(categoryitem.Text, Does.Contain("actuators"));
                allCategory[j++] = categoryitem.Text;
            }

            //Clicking on the fitst hyperlink and verifying the title
            IWebElement containerTitle1 = driver.FindElement(By.CssSelector("div#pagination-container"));
            List<IWebElement> categorytitles = containerTitle1.FindElements(By.CssSelector("div.info-block div.text-holder a")).ToList();
            IWebElement veryFirstElement = categorytitles.First();
            veryFirstElement.Click();
            String browserUrl = driver.Url;
            Assert.That(browserUrl, Does.Contain("diakont-electric-actuators-improve-auto-manufacturing-economy-uptime-and-product-quality"));
            driver.Navigate().Back();

            //Navigating back to previous page and verify the first item earlier is still displayed as first item
            IWebElement containerPrevious = driver.FindElement(By.CssSelector("div#pagination-container"));
            List<IWebElement> previousList = containerPrevious.FindElements(By.CssSelector("div.info-block div.text-holder a")).ToList();
            IWebElement firstElement = previousList.First();
            Assert.AreEqual(firstElement.Text, "Diakont Electric Actuators Improve Auto Manufacturing Economy, Uptime, and Product Quality");
        }

        [Test]
        public void TestOnJobs()
        {
            Actions actions = new Actions(driver);
            IWebElement target = driver.FindElement(By.CssSelector("li.newnav.right:nth-child(4)"));
            actions.MoveToElement(target).Perform();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            //Clicking on Salary survey results 
            IWebElement salarySurveyResult2018 = driver.FindElement(By.XPath("//div[@id='header']//li[4]//div[1]//div[1]//li[3]"));
            salarySurveyResult2018.Click();

            GetRegion("United States");
            RegionOfUS("Pacific (West)");
        }

        //Method to get the avg salary of respective regions 
        public static void GetRegion(string region)
        {
            switch (region)
            {
                case "United States":
                    IWebElement AvgSal1 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(2) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of United State is" + AvgSal1.Text);
                    break;
                case "Canada":
                    IWebElement AvgSal2 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(3) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of Canada is" + AvgSal2.Text);
                    break;
                case "Mexico":
                    IWebElement AvgSal3 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(4) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of Mexico is" + AvgSal3.Text);
                    break;
                case "Central America (including Caribbean)":
                    IWebElement AvgSal4 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(5) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of United State is" + AvgSal4.Text);
                    break;
                case "South America":
                    IWebElement AvgSal5 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(6) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of South America is" + AvgSal5.Text);
                    break;
                case "Europe (Western)":
                    IWebElement AvgSal6 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(7) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of Europe (Western) is" + AvgSal6.Text);
                    break;
                case "Europe (Eastern)":
                    IWebElement AvgSal7 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(8) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of Europe (Eastern) is" + AvgSal7.Text);
                    break;
                case "Africa":
                    IWebElement AvgSal8 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(9) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of Africa is" + AvgSal8.Text);
                    break;
                case "Middle East":
                    IWebElement AvgSal9 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(10) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of Middle East is" + AvgSal9.Text);
                    break;
                case "Australia and New Zealand":
                    IWebElement AvgSal10 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(11) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of Australia and New Zealand is" + AvgSal10.Text);
                    break;
                case "Asia & South Pacific":
                    IWebElement AvgSal11 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(12) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of Asia & South Pacific is" + AvgSal11.Text);
                    break;
                case "South Asia":
                    IWebElement AvgSal12 = driver.FindElement(By.CssSelector("table:nth-child(11) tbody:nth-child(1) tr:nth-child(13) > td:nth-child(2)"));
                    Console.WriteLine("The Average salary of South Asia is" + AvgSal12.Text);
                    break;
            }
        }

        //Method to get the percent respondents of region US 
        public static void RegionOfUS(string regionUS)
        {
            switch (regionUS)
            {
                case "New England (Northeast)":
                    IWebElement precent1 = driver.FindElement(By.CssSelector("table:nth-child(15) tbody:nth-child(1) tr:nth-child(2) > td:nth-child(3)"));
                    Console.WriteLine("The Precent Respondents of New England (Northeast) is" + precent1.Text);
                    break;
                case "Mid-Atlantic (Northeast)":
                    IWebElement precent2 = driver.FindElement(By.CssSelector("table:nth-child(15) tbody:nth-child(1) tr:nth-child(3) > td:nth-child(3)"));
                    Console.WriteLine("The Precent Respondents of Mid-Atlantic (Northeast) is " + precent2.Text);
                    break;
                case "East North Central (Midwest)":
                    IWebElement precent3 = driver.FindElement(By.CssSelector("table:nth-child(15) tbody:nth-child(1) tr:nth-child(4) > td:nth-child(3)"));
                    Console.WriteLine("The Precent Respondents of East North Central (Midwest) is " + precent3.Text);
                    break;
                case "West North Central (Midwest)":
                    IWebElement precent4 = driver.FindElement(By.CssSelector("table:nth-child(15) tbody:nth-child(1) tr:nth-child(5) > td:nth-child(3)"));
                    Console.WriteLine("The Precent Respondents of West North Central (Midwest) is " + precent4.Text);
                    break;
                case "South Atlantic (South)":
                    IWebElement precent5 = driver.FindElement(By.CssSelector("table:nth-child(15) tbody:nth-child(1) tr:nth-child(6) > td:nth-child(3)"));
                    Console.WriteLine("The Precent Respondents of South Atlantic (South) is " + precent5.Text);
                    break;
                case "East South Central (South)":
                    IWebElement precent6 = driver.FindElement(By.CssSelector("table:nth-child(15) tbody:nth-child(1) tr:nth-child(7) > td:nth-child(3)"));
                    Console.WriteLine("The Precent Respondents of East South Central (South) is " + precent6.Text);
                    break;
                case "West South Central (South)":
                    IWebElement precent7 = driver.FindElement(By.CssSelector("table:nth-child(15) tbody:nth-child(1) tr:nth-child(8) > td:nth-child(3)"));
                    Console.WriteLine("The Precent Respondents of West South Central (South) is " + precent7.Text);
                    break;
                case "Mountain (West)":
                    IWebElement precent8 = driver.FindElement(By.CssSelector("table:nth-child(15) tbody:nth-child(1) tr:nth-child(9) > td:nth-child(3)"));
                    Console.WriteLine("The Precent Respondents of Mountain (West) is " + precent8.Text);
                    break;
                case "Pacific (West)":
                    IWebElement precent9 = driver.FindElement(By.CssSelector("table:nth-child(15) tbody:nth-child(1) tr:nth-child(10) > td:nth-child(3)"));
                    Console.WriteLine("The Precent Respondents of Pacific (West) is " + precent9.Text);
                    break;

            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Quit();
        }



    }
}
