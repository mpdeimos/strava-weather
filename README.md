Strava Weather [![Build status](https://ci.appveyor.com/api/projects/status/1psx9lt55cx30e8s?svg=true)](https://ci.appveyor.com/project/mpdeimos/strava-weather)
==============

ASP.NET Core MVC application that adds weather information to Strava activities.

Status
------

Currently just a proof of concept without public web UI deployed as ASP.NET Core MVC application to Heroku.

* Connects user to Strava via OAuth
* Uses IFTTT.com to get notifies for new Strava activities (POST request with Strava activity url, might be replaced by own polling mechanism in the future)
* Calculates mean location and time of the activity and queries https://darksky.net for the weather conditions
* Adds temperature and weather condition (e.g. Light Snow) toStrava activity name

TODO
----

* Public service
* Web UI with configuration

If you are interested in using the service, just ping me on GitHub or Twitter.
