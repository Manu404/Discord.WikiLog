# WikiLog

[![Build status](https://ci.appveyor.com/api/projects/status/5n6fifahr986mj8c/branch/main?svg=true)](https://ci.appveyor.com/project/Manu404/wikidiscordnotifier/branch/main)



A tool based on discord webhook to publish message related to MediaWiki changes to discord channels. Actually triggered by cronjob to monitor wiki.

Maybe future plan to make it usable by anyone. 

It can be used for now by any MediaWiki as the webhook url and wiki url are given as parameter, but the UI is in French and some part of the link URLS like for page and user use French words, a localization effort would be required to at least support english.

Maybe in the future, stay tuned !

A more in depth document might be coming in the future about deployment and configuration.

Basic usage:

```
politilog --wiki <wiki_url> --webhook <discord_webhook_url> 
```

#### Parameter reference

|      |           | Flag | Required | Description                                                  |
| ---- | --------- | :--: | :------: | ------------------------------------------------------------ |
|      | --wiki    |      |    x     | The url of the wiki                                          |
|      | --webhook |      |    x     | The url of the discord webhook                               |
| -l   | --limit   |      |          | Number of changes requested by queries. Max 500 (MediaWiki api limit) |
| -n   | --no-log  |  x   |          | Don't output to logfile                                      |
| -s   | --silent  |  x   |          | Don't output to console                                      |


