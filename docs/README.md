# NeverWinter

Allows fine-grained control over the temperatures, and seasons, on the server.

This mod was commissioned by Rythillian.

## Suport the Mod Author

If you find this mod useful, and you would like to show appreciation for the work I produce; please consider supporting me, and my work, using one of the methods below. Every single expression of support is most appreciated, and makes it easier to produce updates, and new features for my mods, moving fowards. Thank you.

 - [Join my Patreon!](https://www.patreon.com/ApacheTechSolutions?fan_landing=true)
 - [Donate via PayPal](http://bitly.com/APGDonate)
 - [Buy Me a Coffee](https://www.buymeacoffee.com/Apache)
 - [Subscribe on Twitch.TV](https://twitch.tv/ApacheGamingUK)
 - [Subscribe on YouTube](https://youtube.com/c/ApacheGamingUK)
 - [Purchase from my Amazon Wishlist](http://amzn.eu/7qvKTFu)
 - [Visit my website!](https://apachegaming.net)

## Server-Side Commands:

When updating the settings for this mod, changes may not take effect immediately. The climate systems in this game are very in-depth, and local weather can take a while to propagate.

However, weather side-effects, such as the player freezing in winter, should update immediately.

| Command													| Description |
| ---														| --- |
| **/nwn**													| Display the current settings for the mod. |
| **/nwn min [-20.0 - 40.0]**								| Ensure the server temperature will never go under this limit. |
| **/nwn max [-20.0 - 40.0]**								| Ensure the server temperature will never go over this limit. |
| **/nwn season [spring\|summer\|autumn\|winter\|auto]**	| Force the season to be spring, summer, autumn, or winter, all year round. |
| **/nwn reset**											| Reset the mod back to factory settings. |

Many things in the game are determined by the time of year, including whether a player can freeze to death; the hours of daylight per day; growing seasons, and more. Bear this in mind when forcing a world to be set within a single season. 

## Known Issues:

In testing, it has been seen that the "Outside Temperature" may not properly represent the overridden temperature values. However, this may have been a side-effect of testing the mod with a high game-speed. This will be monitored, as the mod is released, and changes can be made, if needs be.