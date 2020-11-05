# TvMaze Scraper API
API implemented in .Net Core using HttpClient to handle requests to the external TvMaze API, EF Core for interaction with the database and .Net's out of the box DI Container for injecting abstractions, services and objects like the Database context. 

### Database Relationships 
I have set up a many to many relationship between Actors and Shows through EF. The relation between the model objects is kept as a collection of type ActorShow.
ActorShow contains the ActorId and ShowId. This allowed me much flexibility in building the get and add logic as now I can use Joins or through EF just Include 
one of the properties. 

### Application Logic 
#### Scraping Shows
I have created a separate HttpClient class to contain all the logic in connecting and requesting TvMazeAPI(TvMazeHttpClient). It is injected in the TvMazeService
and serialisez the received json string to any type T that conforms to IAppModel. For scraping the cast I chose to send batch requests in parallel, as there could
be potentially many shows that we need the cast for. It batches requests to the cast endpoint for 3 shows, and when all have finished returns the whole cast.
One workarround that can be definetelly revisited is adding a nullable int ParentId to the IAppModels, this allows me to easily keep track of the actors show and 
to make grouping/joining decisions based on that. This property is ignored by the JsonSerializer so the response from this API will not contain this property and it is set 
internally. This also helped alot in keeping nice abstractions, because theoreitcally you can scrape any IAppModel type object this, so it can be reused 
for other endpoints and types of data.

Once we have the shows and the actors we join these based on their shared ShowId, I add the cast to the show and the collection of shows is returned to the controller

#### Adding Shows 
What I did for adding shows was focus on finding the new actors and new shows that are not in our database, with the new actors and shows we can also
check if any of the new data sent contains existing shows/actors in our database that have no relation(maybe someone new joined the cast of the show in season 3 and that actor 
was scraped before from another show/movie). We create the new actors and shows that were not in our database and then create all the new relations(of existing cast/show and new ones).
This way we can update a show/actor relation without having to specificaly find it and update it, we keep the database clean and work through a few data collections only, ensurring we dont iterate 
through too many uneeded collections or make too many calls to the database.

#### Getting Shows 
Getting shows from the DB at this point is fairly simple, we just query the table and make sure we include our Cast and Actor properties, then map them to Models.

#### Other minor key moments
I added AutoMapper so we dont have too many selects and object instantiations near the application logic. I added some basic global error handling,
now on any error the api returns a response with teh status code, exception message and stack trace. I also refactored a lot of code in a lot of places
becasue it made sense given that I have the time.

#### Potential drawbacks 
I focused too much on getting the results in the right format and working with the data in the correct format this time arround, and I seem to have 
ended up mutating a lot of objects(only ShowModels that have no relation to the storage) and this can lead up to some bad bugs here and there. 
I did this also because the way TvMaze API has its data organized its a bit hard for example to get the actors while still keeping track of the show,
the TvMaze API objects are all nested and always require a sort of Proxy Object (TvMaze Responses classes) that need to have properties named after the main json key 
in the response.
In a real life scenario do I would dedicate a lot more time to this issue and would change the services and abstractions in a way where we could have:
1. ScrapperModels that contain only data from the API as properties in the Proxy Objects.
2. Once we have the scrape models for show and actor, merge them together via their shared show Id and create new ShowModels that will be the
3. Configure the Json Serializer + Automapper to map these different named keys correctly.
full object containing the show data and cast data.

#### Time Table: 
1. Initial implementation of persistence and application code - 2 hours
1. Db Refactor and Application refactor(add many to many relations, clean up code, implement correct joins and make parallel scrape): 2 hours.
2. Automapper and Error Handling: 40 mins - 1 hour
3. Refactoring and testing: 2 hours

Run the project via the IDE or through the .Net CLI and dont forget to change the Connection String in the appsettings.json to your own local(usr name and password)
There are 3 endpoints available:
**GET api/scrape** - It will scrape the TvMazer API, it needs a query string of this structure
```json
https://localhost:5001/api/scrape?query=search/shows?q=bat&page=1&count=3
```
**GET api/show** - Will try to fetch show and cast data from storage, here the query string must be part of the name of a movie 
```json
https://localhost:5001/api/show?query=der&page=1&count=3
```

**POST api/show/add** - This is used to persist data into storage, it takes a body in the format returned by either the  /scrape or /show ednpoints, an array of show/cast objects. 
```javascript
[
    {
        "id": 33320,
        "name": "Derry Girls",
        "cast": [
            {
                "id": 128468,
                "name": "Saoirse-Monica Jackson",
                "birthday": "1993-11-24"
            },
            {
                "id": 103876,
                "name": "Tommy Tiernan",
                "birthday": "1969-06-16"
            },
            {
                "id": 14138,
                "name": "Ian McElhinney",
                "birthday": "1948-08-19"
            },
            {
                "id": 193030,
                "name": "Louisa Harland",
                "birthday": "1994-01-31"
            },
            {
                "id": 193031,
                "name": "Nicola Coughlan",
                "birthday": "1987-01-09"
            }]}]
```
### General Overview 
Given the tech tasks requirements I have decided to start building the API from the smallest parts and then tying them togheter rather than setting up a infrastructure and layering from the beggining(ussualy saves time). First I implemented the Scraper functionality: Query data from the TvMaze API and serialize it to some models and to set up some basic interfaces for services and models also set up the controllers and application settings. I use a body more often for receiving data than form query strings in the URL, so I used a POST method for fetching the data beacuse I used a body containing the query and pagination params.

This gave me a nice understading of the data I will be working with and the data the API needs to serve back.
Next I implemented the services with the logic for fetching show data, then actor data for that show and parsing it in the format specified in the task. 

Once I could scrape the data in the needed format it was time to set up the Persistance Layer: the DbContext, service and connection configurations. Here is the first time that I felt the need to start layering and creating abstractions for the service layer, data layer and application layer. These abstractions allowed me to quickly start implementing how the data is used and mapped. Here also came the need to have a IAppModel type class and a IEntityModel type class so we can differentiate between the objects for the API and the ones for the Database. This helped alot in isolating the scope of some services.

Last I refactored some of the code and made a few minor changes mostly to models and some inconstitencies with the inferfaces and their implementation. 

I intentionally left out error handling/logging and validation. I will talk about those a bit further. I tried to use more common features of C#/.Net and not get into more complex project configurations or implementations. Overall my main goal was to keep the code simple but at the same time decoupled and clean just enough so we can have the desired result, and also to use the time I had well. 
### Key Moments 
##### Structure
I have organized the project into an API project, and a couble of class Libraries: 
 - Persistance - for DatabseContex set up and migrations
 - Common - a library where some interfaces and models are stored that are widely used all over 
 - Application - this library does all the heavy lifting, its where the services are implemented and their abstractiosn are kept aslo here. This library is the bridge between the API and the Persistance layer and also provides the logic for processing the data in the correct format. 

I tried to use a combination of DI/Generics/Abstractions to decouple the layers as much as possible and also allow for a lot of flexibility, like chaging parts of the logic, adding new features or endpoints. Nice trick with interfaces is to make them have a generic parameter, this way we can register services with the same interface and when we will inject them the compiler infers the concrete implementation nicely by what type that generic parameter ended up being, other solutions to this problem include using the service provider and getting the implementation we need with a FirstOrDefault() on the service container, otherwise if my memory doesnt fail me, the DI container will choose the last implementation as the one it will inject where it finds that interface in the constructors.
This also allows us to easily mock up alot of the services and their models for unit testing. 
##### Database Design Choice
I struggled a bit with this one. I wanted from the get go models and relations that would allow me to easily construct the data in the format its supposed to be served by the API. I've forgotten a bit about the knit and grit of Entity Framework and configuring a Many to Many Relation to work nicely would make me lose a lot of time. Thats why I opted for a simpler option to have 2 separate tables for Shows and Actors and one for Cast where the rlation is kept. This allowed me to quickly query for data I needed or to persist it as well. This approach potentially is way more perfomance costly than a correct configuration of the relatiosn between the models in EF would give. The new features added in EF Core 3 for LINQ based querying are amazing, it makes really good use of JOINS and creates really nice query conditions underneath, based on the linq expression. This would be ideal case for such a dataset.
##### Performance 
Theres one place where the issue of performance and safety overall bugged me. Thats when I try to fetch the data from the MazeAPI, the API endpoints are not very rich in options and im basically pulling a potentially long string that I serialize into a collection of objects that afterwards I paginate and process. Would be nice to at least try and parse the string in parrallel and wait for it to serialize all objects safely. Same concern goes for fetching actors afterwards. 

Also because of how I chose to design the data base it ended up in me doing a lot of work with collections especially Selecting(aka map) sets of data from one type to another, or crossreference collections to map Actor/Show data and this is quite punishing performance wise, with some nice linq queries on a nicely desinged many to many data set this would be a lot more performant and a lot more prettier. One thing I would add for sure would be AutoMapper and use that for any A => B conversions and ommit the new Model syntax that is everywhere in this solution. With a bit of handy work we can create complex projections to use together with queries that would end up in keeping all the mapping logic in the Automapper Profiles, but available for use together with linq to create some really nice looking expressions.

### What dint make it
Dint have the time to properly dockerize it together with a sql container running, would have been nice and very simple to use.
I pretty much dint think too much about concretely implementing the following very important parts Error Handling, Logging and Testing. I made this decision more on the basis that I wont have enough adequte time to start implementing them. Configuring Error Handling and Loggin correcrly in .NET can take some time but it can be very handy. I first got introduced to the concepts of middleware in Node.JS and was amazed when in .NET core we could set filters on actions that execute in before various hooks especially for error handling and loggin, this allows for a very nice pattern to evolve where the handling and loggin (with the help of some abstractions and services) are kept far away from the actual logic and we can keep erorrs related to validation, content and construct nice messages based on the errors in the application layer. For validation I would always use FluentValidation, those validation query builders are so nice. 

I havent done a great amount of testing in production environments overall, but I like the idea and the benefits it brings. For example with Unit testing I really learned the importance of correct abstractions for mocking functionality and data, together with the DI solution it made much more sense then before when I would write interfaces more for the sake of writing them than actually having a need or purpose(other than injecting services into other services or controllers). With this API I tried to make correct and usefull abstractions that make the application less decoupled and would theoretically(at this point anyway) make for good interfaces on mocking up services and testing different parts of the functionality. Also would be nice to make an integration test with the TvMaze API. 
