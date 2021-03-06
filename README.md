# Advent of Code 2020

This Advent of Code solution set is formatted as a web server. The ultimate goal is to provide a web-based interface for computing AoC solutions by uploading inputs through the frontend and doing calculations on the server side.

The website will explain the problem for each day, explain the chosen solution, and allow users to upload an input to get an answer back. One of the primary goals is to generalize the inputs as much as possible; rather than limit to the specific problem parameters, methods should be able to solve as many variants of the problem as possible, and provide visualizations where appropriate.

## Instructions for using locally

In the terminal, navigate to the `backend` folder and execute `dotnet run`. This will start the backend server.

Navigate to the `frontend` folder, and run the following:

```
npm install  // on first run
npm run serve
```

Now navigate to http://localhost:8080 to see the app in action!