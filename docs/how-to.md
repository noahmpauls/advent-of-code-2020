# How To

A document detailing the development process of this application.

## Scaffolding

I want to create an ASP.NET app with a Vue frontend. I used the basic startup found [here](https://developer.okta.com/blog/2018/08/27/build-crud-app-vuejs-netcore) to begin scaffolding the project:
1. I created a new ASP.NET solution and project based on the API template. The project is called "backend".
2. I created a new Vue app called "frontend" in the newly created solution folder. I included Router, Vuex, and Jest.
   
This defines the frontend and backend scaffolding for the app.

## Get Things Working

The ASP.NET template comes with a `WeatherForecast` controller, which I can use to test. I changed the route for `WeatherForecase` to have `api` prepended.

Now I need to use Axios to call the API. This requires a proxy for all API-related routes. In my backend project, I found the IIS Express port number in launch settings. I then set up `vue.config.js` to proxy to the new port number like so:

```js
module.exports = {
  devServer: {
    proxy: {
      "/api": {
        target: "http://localhost:50028/"
      },
    }
  }
}
```

Initially, this didn't work because I had SSL enabled for the API (can only use HTTPS). Using [this question](https://stackoverflow.com/questions/46507029/how-to-disable-https-in-visual-studio-2017-web-proj-asp-net-core-2-0/46507122), I disabled SSL, got the right port number, and sucessfully queried data from the API.