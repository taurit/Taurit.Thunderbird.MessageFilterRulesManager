# Taurit.Thunderbird.MessageFilterRulesManager

This is a simple tool to manage Thunderbird's list of filtering rules.

Thunderbirds stores the filtering rules in a file called *msgFilterRules.dat*. It's a text file so it can be easily parsed and modified outside of the application.

I want to manage a large list of rules because I use them to filter out irrelevant stuff from the of RSS feeds I subscribe. It turned out that to do this efficiently I might need hundreds of them. Conceptually my list of rules might look like:

```
Filtering rule: filter out news about hyped technologies.

Conditions:

* subject contains 'blockchain'
* content contains 'blockchain'

* subject contains ' IoT '
* subject starts with 'IoT '
* subject ends with ' IoT'
* subject contains ' IoT,'
* subject contains ' IoT.'
* subject contains ' IoT?'
* subject contains ' IoT!'

...
```

Managing those filtering rules in Thunderbird's GUI is a pain, and I would like to use Excel instead to quickly add new rules, eg. in a file like this:

```
[Text]       [Search also in content]   [Only whole words]   [Category]
blockchain   yes                        no                   hype
iot          no                         yes                  hype
```

Then such excel file is automatically converted to Thunderbird's native format by the tool.