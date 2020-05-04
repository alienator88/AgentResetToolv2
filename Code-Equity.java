package com.equity.elevate;

import org.openqa.selenium.*;
import org.openqa.selenium.chrome.ChromeDriver;
import java.util.Calendar;
import java.util.Collections;
import java.util.Date;
import java.util.List;
import java.util.concurrent.TimeUnit;

public class ResetAgentPassword {

    WebDriver driver;

    public void setup(){

            String os = System.getProperty("os.name").toLowerCase();
            if (os.contains("mac")) {
                System.setProperty("webdriver.chrome.driver", System.getProperty("user.dir")+ "/chromedriver");
            }else {
                System.setProperty("webdriver.chrome.driver", System.getProperty("user.dir")+"\\chromedriver.exe");
            }

            driver = new ChromeDriver();
            driver.manage().deleteAllCookies();
            driver.manage().window().maximize();
            driver.manage().timeouts().implicitlyWait(30, TimeUnit.SECONDS);
            driver.manage().timeouts().pageLoadTimeout(30, TimeUnit.SECONDS);

            driver.get("https://www.equityelevate.com");


    }

    public void Reset() throws InterruptedException {

        //Declarations
        JavascriptExecutor js = (JavascriptExecutor) driver;
        Date date = new Date();
        Calendar cal = Calendar.getInstance();
        cal.setTime(date);
        int year = cal.get(Calendar.YEAR);

        //Login
        driver.findElement(By.id("Email")).sendKeys("brittnibasso@gmail.com");
        driver.findElement(By.id("Password")).sendKeys("Equity" + year + "!");

        //Select user and navigate to roster
        driver.findElement(By.className("button-title-h")).click();
        driver.findElement(By.className("button-title-h")).click();
        driver.get("https://equityelevate.com/home/mybusinessroster");

        //Get name of agent and search for it
        js.executeScript("window.promptResponse=prompt('Enter the agent name')");
        isAlertPresent(driver);
        String agentname = (String) js.executeScript("return window.promptResponse");
        driver.manage().timeouts().implicitlyWait(20, TimeUnit.SECONDS);
        driver.findElement(By.name("searchname")).sendKeys(agentname);
        driver.findElement(By.id("searchsubmit")).click();
        Thread.sleep(500);

        //Count number of results. If more than 1, stop reset process
        List<WebElement> agents = driver.findElements(By.xpath("//*[@id=\"exitingform\"]/div/div[2]"));
        if (agents.size() > 1) {} //Do nothing if greater than 1. Empty {}'s
        else {
            //Select user from list div[1] for first user in list or div[2] for second user in list
            driver.findElement(By.xpath("//*[@id=\"exitingform\"]/div[1]/div[2]/a")).click();
            Thread.sleep(500);

            //Change password button
            driver.findElement(By.xpath("//*[@id=\"personal-info-form\"]/div[3]/div/div/div/button")).click();

            //Enter password, confirm and save
            driver.findElement(By.id("Password1")).sendKeys("Equity" + year + "!");
            Thread.sleep(500);
            driver.findElement(By.id("Password2")).sendKeys("Equity" + year + "!");
            driver.findElement(By.id("chgPasswordSubmit")).click();
            Thread.sleep(500);
            driver.findElement(By.xpath("/html/body/div[2]/div/div[3]/button[1]")).click();
            Thread.sleep(500);
            driver.quit();
        }

    }

    public void isAlertPresent(WebDriver driver) {
        try
        {
            driver.switchTo().alert();
            driver.manage().timeouts().implicitlyWait(20, TimeUnit.SECONDS);  // even though not needed
            isAlertPresent(driver);
        }
        catch (NoAlertPresentException Ex) {}
        }

    public static void main(String[] args) throws InterruptedException {
        ResetAgentPassword rap = new ResetAgentPassword(); //Initiate ResetAgentPassword as new object
        rap.setup(); //Configure browser and URL for task
        rap.Reset(); //Run reset task
        }
}
