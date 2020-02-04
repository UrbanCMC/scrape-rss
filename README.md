# ScrapeRSS

## What?
If you use RSS feeds but have found some websites you use don't provide one, you may be able to generate one by using this web app to scrape their website.  

Simply run this app and navigate to the website it starts up (default http://localhost:5000/).  
From there you'll be able to generate a link you can subscribe to using your preferred RSS reader that converts the scraped content into an RSS feed.

## Why?
RSS feeds provide a simple way of staying up-to-date on any changes to a website.  
But not every website provides an RSS feed, which is where this app comes into play.  

There are obviously other tools/websites providing a similar service, such as the [Feed Creator by FiveFilters.org](http://createfeed.fivefilters.org/), which is where the inspiration for doing this came from. But using these services also means that all traffic is going through their servers, meaning they're theoretically able to monitor/log what people are looking at.  

If this bothers you as it does me, you can use this web app to keep that information on your local machine/network.

## Features
* Generate API links that will return the scraped data as RSS feed.
* Every aspect of the links can be set through the 'Link Builder' page, so there's no need to manually modify the URL parameters directly.
* Paste already built links back into the 'Link Builder', so that they can be modified without re-entering everything.

## ToDo
* Implement preview page, so it's easier to quickly check whether the generated link is correct.
* Persist state between page changes. Switching back to the 'Link Builder' shouldn't clear it.
