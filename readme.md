# Linkmir Code Challenge
This webapi application is a code challenge exercise for Quicken Loans.

## Prerequisites
- Visual Studio Code
- C# for Visual Studio Code
- .Net 5.0 SDK

## Build Instructions
1. From Visual Studio Code, open the integrated terminal
2. Change to the folder that will contain the project folder (ex. C:\projects or /home/user/projects )
3. from terminal window execute: 
```
git clone https://github.com/rhkean/linkmir.git
cd linkmir
code -r linkmir
```
4. When prompted (in the lower right corner) to load missing dependencies, select 'Yes'
5. The project should now compile and run at http://localhost:5000/Zja8syffynb
## Usage
POST:  http://localhost:5000

body:  
```
{
    "link": "http://www.microsoft.com"
}
```
response:
```
{
    "link": "http://www.microsoft.com"
    "shortlink": "http://localhost:5000/aazr35wn82b"
}
```
---
GET:  http://localhost:5000/aazr35wn82b

response:
```
{
    "link": "http://www.microsoft.com/",
    "shortLink": "http://localhost:5000/aazr35wn82b"
}
```
---
GET: http://localhost:5000/stats/aazr35wn82b

response:
```
{
    "domain": "microsoft.com",
    "subdomain": "www",
    "submissionCount": 1,
    "accessCount": 2,
    "link": "http://www.microsoft.com/",
    "shortLink": "http://localhost:5000/aazr35wn82b"
}
```
---
GET: http://localhost:5000/stats

body:
```
{
  "subdomain": "*",
  "domain": "microsoft.com"
}
```
response:
```
{
    "domain": "microsoft.com",
    "subDomain": "*",
    "links": [
        {
            "domain": "microsoft.com",
            "subdomain": "www",
            "submissionCount": 1,
            "accessCount": 2,
            "link": "http://www.microsoft.com/",
            "shortLink": "aazr35wn82b"
        }
    ],
    "matchingLinksCount": 1,
    "totalSubmissionCount": 1,
    "totalAccessCount": 2
}
```
---
---
### **Disclaimer**
This is my first Visual Studio Code for LInux application written in a ChromeOS chroot environment.  I have extensive previous experience with Visual Studio, however my chromebook is my only current build machine.  While not the best or most efficient way to demonstrate my coding skills, I understand that this exercise is more focused on seeing **how** I think through the coding process.

With that said, I have begun this webapi utilizing a [Microsoft Tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio-code "Tutorial: Create a web API with ASP.NET Core") to build from.  I believe this provided a quick start for success in this situation, however this may result in remnants of the sample project from the tutorial due to time constraints.